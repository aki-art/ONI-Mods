using HarmonyLib;

namespace Twitchery.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				FUtility.Assets.LoadSprites(__instance);
			}

			public static void Postfix()
			{
				ModAssets.OnGameAssetsLoaded();
			}
		}
	}
}
