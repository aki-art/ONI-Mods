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
                .Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT.TOOLTIP)
                .Modifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.05f)
                .Add(set);

            new EffectBuilder(RADISH_STRENGTH, 600f, false)
                .Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_RADISHSTRENGTHEFFECT.TOOLTIP)
                .Modifier(Db.Get().Attributes.Strength.Id, 3)
                .Add(set);
        }
    }
}
