using FUtility;
using static STRINGS.DUPLICANTS.MODIFIERS;

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
			SUGARHIGH = "AkisExtraTwitchEvents_SugarHigh";

		
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
				.Modifier(athlethics, -6f)
				.Add(set);

			new EffectBuilder(SOAKEDINSLIME, 1f, true)
				.Modifier(athlethics, -10f)
				.Add(set);

			new EffectBuilder(SUGARHIGH, 300f, false)
				.Modifier(athlethics, 2f)
				.Add(set);

			new EffectBuilder(DOUBLETROUBLE, ModTuning.DOUBLE_TROUBLE_DURATION, false)
				.Modifier(db.Attributes.AirConsumptionRate.Id, -0.05f)
				.Add(set);
		}
	}
}
