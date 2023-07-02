using FUtility;
using HarmonyLib;
using Twitchery.Content;

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

                var lumber = Util.StripTextFormatting(global::STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME.ToString());
                lumber = FUtility.Utils.FormatAsLink(lumber, Elements.FakeLumber.ToString());
				Strings.Add("STRINGS.ELEMENTS.FAKELUMBER.NAME", lumber);
			}
        }
    }
}
