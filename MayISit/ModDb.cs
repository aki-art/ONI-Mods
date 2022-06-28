using FUtility;
using Klei.AI;
using System.Collections.Generic;

namespace MayISit
{
    public class ModDb
    {
        public class Effects
        {
            public const string LOUNGED = "MayISit_Lounged";
            public const string RECENTLY_LOUNGED = "MayISit_RecentlyLounged";
            public const string RELAXING = "MayISit_Relaxing";

            public static void Register(ModifierSet set)
            {
                var loungedEffect = new Effect(
                LOUNGED,
                STRINGS.DUPLICANTS.STATUSITEMS.LOUNGED.NAME,
                STRINGS.DUPLICANTS.STATUSITEMS.LOUNGED.TOOLTIP,
                660f,
                false,
                true,
                false);

                loungedEffect.SelfModifiers = new List<AttributeModifier>() 
                {
                    new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 1)
                };

                set.effects.Add(loungedEffect);

                var recentlyLoungedEffect = new Effect(
                RECENTLY_LOUNGED,
                STRINGS.DUPLICANTS.STATUSITEMS.LOUNGED.NAME,
                STRINGS.DUPLICANTS.STATUSITEMS.LOUNGED.TOOLTIP,
                300f,
                false,
                false,
                false);

                set.effects.Add(recentlyLoungedEffect);

                var relaxingEffect = new Effect(
                RELAXING,
                "Relaxing",
                STRINGS.DUPLICANTS.STATUSITEMS.LOUNGED.TOOLTIP,
                0,
                true,
                false,
                false);

                relaxingEffect.SelfModifiers = new List<AttributeModifier>()
                {
                    new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -5f / Consts.CYCLE_LENGTH, "Relaxing")
                };

                set.effects.Add(relaxingEffect);
            }
        }
    }
}
