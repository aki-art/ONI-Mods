using Harmony;
using KMod;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asphalt
{
    class AsphaltPatches
    {
        public static string ModPath { get; set; }
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Log.Info("Loaded Asphalt Tiles version " + typeof(Log).Assembly.GetName().Version.ToString());
                SettingsManager.Initialize();
                ModPath = path;
                ModAssets.LoadAll();
            }
        }

        // Add building strings, add it to building plan
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                string ID = AsphaltConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID}.NAME", AsphaltConfig.NAME);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID}.EFFECT", AsphaltConfig.EFFECT);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID}.DESC", AsphaltConfig.DESC);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.OILREFINERY.EFFECT", $"Converts {UI.FormatAsLink("Crude Oil", "CRUDEOIL")} into {UI.FormatAsLink("Petroleum", "PETROLEUM")}, {UI.FormatAsLink("Bitumen", "BITUMEN")} and {UI.FormatAsLink("Natural Gas", "METHANE")}.");

                ModUtil.AddBuildingToPlanScreen("Base", AsphaltConfig.ID);
            }
        }

        // Add Bitumen as edible for hatches
        [HarmonyPatch(typeof(BaseHatchConfig), "BasicRockDiet")]
        public static class HatchConfig_BasicRockDiet_Patch
        {
            public static void Postfix(List<Diet.Info> __result)
            {
                __result[0].consumedTags.Add(SimHashes.Bitumen.CreateTag());
            }
        }


        // Adding Asphalt tiles to the research tree.
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var techList = new List<string>(Database.Techs.TECH_GROUPING["ImprovedCombustion"])
                {
                    AsphaltConfig.ID
                };

                Database.Techs.TECH_GROUPING["ImprovedCombustion"] = techList.ToArray();

                // Translation support not yet added, but planned for future
                // Localization.RegisterForTranslation(typeof(ASPHALT_MOD));
                // Localization.GenerateStringsTemplate(typeof(ASPHALT_MOD), Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
        }

        // Making the Oil refinery produce bitumen
        [HarmonyPatch(typeof(OilRefineryConfig), "ConfigureBuildingTemplate")]
        public static class OilRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                ElementDropper elementDropper = go.AddComponent<ElementDropper>();
                elementDropper.emitMass = 100f;
                elementDropper.emitTag = new Tag("Bitumen");
                elementDropper.emitOffset = Vector3.zero;

                ElementConverter elementConverter = go.AddOrGet<ElementConverter>();

                var bitumenOutput = new ElementConverter.OutputElement(
                    kgPerSecond: 5f,
                    element: SimHashes.Bitumen,
                    minOutputTemperature: 348.15f,
                    useEntityTemperature: false,
                    storeOutput: true,
                    outputElementOffsetx: 0,
                    outputElementOffsety: 1f);

                Array.Resize(ref elementConverter.outputElements, elementConverter.outputElements.Length + 1);
                elementConverter.outputElements[elementConverter.outputElements.GetUpperBound(0)] = bitumenOutput;
            }
        }

        // Fixing up bitumens looks
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        private static class Patch_ElementLoader_Load
        {
            private static SubstanceTable subTable;

            private static void Prefix(SubstanceTable substanceTable)
            {
                subTable = substanceTable;
            }

            private static void Postfix()
            {

                Tag phaseTag = TagManager.Create("Solid");
                var bitumen = ElementLoader.FindElementByHash(SimHashes.Bitumen);

                // Assigning appropiate tags
                bitumen.materialCategory = CreateMaterialCategoryTag(phaseTag, GameTags.ManufacturedMaterial.ToString()); // This tag is for storage
                bitumen.oreTags = new Tag[] {
                    GameTags.ManufacturedMaterial, // This tag is for the autosweeper
                    GameTags.BuildableAny, // This tag is for any building material category (Tempshift Plates or Wallpapers use this)
                    GameTags.Solid // This tag is to mark generic solids.
                };

                // Pickupable bitumen art
                KAnimFile animFile = Assets.Anims.Find(anim => anim.name == "solid_bitumen_kanim");

                // Assigning new material and texture
                if (ModAssets.bitumenSubstanceTexture == null)
                {
                    Log.Warning("Texture file not found for Bitumen.");
                    return;
                }

                var material = subTable.solidMaterial;
                material.mainTexture = ModAssets.bitumenSubstanceTexture;
                var bitumenElementColor = Util.ColorFromHex(SettingsManager.Settings.BitumenColor);

                try
                {
                    Substance bitumensubstance = ModUtil.CreateSubstance(
                        name: "Bitumen",
                        state: Element.State.Solid,
                        kanim: animFile,
                        material: material,
                        colour: bitumenElementColor,
                        ui_colour: bitumenElementColor,
                        conduit_colour: bitumenElementColor
                        );

                    bitumen.substance = bitumensubstance;
                }
                catch (Exception e)
                {
                    Log.Error("Could not assign new material to Bitumen element: " + e);
                }
            }
        }


        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                // Adjusts tile speed settings runtime if the user saved configuration and didn't restart yet
                if (SettingsManager.TempSettings.UpdateMovementMultiplierRunTime)
                    if (__instance.name == AsphaltConfig.ID + "Complete")
                        __instance.GetComponent<SimCellOccupier>().movementSpeedMultiplier = FSpeedSlider.MapValue(SettingsManager.Settings.SpeedMultiplier);

                // Stops bitumen production runtime for newly spawned oil refineries
                if (SettingsManager.TempSettings.HaltBitumenProduction)
                    if (__instance.name == OilRefineryConfig.ID + "Complete")
                    {
                        Log.Debuglog("Halting bitumen production");
                        ElementConverter elementConverter = __instance.GetComponent<ElementConverter>();

                        int keyIndex = Array.FindIndex(elementConverter.outputElements, w => w.elementHash == SimHashes.Bitumen);
                        var outPutElementList = new List<ElementConverter.OutputElement>(elementConverter.outputElements);
                        outPutElementList.RemoveAt(keyIndex);
                        elementConverter.outputElements = outPutElementList.ToArray();
                    }
            }
        }

        private static Tag CreateMaterialCategoryTag(Tag phaseTag, string materialCategoryField)
        {
            if (string.IsNullOrEmpty(materialCategoryField))
                return phaseTag;

            return TagManager.Create(materialCategoryField);
        }

        // Injecting the Settings Button to the mods menu
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods, ModsScreen __instance)
            {
                foreach (var modEntry in (IEnumerable)___displayedMods)
                {
                    int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
                    Mod mod = Global.Instance.modManager.mods[index];

                    // checks if the current mod entry is this mod
                    if (index >= 0 && mod.file_source.GetRoot() == ModPath)
                    {
                        Transform transform = Traverse.Create(modEntry).Field("rect_transform").GetValue<RectTransform>();
                        if (transform != null)
                        {
                            // find an existing subscription button to copy
                            KButton subButton = null;
                            foreach (Transform child in transform)
                            {
                                if (child.gameObject.name == "ManageButton")
                                    subButton = child.gameObject.GetComponent<KButton>();
                            }

                            // copy the subscription button
                            KButton configButton = UIHelper.MakeKButton(
                                info: new UIHelper.ButtonInfo(
                                    text: "Settings",
                                    action: new System.Action(OpenModSettingsScreen),
                                    font_size: 14),
                                buttonPrefab: subButton.gameObject,
                                parent: subButton.transform.parent.gameObject,
                                index: subButton.transform.GetSiblingIndex() - 1);
                        }
                    }
                }

            }

            private static void OpenModSettingsScreen()
            {
                if (ModAssets.Prefabs.modSettingsScreenPrefab == null)
                {
                    Log.Warning("Could not display UI: Mod Settings screen prefab is null.");
                    return;
                }

                Transform parent = UIHelper.GetACanvas("AsphaltModSettings").transform;
                GameObject settingsScreen = UnityEngine.Object.Instantiate(ModAssets.Prefabs.modSettingsScreenPrefab.gameObject, parent);
                ModSettingsScreen settingsScreenComponent = settingsScreen.AddComponent<ModSettingsScreen>();
                settingsScreenComponent.ShowDialog();
            }
        }

    }
}