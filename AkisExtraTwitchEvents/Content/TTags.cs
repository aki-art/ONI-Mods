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
			// for wormy bois
			longBoi = TagManager.Create("AkisExtraTwitchEvents_LongBoi");
    }
}
