using HarmonyLib;
using System;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class BaseMinionConfigPatch
	{
		[HarmonyPatch(typeof(BaseMinionConfig), "BaseRationalAiStateMachines")]
		public class BaseMinionConfig_BaseRationalAiStateMachines_Patch
		{
			public static void Postfix(ref Func<RationalAi.Instance, StateMachine.Instance>[] __result)
			{
				__result = [.. __result.AddItem(AddJailedMonitor)];
			}

			private static StateMachine.Instance AddJailedMonitor(RationalAi.Instance smi)
			{
				return new AETE_JailMonitor.Instance(smi.master, new AETE_JailMonitor.Def());
			}
		}
	}
}
