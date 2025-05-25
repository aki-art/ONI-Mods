using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class SlipperyMonitorPatch
	{
		[HarmonyPatch(typeof(SlipperyMonitor), "IsStandingOnASlipperyCell")]
		public class SlipperyMonitor_IsStandingOnASlipperyCell_Patch
		{
			public static void Postfix(SlipperyMonitor.Instance smi, ref bool __result)
			{
				if (!__result)
					__result |= smi.HasTag(TTags.oiledUp);
			}
		}
	}
}
