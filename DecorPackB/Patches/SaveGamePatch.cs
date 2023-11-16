using DecorPackB.Content.Scripts;
using HarmonyLib;
namespace DecorPackB.Patches
{
	public class SaveGamePatch
	{
		[HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				__instance.gameObject.AddOrGet<DecorPackB_Mod>();
			}
		}
	}
}
