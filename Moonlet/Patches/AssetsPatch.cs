using HarmonyLib;

namespace Moonlet.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), nameof(Assets.OnPrefabInit))]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				foreach (var mod in Mod.modLoaders)
					mod.spriteLoader.LoadSprites(__instance);
			}
		}
	}
}
