using DecorPackB.Content.Scripts;
using FUtility.FLocalization;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);

				foreach (var tile in FloorLampFrames.tileInfos)
				{
					var key = $"STRINGS.BUILDINGS.PREFABS.{Utils.GetLinkAppropiateFormat(tile.ID)}";
					Strings.Add(key + ".NAME", STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.NAME);
					Strings.Add(key + ".DESC", STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.DESC);
					Strings.Add(key + ".EFFECT", STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.EFFECT);
				}
			}
		}
	}
}
