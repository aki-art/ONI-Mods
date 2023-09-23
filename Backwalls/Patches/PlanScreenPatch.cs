using Backwalls.Cmps;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class PlanScreenPatch
	{
		[HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
		public static class PlanScreen_OnClickCopyBuilding_Patch
		{
			public static void Prefix() => Backwalls_Mod.Instance.OnCopyBuilding();
		}
	}
}
