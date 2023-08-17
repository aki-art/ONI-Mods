using HarmonyLib;

namespace Twitchery.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				FLocalization.Translations.RegisterForTranslation(typeof(STRINGS), true);

				// duplicate entry, but the game expects both keys to exist
				Strings.Add("STRINGS.ITEMS.FOOD.JELLO.NAME", STRINGS.ELEMENTS.JELLO.NAME);
				Strings.Add("STRINGS.ITEMS.FOOD.JELLO.DESC", STRINGS.ELEMENTS.JELLO.DESC);

				Strings.Add("STRINGS.ITEMS.FOOD.FROZENHONEY.NAME", STRINGS.ELEMENTS.FROZENHONEY.NAME);
				Strings.Add("STRINGS.ITEMS.FOOD.FROZENHONEY.DESC", STRINGS.ELEMENTS.FROZENHONEY.DESC);

			}
		}
	}
}
