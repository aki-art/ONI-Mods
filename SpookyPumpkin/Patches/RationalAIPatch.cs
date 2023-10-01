using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;

namespace SpookyPumpkinSO.Patches
{
	public class RationalAIPatch
	{
		[HarmonyPatch(typeof(RationalAi), "InitializeStates")]
		public class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive
					.ToggleStateMachine(smi => new Ghastly.Instance(smi.master));
			}
		}
	}
}
