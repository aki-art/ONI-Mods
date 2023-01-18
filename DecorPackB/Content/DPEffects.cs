using FUtility;

namespace DecorPackB.Content
{
    public class DPEffects
    {
        public const string INSPIRED_LOW = "DecorpackB_Inspired_Low";
        public const string INSPIRED_OKAY = "DecorpackB_Inspired_Okay";
        public const string INSPIRED_GREAT = "DecorpackB_Inspired_Great";
        public const string INSPIRED_GIANT = "DecorpackB_Inspired_Giant";
        public const float INSPIRED_DURATION = 60f;

        public static void Register(ModifierSet instance)
        {
            var learning = Db.Get().Attributes.Learning.Id;

            new EffectBuilder(INSPIRED_LOW, INSPIRED_DURATION, false)
                .Modifier(learning, 1)
                .Add(instance);

            new EffectBuilder(INSPIRED_OKAY, INSPIRED_DURATION, false)
                .Modifier(learning, 2)
                .Add(instance);

            new EffectBuilder(INSPIRED_GREAT, INSPIRED_DURATION, false)
                .Modifier(learning, 4)
                .Add(instance);

            new EffectBuilder(INSPIRED_GIANT, Consts.CYCLE_LENGTH, false)
                .Modifier(learning, 6)
                .Add(instance);
        }
    }
}
