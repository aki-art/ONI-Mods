using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;

namespace SpookyPumpkin
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                Effect spookedEffect = new Effect(
                ModAssets.spookedEffectID,
                STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME,
                STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP,
                120f,
                true,
                true,
                false);

                spookedEffect.SelfModifiers = new List<AttributeModifier>() {
                    new AttributeModifier(Db.Get().Attributes.Athletics.Id, 8),
                    new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, 2f / 3f)
                };

                Effect holidaySpiritEffect = new Effect(
                ModAssets.holidaySpiritEffectID,
                STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.NAME,
                STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.TOOLTIP,
                360f,
                true,
                true,
                false);

                holidaySpiritEffect.SelfModifiers = new List<AttributeModifier>() {
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
                };

                __instance.effects.Add(spookedEffect);
                __instance.effects.Add(holidaySpiritEffect);
            }
        }
    }
}
