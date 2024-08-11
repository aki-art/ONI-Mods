using GoldenThrone.Cmps;
using HarmonyLib;

namespace GoldenThrone.Patches
{
	public class RationaAIPatch
	{
		// Extend duplicant AI
		[HarmonyPatch(typeof(RationalAi), "InitializeStates")]
		public class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive
					.ToggleStateMachine(smi => new RoyalRelief.Instance(smi.master));
			}
		}
	}
}
