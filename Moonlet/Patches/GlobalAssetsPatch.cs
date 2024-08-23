using HarmonyLib;

namespace Moonlet.Patches
{
	public class GlobalAssetsPatch
	{
		[HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
		public class GlobalAssets_OnPrefabInit_Patch
		{
		}
	}
}
