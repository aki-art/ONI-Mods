using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class CropPatch
	{
		[HarmonyPatch(typeof(Crop), "OnSpawn")]
		public class Crop_OnSpawn_Patch
		{
			public static void Postfix(Crop __instance)
			{
				if (AkisTwitchEvents.Instance.IsHarvestMoonActive())
					AkisTwitchEvents.Instance.AddHarvestMoonBoon(__instance);
			}
		}
	}
}
