using FMODUnity;
using HarmonyLib;

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


		[HarmonyPatch(typeof(KFMOD), "Initialize")]
		public class KFMOD_Initialize_Patch
		{
			public static void Postfix()
			{
				Log.Debug("Initializing KFMOD");
			}
		}

		[HarmonyPatch(typeof(StudioBankLoader), "Load")]
		public class StudioBankLoader_Load_Patch
		{
			public static void Postfix()
			{
				Log.Debug("STUDIO LOAD");
			}
		}

		[HarmonyPatch(typeof(Game), "OnPrefabInit")]
		public class Game_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
			}

		}
	}
}
