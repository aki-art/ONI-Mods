using FUtility;
using Harmony;
using Klei.AI;
using SpookyPumpkin.Foods;
using SpookyPumpkin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TUNING;
using static ComplexRecipe;

namespace SpookyPumpkin
{
    class PumpkinPatches
    {
        [HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
        public static class GlobalAssets_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<string, string> ___SoundTable)
            {
                foreach (var item in ___SoundTable)
                {
                    Log.Debuglog($"{item.Key} | {item.Value}");
                }
            }
        }

        // Transpiles a method to display what fertilizer is needed for a plant, so it supports non-element fertilizers
        [HarmonyPatch(typeof(MinionVitalsPanel), "GetFertilizationLabel")]
        public static class MinionVitalsPanel_GetFertilizationLabel_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var getElement = AccessTools.Method(typeof(ElementLoader), "GetElement", new Type[] { typeof(Tag) });
                var getProperName = AccessTools.Method(typeof(GameTagExtensions), "ProperNameStripLink", new Type[] { typeof(Tag) });

                var codes = orig.ToList();
                var index = codes.FindIndex(c => c.operand is MethodInfo m && m == getElement);

                codes[index] = new CodeInstruction(OpCodes.Call, getProperName);
                codes.RemoveAt(index + 1);

                return codes;
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

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(typeof(SpookyPumpkinConfig));
            }
        }

        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                var effect = new Effect(
                    id: ModAssets.spookedEffectID,
                    name: STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME,
                    description: STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP,
                    duration: 120f,
                    show_in_ui: true,
                    trigger_floating_text: true,
                    is_bad: false)
                {
                    SelfModifiers = new List<AttributeModifier>() {
                        new AttributeModifier(Db.Get().Attributes.Athletics.Id, 8),
                        new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, 2f / 3f)
                    }
                };

                __instance.effects.Add(effect);
            }
        }

        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 4f * 600.0f, 1));
                GameTags.MaterialBuildingElements.Add(ModAssets.buildingPumpkinTag);
            }
        }
    }
}
