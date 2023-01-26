namespace CrittersDropBones.Content
{
    public class CDBEffects
    {
        public const string STAMINA_REGENERATION = "CrittersDropBones_StaminaRegeneration";
        public const string CHILL_FOOD = "CrittersDropBones_ChillFood";
        public const string HOT_FOOD = "CrittersDropBones_HotFood";
        public const string UPSET_STOMACH = "CrittersDropBones_UpsetStomach";

        public static void RegisterAll(ModifierSet modifierSet)
        {
            new EffectBuilder(STAMINA_REGENERATION, 120f, false)
                .Modifier(Db.Get().Attributes.Athletics.Id, 8)
                .Modifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.2f)
                .Add(modifierSet);

            new EffectBuilder(CHILL_FOOD, 120f, false)
                .Modifier(Db.Get().Attributes.ScaldingThreshold.Id, 0.1f)
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
