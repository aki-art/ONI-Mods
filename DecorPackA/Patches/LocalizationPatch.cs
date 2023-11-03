using DecorPackA.Buildings.StainedGlassTile;
using FUtility.FLocalization;
using HarmonyLib;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE;

namespace DecorPackA.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);

				// Add stained glass variants
				foreach (var tile in StainedGlassTiles.tileInfos)
				{
					var key = $"STRINGS.BUILDINGS.PREFABS.{Utils.GetLinkAppropiateFormat(tile.ID)}";
					Strings.Add(key + ".NAME", NAME);
					Strings.Add(key + ".DESC", DESC);
					Strings.Add(key + ".EFFECT", EFFECT);
				}

				Strings.Add("STRINGS.UI.KLEI_INVENTORY_SCREEN.SUBCATEGORIES.BUILDING_DOOR", global::STRINGS.UI.NEWBUILDCATEGORIES.DOORS.NAME);
			}
		}
	}
}
