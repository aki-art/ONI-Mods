using FUtility.FLocalization;
using HarmonyLib;

namespace SpookyPumpkinSO.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);

				Strings.Add("STRINGS.EQUIPMENT.SP_HALLOWEENCOSTUME.NAME", STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.GENERICNAME);
				Strings.Add("STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.DESC", global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.DESC);
				Strings.Add("STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.RECIPE_DESC", global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC);
				Strings.Add("STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.EFFECT", global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.EFFECT);
			}
		}
	}
}
