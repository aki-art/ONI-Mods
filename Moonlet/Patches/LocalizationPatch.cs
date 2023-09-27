using FUtility.FLocalization;
using HarmonyLib;

namespace Moonlet.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Mod.translationLoader.RegisterAll();
				Translations.RegisterForTranslation(typeof(STRINGS));
				Strings.Add("STRINGS.UI.DEVCONSOLE_TMPCONVERTED.HEADER.LABEL", "Console");
				Strings.Add("STRINGS.UI.DEVCONSOLE_TMPCONVERTED.COMMANDINPUT.TEXTAREA.PLACEHOLDER", "Type `help` to see a list of commands");
				Strings.Add("STRINGS.UI.DEVCONSOLE_TMPCONVERTED.OPENLOGBUTTON.LABEL", "Open log.txt");
				Strings.Add("STRINGS.UI.DEVCONSOLE_TMPCONVERTED.COPYBUTTON.LABEL", "Copy");
			}
		}
	}
}
