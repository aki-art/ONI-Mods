using FUtility;
using Klei.AI;
using System;
using System.Collections.Generic;
using static DecorPackB.STRINGS.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS;

namespace DecorPackB.Content
{
    public class DPEffects
    {
        public const string INSPIRED_LOW = "DecorpackB_Inspired_Low";
        public const string INSPIRED_OKAY = "DecorpackB_Inspired_Okay";

        internal static void Register(ModifierSet instance)
        {
        }

        public const string INSPIRED_GREAT = "DecorpackB_Inspired_Great";
        public const string INSPIRED_GIANT = "DecorpackB_Inspired_Giant";

        public static List<Effect> GetEffectsList()
        {
            return new List<Effect>
                {
                    new Effect(INSPIRED_LOW, NAME1, TOOLTIP, 60f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 1)
                        }
                    },
                    new Effect(INSPIRED_OKAY, NAME2, TOOLTIP, 60f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 2)
                        }
                    },
                    new Effect(INSPIRED_GREAT, "Inspired III", TOOLTIP, 60f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 4)
                        }
                    },
                    new Effect(INSPIRED_GIANT, NAME3, TOOLTIP, Consts.CYCLE_LENGTH, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 6)
                        }
                    },
                };
        }
    }
}
