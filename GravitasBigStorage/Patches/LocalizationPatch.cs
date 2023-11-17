using FUtility.FLocalization;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);
				Strings.Add(
					"STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.EFFECT",
					global::STRINGS.BUILDINGS.PREFABS.LONELYMINIONHOUSE_COMPLETE.EFFECT);
			}
		}
	}
}
