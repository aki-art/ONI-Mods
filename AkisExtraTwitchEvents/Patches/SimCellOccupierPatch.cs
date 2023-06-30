using HarmonyLib;

namespace Twitchery.Patches
{
	public class SimCellOccupierPatch
	{
		[HarmonyPatch(typeof(SimCellOccupier), "OnSpawn")]
		public class SimCellOccupier_OnSpawn_Patch
		{
			public static void Prefix(SimCellOccupier __instance)
			{
				// crashes if i change the material before things finalize, so just ignore these
				foreach (var midas in Mod.touchers.Items)
					midas.IgnoreCell(__instance.NaturalBuildingCell());
			}
		}
	}
}
