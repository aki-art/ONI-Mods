namespace Twitchery.Content
{
    public class TTags
    {
        public static Tag
            polymorphSpecies = TagManager.Create("AkisExtraTwitchEvents_PolymorphSpecies"),
            // buildings or entities with this tag will ignore midas touch event
            midasSafe = TagManager.Create("AkisExtraTwitchEvents_MidasSafe"),
			// under effects of midas
			goldFlaked = TagManager.Create("AkisExtraTwitchEvents_GoldFlaked"),
			// under effects of midas
			midased = TagManager.Create("AkisExtraTwitchEvents_Midased"),
			// under effects of midas
			angry = TagManager.Create("AkisExtraTwitchEvents_Angry"),
			// under effects of midas
			hungry = TagManager.Create("AkisExtraTwitchEvents_Hungry"),
			// under effects of midas
			eating = TagManager.Create("AkisExtraTwitchEvents_Eating"),
			// under effects of midas
			returningHome = TagManager.Create("AkisExtraTwitchEvents_ReturningHome"),
			// for wormy bois
			longBoi = TagManager.Create("AkisExtraTwitchEvents_LongBoi");
    }
}
