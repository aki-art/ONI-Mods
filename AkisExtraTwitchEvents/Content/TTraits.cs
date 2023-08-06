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

			trait.OnAddTrait += go =>
			{
				go.AddOrGet<AngryTrait>();
			};
		}
	}
}
