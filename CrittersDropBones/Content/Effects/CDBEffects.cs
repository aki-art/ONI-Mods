using FUtility;

namespace CrittersDropBones.Effects
{
    public class CDBEffects
    {
        public const string STAMINA_REGENERATION = Mod.PREFIX + "StaminaRegeneration";
        public const string CHILL_FOOD = Mod.PREFIX + "ChillFood";
        public const string HOT_FOOD = Mod.PREFIX + "HotFood";
        public const string UPSET_STOMACH = Mod.PREFIX + "UpsetStomach";

        public static void RegisterAll(ModifierSet modifierSet)
        {
            new EffectBuilder(STAMINA_REGENERATION, 120f, false)
                .Modifier(Db.Get().Attributes.Athletics.Id, 8)
                .Modifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.2f)
                .Add(modifierSet);

            new EffectBuilder(CHILL_FOOD, 120f, false)
                .Modifier(Db.Get().Attributes.ScaldingThreshold.Id, 4)
                //.Modifier(Db.Get().Amounts.Temperature.deltaAttribute.Id, -0.1f)
                .Modifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -5f / Consts.CYCLE_LENGTH)
                .Add(modifierSet);

            new EffectBuilder(HOT_FOOD, 120f, false)
                //.Modifier(Db.Get().Amounts.Temperature.deltaAttribute.Id, 0.1f)
                .Modifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 0.5f)
                .Modifier(Db.Get().Attributes.Immunity.Id, 10f)
                .Add(modifierSet);

            new EffectBuilder(UPSET_STOMACH, 120f, false)
                .Modifier(Db.Get().Attributes.ToiletEfficiency.Id, -50f)
                .Add(modifierSet);
        }
    }
}
