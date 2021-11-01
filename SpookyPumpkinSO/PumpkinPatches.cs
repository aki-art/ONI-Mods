using FUtility;
using HarmonyLib;
using Klei.AI;
using SpookyPumpkinSO.Foods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TUNING;
using UnityEngine;

namespace SpookyPumpkinSO
{
    class PumpkinPatches
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);
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

        // inserts building into build menu after a given other building
        private static void AddToBuildMenu(string menu, string id, string afterId)
        {
            if (menu.IsNullOrWhiteSpace()) return;

            IList<string> category = BUILDINGS.PLANORDER.Find(x => x.category == menu).data;
            int index = category.IndexOf(afterId);
            if (index != -1)
            {
                category.Insert(index + 1, id);
                return;
            }

            ModUtil.AddBuildingToPlanScreen(menu, id);
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                AddToBuildMenu("Furniture", SpookyPumpkinConfig.ID, FloorLampConfig.ID);
            }
        }

        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                var spookedEffect = new Effect(
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

                var holidaySpiritEffect = new Effect(
                    id: ModAssets.holidaySpiritEffectID,
                    name: STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.NAME,
                    description: STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.TOOLTIP,
                    duration: 360f,
                    show_in_ui: true,
                    trigger_floating_text: true,
                    is_bad: false)
                {
                    SelfModifiers = new List<AttributeModifier>() {
                        new AttributeModifier(Db.Get().Attributes.Athletics.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Art.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Botanist.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Construction.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Caring.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Learning.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Machinery.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Strength.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Ranching.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Cooking.Id, 1),
                        new AttributeModifier(Db.Get().Attributes.Digging.Id, 1)
                    }
                };

                __instance.effects.Add(spookedEffect);
                __instance.effects.Add(holidaySpiritEffect);
            }
        }

        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 4f * 600.0f, 1));
            }

            public static void Postfix()
            {
                if(ModAssets.pipTreats != null) 
                {
                    foreach(Tag tag in ModAssets.pipTreats)
                    {
                        GameObject obj = Assets.TryGetPrefab(tag);
                        if (obj != null)
                        {
                            obj.AddTag(ModAssets.luxuryFood);
                        }
                    }
                }
                else
                {
                    Log.Warning("PIP TREATS IS NULL");
                }
            }
        }

        
        // Fixes the display what fertilizer is needed for a plant, so it supports non-element fertilizers
        // TODO: what if another mod does this?
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
        
    }
}
