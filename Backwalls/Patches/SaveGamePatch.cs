using Backwalls.Cmps;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class SaveGamePatch
	{
		[HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				var mod = __instance.gameObject.AddOrGet<Backwalls_Mod>();
				mod.ShowHSV = true;
				mod.ShowSwatches = true;
				mod.CopyColor = true;
				mod.CopyPattern = true;

				var tool = __instance.gameObject.AddOrGet<Backwalls_SmartBuildTool>();
				/*
								if (KInputManager.currentController != null)
									KInputHandler.Add(KInputManager.currentController, tool, -2);
								else
									KInputHandler.Add(Global.GetInputManager().GetDefaultController(), tool, -2);*/
			}
		}
	}
}
