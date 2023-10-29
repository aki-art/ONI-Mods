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
			DOUBLETROUBLE = "AkisExtraTwitchEvents_DoubleTrouble",
			HONEY = "AkisExtraTwitchEvents_Honey",
			SUGARHIGH = "AkisExtraTwitchEvents_SugarHigh",
			CURE_WEREVOLE = "AkisExtraTwitchEvents_CureWereVole";

		public const float WORKSPEED_MULTIPLIER = 1.5f;

		public static void Register(ModifierSet set)
		{
			var db = Db.Get();
			var athlethics = db.Attributes.Athletics.Id;

			new EffectBuilder(CAFFEINATED, 300f, false)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, -0.05f)
				.Add(set);

			new EffectBuilder(RADISH_STRENGTH, 600f, false)
				.Modifier(db.Attributes.Strength.Id, 3)
				.Add(set);

			new EffectBuilder(GOLDSTRUCK, 600f, true)
				.Add(set);

			new EffectBuilder(STEPPEDINSLIME, 1f, true)
				.Modifier(athlethics, -1f, true)
				.Modifier(athlethics, -6)
				.Add(set);

			new EffectBuilder(SOAKEDINSLIME, 1f, true)
				.Modifier(athlethics, -1f, true)
				.Modifier(athlethics, -10)
				.Add(set);

			new EffectBuilder(SUGARHIGH, 300f, false)
				.Modifier(athlethics, 2f)
				.Add(set);

			new EffectBuilder(HONEY, 300f, false)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, -0.05f)
				.Modifier(db.Amounts.HitPoints.deltaAttribute.Id, 0.05f)
				.Add(set);

			new EffectBuilder(DOUBLETROUBLE, Mod.Settings.DoubleTrouble_DurationCycles * CONSTS.CYCLE_LENGTH, false)
				.Modifier(db.Attributes.AirConsumptionRate.Id, (Mod.Settings.DoubleTrouble_OxygenConsumptionModifier / 100f) / 600f)
				.Modifier(db.Amounts.Stamina.deltaAttribute.Id, 1f)
				.Add(set);
		}
	}
}
