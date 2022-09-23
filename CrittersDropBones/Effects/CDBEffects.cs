using Klei.AI;
using System.Collections.Generic;
using static CrittersDropBones.STRINGS.DUPLICANTS.STATUSITEMS;

namespace CrittersDropBones.Effects
{
    public class CDBEffects
    {
        public const string STAMINA_REGENERATION = Mod.PREFIX + "StaminaRegeneration";

        public static void RegisterAll(ModifierSet modifierSet)
        {
            var effect = new Effect(
               STAMINA_REGENERATION,
               CDB_STAMINAREGENERATION.NAME,
               CDB_STAMINAREGENERATION.TOOLTIP,
               120f,
               true,
               true,
               false);

            effect.SelfModifiers = new List<AttributeModifier>()
            {
                new AttributeModifier(Db.Get().Attributes.Athletics.Id, 8),
                new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 1.2f, is_multiplier: true)
            };
        }

    }
}
