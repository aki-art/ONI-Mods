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
				MoonletMods.Instance.moonletMods.Do(mod => LoadSprite(__instance, mod.Value));
			}

			private static void LoadSprite(Assets asset, MoonletMod mod)
			{
				Mod.spritesLoader.LoadSprites(asset, mod);
			}
		}
	}
}
