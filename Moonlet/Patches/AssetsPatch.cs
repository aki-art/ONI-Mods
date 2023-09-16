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
				MoonletMods.Instance.moonletMods.Do(mod => Mod.spritesLoader.LoadSprites(__instance, mod));
			}
		}
	}
}
