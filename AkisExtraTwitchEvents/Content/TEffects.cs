using FUtility;

namespace Twitchery.Content
{
    public class TEffects
    {
        public const string CAFFEINATED = "AkisExtraTwitchEvents_CaffeinatedEffect";
        public const string HYPER_FOCUSED = "AkisExtraTwitchEvents_HyperFocusedEffect";
        public const string RADISH_STRENGTH = "AkisExtraTwitchEvents_RadishStrengthEffect";

        public static void Register(ModifierSet set)
        {
            new EffectBuilder(CAFFEINATED, 300f, false)
                .Modifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.1f)
                .Add(set);

            new EffectBuilder(RADISH_STRENGTH, 600f, false)
                .Modifier(Db.Get().Attributes.Strength.Id, 3)
                .Add(set);
        }
    }
}
