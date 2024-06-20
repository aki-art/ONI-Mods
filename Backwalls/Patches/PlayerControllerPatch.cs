using Backwalls.Cmps;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class PlayerControllerPatch
	{
		[HarmonyPatch(typeof(PlayerController), "OnKeyDown")]
		public class PlayerController_OnKeyDown_Patch
		{
			public static void Prefix(KButtonEvent e)
			{
				if (e.TryConsume(BWActions.SmartBuildAction.GetKAction()))
					Backwalls_SmartBuildTool.Instance.Toggle();
			}
		}
	}
}
