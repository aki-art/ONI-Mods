using FUtility;

namespace SpookyPumpkinSO.Content
{
	public class SPEffects
	{
		public const string
			SPOOKED = "SP_Spooked",
			HOLIDAY_SPIRIT = "AHM_HolidaySpirit",
			GHASTLY = "SP_Ghastly",
			PUMPKINED = "SP_Pumpkined",
			SUGARSICKNESS_RECOVERY = "SP_SugarSicknessRecovery";

		public static void Register(ModifierSet parent)
		{
			var attributes = Db.Get().Attributes;
			var amounts = Db.Get().Amounts;

			new EffectBuilder(SUGARSICKNESS_RECOVERY, CONSTS.CYCLE_LENGTH, false)
				.HideInUI()
				.HideFloatingText()
				.Add(parent);

			new EffectBuilder(SPOOKED, 120f, false)
				.Name(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.NAME)
				.Description(STRINGS.DUPLICANTS.STATUSITEMS.SPOOKED.TOOLTIP)
				.Modifier(attributes.Athletics.Id, 8)
				.Modifier(amounts.Bladder.deltaAttribute.Id, 2f / 3f)
				.Add(parent);

			var holidaySpirit = new EffectBuilder(HOLIDAY_SPIRIT, 360f, false)
				.Name(STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.NAME)
				.Description(STRINGS.DUPLICANTS.STATUSITEMS.HOLIDAY_SPIRIT.TOOLTIP)
				.Modifier(attributes.Athletics.Id, 1)
				.Modifier(attributes.Art.Id, 1)
				.Modifier(attributes.Botanist.Id, 1)
				.Modifier(attributes.Construction.Id, 1)
				.Modifier(attributes.Caring.Id, 1)
				.Modifier(attributes.Learning.Id, 1)
				.Modifier(attributes.Machinery.Id, 1)
				.Modifier(attributes.Strength.Id, 1)
				.Modifier(attributes.Ranching.Id, 1)
				.Modifier(attributes.Cooking.Id, 1)
				.Modifier(attributes.Digging.Id, 1);

			if (DlcManager.IsExpansion1Active())
				holidaySpirit.Modifier(attributes.SpaceNavigation.Id, 1);

			holidaySpirit.Add(parent);

			new EffectBuilder(PUMPKINED, CONSTS.CYCLE_LENGTH, false)
				.HideInUI()
				.HideFloatingText()
				.Add(parent);

			new EffectBuilder(GHASTLY, 0, false)
				.Name(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.NAME)
				.Description(STRINGS.DUPLICANTS.STATUSITEMS.GHASTLY.TOOLTIP)
				.HideFloatingText()
				.HideInUI()
				.Modifier(amounts.Stress.deltaAttribute.Id, -(Mod.Config.GhastlyStressBonus) / CONSTS.CYCLE_LENGTH)
				.Add(parent);
		}
	}
}
