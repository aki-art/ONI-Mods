using HarmonyLib;
using TwitchEventsTimer;

namespace Patches
{
	public class SaveGamePatch
	{
        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
			public static void Postfix(SaveGame __instance) => __instance.gameObject.AddOrGet<TwitchEventsTimer_Mod>();
		}
	}
}
