using Klei.AI;
using Twitchery.Content.Scripts;

namespace Twitchery.Content
{
	public class TTraits
	{
		public const string ANGRY = "AkisExtraTwitchEvents_Trait_Angry";

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
				trait.Name,
				false));

			trait.Add(new AttributeModifier(
				Db.Get().Amounts.HitPoints.maxAttribute.Id,
				1,
				trait.Name,
				true));

			trait.OnAddTrait += go =>
			{
				go.AddOrGet<AngryTrait>();
			};
		}
	}
}
