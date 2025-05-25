namespace Twitchery.Content
{
	public class TEffects
	{
		public const float PERSISTENT = 0f;

		public const string
			CAFFEINATED = "AkisExtraTwitchEvents_CaffeinatedEffect",
			HYPER_FOCUSED = "AkisExtraTwitchEvents_HyperFocusedEffect",
			RADISH_STRENGTH = "AkisExtraTwitchEvents_RadishStrengthEffect",
			GOLDSTRUCK = "AkisExtraTwitchEvents_GoldStruck",
			STEPPEDINSLIME = "AkisExtraTwitchEvents_SteppedInSlime",
			SOAKEDINSLIME = "AkisExtraTwitchEvents_SoakedInSlime",
			DOUBLETROUBLE = "AkisExtraTwitchEvents_DoubleTrouble",
			TWITCH_GUEST = "AkisExtraTwitchEvents_TwitchGuest",
			HONEY = "AkisExtraTwitchEvents_Honey",
			SUGARHIGH = "AkisExtraTwitchEvents_SugarHigh",
			LEMON = "AkisExtraTwitchEvents_Lemon",
			COMFORT_FOOD = "AkisExtraTwitchEvents_ComfortFood",
			CURE_WEREVOLE = "AkisExtraTwitchEvents_CureWereVole",
			BIONIC_SOLAR_ZAP = "AkisExtraTwitchEvents_BionicSolarZap",
			OILED_UP = "AkisExtraTwitchEvents_OiledUp",
			HARVESTMOON = "AkisExtraTwitchEvents_HarvestMoon",
			SWEATY = "AkisExtraTwitchEvents_Sweaty",
			SUPERDUPE = "AkisExtraTwitchEvents_SuperDupe";

		public const float WORKSPEED_MULTIPLIER = 1.5f;

		public static void Register(ModifierSet set)
		{
			var db = Db.Get();
			var attributes = Db.Get().Attributes;
			var athlethics = db.Attributes.Athletics.Id;

			new EffectBuilder(CAFFEINATED, 300f, false)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, -0.05f)
				.Add(set);

			new EffectBuilder(HARVESTMOON, CONSTS.CYCLE_LENGTH * 3f, false)
				.Modifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 3f, true)
				.Add(set);

			new EffectBuilder(LEMON, 600f, false)
				.Modifier(db.Attributes.Immunity.Id, 1f)
				.Modifier(db.Attributes.GermResistance.Id, 0.5f)
				.Add(set);

			new EffectBuilder(OILED_UP, 300f, true)
				.Modifier(db.Attributes.CarryAmount.Id, -0.2f, true)
				.Add(set);

			new EffectBuilder(SWEATY, 30f, true)
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

			new EffectBuilder(COMFORT_FOOD, 300f, false)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, -0.05f, true)
				.Add(set);

			new EffectBuilder(HONEY, 300f, false)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, -0.05f)
				.Modifier(db.Amounts.HitPoints.deltaAttribute.Id, 0.05f)
				.Add(set);

			new EffectBuilder(DOUBLETROUBLE, Mod.Settings.DoubleTrouble_DurationCycles * CONSTS.CYCLE_LENGTH, false)
				.Modifier(db.Attributes.AirConsumptionRate.Id, (Mod.Settings.DoubleTrouble_OxygenConsumptionModifier / 100f) / 600f)
				.Modifier(db.Amounts.Stamina.deltaAttribute.Id, 1f)
				.Add(set);

			new EffectBuilder(TWITCH_GUEST, Mod.Settings.TwitchGuest_DurationCycles * CONSTS.CYCLE_LENGTH, false)
				.Modifier(db.Attributes.AirConsumptionRate.Id, (Mod.Settings.DoubleTrouble_OxygenConsumptionModifier / 100f) / 600f)
				.Modifier(db.Amounts.Stamina.deltaAttribute.Id, 1f)
				.Add(set);

			new EffectBuilder(BIONIC_SOLAR_ZAP, 0f, true)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, 0.2f, true)
				.Add(set);


			var superDupeBonus = 10f;

			var superDupeEffect = new EffectBuilder(SUPERDUPE, 30 * CONSTS.CYCLE_LENGTH, true)
				// additionally setting to white or the "black" will get mixed with the gradient, resulting in still black
				.Name($"<color=#FFFFFF><gradient=\"{ModAssets.TMP_GradientPresetIDs.SUPER_DUPE}\">{STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_SUPERDUPE.NAME}</gradient></color>")
				.Modifier(attributes.Athletics.Id, 20, false)
				.Modifier(attributes.Art.Id, superDupeBonus, false)
				.Modifier(attributes.Botanist.Id, superDupeBonus, false)
				.Modifier(attributes.Caring.Id, superDupeBonus, false)
				.Modifier(attributes.Construction.Id, superDupeBonus, false)
				.Modifier(attributes.Cooking.Id, superDupeBonus, false)
				.Modifier(attributes.Digging.Id, superDupeBonus, false)
				.Modifier(attributes.Learning.Id, superDupeBonus, false)
				.Modifier(attributes.Machinery.Id, superDupeBonus, false)
				.Modifier(attributes.Ranching.Id, superDupeBonus, false)
				.Modifier(attributes.Strength.Id, superDupeBonus, false);

			if (DlcManager.FeatureClusterSpaceEnabled())
				superDupeEffect
					.Modifier(attributes.SpaceNavigation.Id, superDupeBonus, false);

			if (Mod.isBeachedHere)
				superDupeEffect
					.Modifier("Beached_Precision", superDupeBonus, false);

			superDupeEffect.Add(set);
		}
	}
}
