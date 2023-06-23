using FUtility;

namespace Twitchery.Content
{
    public class TEffects
    {
        public const string
            CAFFEINATED = "AkisExtraTwitchEvents_CaffeinatedEffect",
            HYPER_FOCUSED = "AkisExtraTwitchEvents_HyperFocusedEffect",
            RADISH_STRENGTH = "AkisExtraTwitchEvents_RadishStrengthEffect",
            GOLDSTRUCK = "AkisExtraTwitchEvents_GoldStruck",
            STEPPEDINSLIME = "AkisExtraTwitchEvents_SteppedInSlime",
            SOAKEDINSLIME = "AkisExtraTwitchEvents_SoakedInSlime",
            DOUBLETROUBLE = "AkisExtraTwitchEvents_DoubleTrouble";

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

			new EffectBuilder(GOLDSTRUCK, 600f, true)
				.Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_GOLDSTRUCK.TOOLTIP)
				.Add(set);

            new EffectBuilder(STEPPEDINSLIME, 100f, true)
                .Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_STEPPEDINSLIME.TOOLTIP)
                .Modifier(Db.Get().Attributes.Athletics.Id, -4f)
                .Add(set);

			new EffectBuilder(SOAKEDINSLIME, 100f, true)
				.Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_SOAKEDINSLIME.TOOLTIP)
				.Modifier(Db.Get().Attributes.Athletics.Id, -8f)
				.Add(set);

			new EffectBuilder(DOUBLETROUBLE, ModTuning.DOUBLE_TROUBLE_DURATION, false)
				.Description(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_DOUBLETROUBLE.TOOLTIP)
				.Add(set);
		}
    }
}
