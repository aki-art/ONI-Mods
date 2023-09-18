using DecorPackA.Buildings.StainedGlassTile;
using DecorPackA.Scripts;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class SaveGamePatch
	{
		[HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				var modStorage = __instance.gameObject.AddOrGet<Scripts.DecorPackA_Mod>();
				modStorage.showHSV = true;
				modStorage.showSwatches = true;

				if (!Mod.Settings.GlassTile.DisableColorShiftEffect)
				{
					__instance.gameObject.AddOrGet<DecorPackAGlassShineColors>();
				}
			}
		}
	}
}
