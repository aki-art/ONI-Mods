using FUtility;
using Klei.AI;

namespace SpookyPumpkinSO.Content.Sicknesses
{
	public class SugarSickness : Sickness
	{
		public const string ID = "SP_Sickness_SugarSickness";
		public const float IMMUNE_ATTACK_STRENGTH = 0.00025f;
		public const float DURATION = CONSTS.CYCLE_LENGTH;
		public const float VOMIT_FREQUENCY = 200f;

		public SugarSickness() : base(
			ID,
			SicknessType.Ailment,
			Severity.Minor,
			IMMUNE_ATTACK_STRENGTH,
			new(),
			DURATION,
			SPEffects.SUGARSICKNESS_RECOVERY)
		{
			AddSicknessComponent(new CommonSickEffectSickness());

			var attributes = Db.Get().Attributes;

			AddSicknessComponent(new AttributeModifierSickness(new[]
			{
				new AttributeModifier(attributes.ToiletEfficiency.Id, -0.2f),
				new AttributeModifier(attributes.Athletics.Id, -4),
				new AttributeModifier(attributes.QualityOfLife.Id, -2)
			}));


			AddSicknessComponent(new AnimatedSickness(new HashedString[1]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));

			AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 10f));
		}
	}
}
