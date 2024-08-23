using HarmonyLib;
using Moonlet.Utils;

namespace Moonlet.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				MoonletMods.Instance.moonletMods.Do(mod =>
				{
					Mod.spritesLoader.LoadSprites(__instance, mod.Value);
				});
			}
		}


		[HarmonyPatch(typeof(Game), "OnPrefabInit")]
		public class Game_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				MoonletMods.Instance.moonletMods.Do(mod =>
				{
					Mod.FMODBanksLoader.LoadContent(mod.Value, FileUtil.delimiter);
				});

				App.OnPreLoadScene += ClearFMOD;
			}

			private static void ClearFMOD()
			{
				MoonletMods.Instance.moonletMods.Do(mod =>
				{
					Mod.FMODBanksLoader.UnLoadContent();
				});

			}
		}
	}
}
