using HarmonyLib;

namespace DecorPackA.Patches
{
	internal class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public static class Assets_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				BuildingFacadesPatch.Patch(Mod.harmonyInstance);
			}
		}
	}
}
