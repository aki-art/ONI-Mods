using FUtility;

namespace CrittersDropBones.Effects
{
    public class CDBEffects
    {
        public const string STAMINA_REGENERATION = Mod.PREFIX + "StaminaRegeneration";

        public static void RegisterAll(ModifierSet modifierSet)
        {
            new EffectBuilder(STAMINA_REGENERATION, 120f, false)
                .Modifier(Db.Get().Attributes.Athletics.Id, 8)
                .Modifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.2f)
                .Add(modifierSet);

        }

    }
}
