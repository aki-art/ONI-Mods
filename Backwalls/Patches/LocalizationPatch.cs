using FUtility.FLocalization;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);

				Strings.Add("STRINGS.BUILDINGS.PREFABS.BACKWALL_SEALEDBACKWALL.EFFECT", global::STRINGS.BUILDINGS.PREFABS.EXTERIORWALL.EFFECT);
			}
		}
	}
}
