namespace Twitchery.Content
{
	public class StoryCards : ResourceSet<StoryCard>
	{
		public const string
			ABDUCTORS_GLOBS = "Abductors_Globs",
			ABDUCTORS_SCARY = "Abductors_Scary",
			ABDUCTORS_MERMALADE = "Abductors_Mermalade",

			TEA_TIME = "Journey_Tea";

		public static class TAGS
		{
			public const string
				AGGRESSIVE = "Aggressive",
				FRIENDLY = "Friendly",
				ADVANCED = "Advanced";
		}

		public StoryCards()
		{
			Add(new StoryCard(ABDUCTORS_GLOBS, "aete_storycard_abductors_globs_kanim")
				.MustBeNthProgression(0)
				.GrantTrait("Test"));

			Add(new StoryCard(ABDUCTORS_SCARY, "aete_storycard_abductors_scary_kanim")
				.MustBeNthProgression(0)
				.GrantTrait("Test"));

			Add(new StoryCard(ABDUCTORS_MERMALADE, "aete_storycard_abductors_mermalade_kanim")
				.MustBeNthProgression(0)
				.GrantTrait("Test"));

			Add(new StoryCard(TEA_TIME, "aete_storycard_journey_tea_kanim")
				.RequiresTag(TAGS.FRIENDLY)
				.GrantTrait("Test"));
		}
	}
}
