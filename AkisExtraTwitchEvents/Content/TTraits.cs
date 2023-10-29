using Klei.AI;
using Twitchery.Content.Defs.Critters;
using Twitchery.Content.Scripts;

namespace Twitchery.Content
{
	public class TTraits
	{
		public const string
			ANGRY = "AkisExtraTwitchEvents_Trait_Angry",
			WEREVOLE = "AkisExtraTwitchEvents_Trait_WereVole",
			PIP_ROOKIE1 = "AkisExtraTwitchEvents_Trait_Pip_Rookie_1",
			PIP_ROOKIE2 = "AkisExtraTwitchEvents_Trait_Pip_Rookie_2";

		public static void Register()
		{
			var trait = Db.Get().CreateTrait(
				ANGRY,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ANGRY.NAME,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ANGRY.SHORT_DESC,
				null,
				true,
				null,
				false,
				false);

			trait.Add(new AttributeModifier(
				Db.Get().Attributes.Strength.Id,
				20,
				trait.Name));

			trait.Add(new AttributeModifier(
				Db.Get().Amounts.HitPoints.maxAttribute.Id,
				1,
				trait.Name,
				true));

			if (DlcManager.FeatureRadiationEnabled())
				trait.Add(new AttributeModifier(
					Db.Get().Attributes.RadiationResistance.Id,
					0.66f,
					trait.Name));

			trait.OnAddTrait += go =>
			{
				go.AddOrGet<AngryTrait>();
			};


			var wereVoleTrait = Db.Get().CreateTrait(
				WEREVOLE,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_WEREVOLE.NAME,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_WEREVOLE.SHORT_DESC,
				null,
				true,
				null,
				true,
				false);

			wereVoleTrait.Add(new AttributeModifier(
				Db.Get().Attributes.Digging.Id,
				10,
				wereVoleTrait.Name,
				false));
			var rookie1 = Db.Get().CreateTrait(
				PIP_ROOKIE1,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ROOKIE.NAME,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ROOKIE.SHORT_DESC,
				null,
				true,
				RegularPipConfig.Level1ChoreGroups().ToArray(),
				false,
				false);

			var rookie2 = Db.Get().CreateTrait(
				PIP_ROOKIE2,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ROOKIE.NAME,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_ROOKIE.SHORT_DESC,
				null,
				true,
				RegularPipConfig.Level2ChoreGroups().ToArray(),
				false,
				false);
		}
	}
}
