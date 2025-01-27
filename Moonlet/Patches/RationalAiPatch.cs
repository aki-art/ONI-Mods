using HarmonyLib;
using Moonlet.Scripts;
using System.Linq;

namespace Moonlet.Patches
{
	public class RationalAiPatch
	{
		[HarmonyPatch(typeof(RationalAi), "InitializeStates")]
		public class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				if (Moonlet_Mod.stepOnEffects.Any(e =>
				{
					var element = ElementLoader.FindElementByHash(e.Key);
					return element != null && element.IsGas;
				}))
				{
					__instance.alive
						.ToggleStateMachine(smi => new Moonlet_ExposureMonitor.Instance(smi.master));
				}
			}
		}
	}
}
