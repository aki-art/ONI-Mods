using FUtility;
using Harmony;
using Klei;
using Klei.AI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TransparentAluminium
{
    class Patches
    {
        internal static Hashtable subList;
        internal static Material solidMaterial;

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }


        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                Buildings.RegisterBuildings(
                    typeof(TransparentAluminiumTileConfig),
                    typeof(LogicLightSensorConfig));
            }
        }
        [HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
        class LegacyModMain_ConfigElements_Patch
        {
            private static void Postfix()
            {
                Strings.Add("STRINGS.DUPLICANTS.ATTRIBUTES.ARMORED.NAME", STRINGS.ELEMENTS.MATERIAL_MODIFIERS.ARMORED);
                Db.Get().BuildingAttributes.Add(ModAssets.HardnessAttribute);
                Element alon = ElementLoader.FindElementByHash(ModAssets.transparentAluminumHash);
                alon.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, 350, alon.name));
                alon.attributeModifiers.Add(new AttributeModifier(ModAssets.HardnessAttribute.Id, 350, alon.name));

            }
        }

        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                KPrefabID kPrefabID = __instance.GetComponent<KPrefabID>();
                if(!kPrefabID.HasTag(GameTags.Bunker) &&
                    __instance.GetComponent<PrimaryElement>().ElementID == ModAssets.transparentAluminumHash)
                {
                    kPrefabID.AddTag(GameTags.Bunker);
                }
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        class ElementLoader_CollectElementsFromYAML_Patch
        {
            private static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                Strings.Add("STRINGS.ELEMENTS.TRANSPARENTALUMINUM.NAME", STRINGS.ELEMENTS.TRANSPARENTALUMINUM.NAME);
                Strings.Add("STRINGS.ELEMENTS.TRANSPARENTALUMINUM.DESC", STRINGS.ELEMENTS.TRANSPARENTALUMINUM.DESC);

                string elementListText = File.ReadAllText(Path.Combine(ModAssets.ModPath, "assets", "elements.txt"));
                ElementLoader.ElementEntryCollection elementList = YamlIO.Parse<ElementLoader.ElementEntryCollection>(elementListText, new FileHandle());

                __result.AddRange(elementList.elements);

                subList.Add(ModAssets.transparentAluminumHash, TransparentAluminum.SlagGlassElement(solidMaterial));
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        private static class Patch_ElementLoader_Load
        {
            private static void Prefix(ref Hashtable substanceList, SubstanceTable substanceTable)
            {
                subList = substanceList;
                solidMaterial = substanceTable.solidMaterial;
            }
        }

        [HarmonyPatch(typeof(KilnConfig), "ConfigureBuildingTemplate")]
        public static class KilnConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                Tag oxylite = ElementLoader.FindElementByHash(SimHashes.OxyRock).tag;
                Tag aluminum = ElementLoader.FindElementByHash(SimHashes.Aluminum).tag;

                var ingredients = new ComplexRecipe.RecipeElement[2] {
                    new ComplexRecipe.RecipeElement(oxylite, 50f),
                    new ComplexRecipe.RecipeElement(aluminum, 150f)
                };

                var results = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement("TransparentAluminum", 25f)
                };

                string str = ComplexRecipeManager.MakeRecipeID("Kiln", ingredients, results);

                new ComplexRecipe(str, ingredients, results)
                {
                    time = 40f,
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                    description = string.Format(global::STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, ElementLoader.GetElement(results[0].material).name, ElementLoader.GetElement(ingredients[0].material).name),
                    fabricators = new List<Tag>()
                    {
                        TagManager.Create("Kiln")
                    }
                };

            }
        }
    }
}