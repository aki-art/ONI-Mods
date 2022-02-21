using FUtility;
using Klei.AI;
using System.Collections.Generic;

namespace CrittersDropBones.Effects
{
    public class StaminaRegenerationEffect : IEffect
    {
        public static readonly string ID = Mod.Prefix("StaminaRegeneration");

        public Effect Create()
        {
            var effect = new Effect(
               ID,
               STRINGS.DUPLICANTS.STATUSITEMS.CDB_STAMINAREGENERATION.NAME,
               STRINGS.DUPLICANTS.STATUSITEMS.CDB_STAMINAREGENERATION.TOOLTIP,
               120f,
               true,
               true,
               false);

            effect.SelfModifiers = new List<AttributeModifier>()
            {
                new AttributeModifier(Db.Get().Attributes.Athletics.Id, 8),
                new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 1.2f, is_multiplier: true)
            };

            return effect;
        }
    }
}
