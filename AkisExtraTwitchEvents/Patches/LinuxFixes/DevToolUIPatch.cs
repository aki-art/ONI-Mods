using HarmonyLib;

namespace Twitchery.Patches.LinuxFixes
{
	public class DevToolUIPatch
	{
		[HarmonyPatch(typeof(DevToolUI), nameof(DevToolUI.PingHoveredObject))]
		private static class DevToolNoPing
		{
			public static bool Prefix()
			{
				return false;
			}
		}
	}
}
