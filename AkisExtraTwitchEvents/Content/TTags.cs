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
			// vole behavior tag to return home
			returningHome = TagManager.Create("AkisExtraTwitchEvents_ReturningHome"),
			// regular pip behavior tag for summining
			summoning = TagManager.Create("AkisExtraTwitchEvents_Summoning"),
			// for wormy bois
			longBoi = TagManager.Create("AkisExtraTwitchEvents_LongBoi"),
			disableUserScreen = TagManager.Create("AkisExtraTwitchEventsDisableUserScreen"),
			disableChaosToucherTarget = TagManager.Create("AkisExtraTwitchEvents_DisableChaosToucherTarget"),
			hideDeadDupesWithin = TagManager.Create("AkisExtraTwitchEvents_HideDeadDupesWithin");
	}
}
