using FUtility.FLocalization;
using HarmonyLib;

namespace Asphalt.Patches
{
	class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);
			}
		}
	}
}
