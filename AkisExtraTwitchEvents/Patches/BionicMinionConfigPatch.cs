using HarmonyLib;
using System.Linq;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class BionicMinionConfigPatch
	{
		[HarmonyPatch(typeof(BionicMinionConfig), "OnSpawn")]
		public class BionicMinionConfig_OnSpawn_Patch
		{
			public static void Prefix(BionicMinionConfig __instance)
			{
				if (__instance.RATIONAL_AI_STATE_MACHINES.Contains(AddSolarDamageMonitor))
					return;

				var sms = __instance.RATIONAL_AI_STATE_MACHINES.ToList();
				sms.Add(AddSolarDamageMonitor);

				__instance.RATIONAL_AI_STATE_MACHINES = [.. sms];
			}

			private static StateMachine.Instance AddSolarDamageMonitor(RationalAi.Instance smi)
			{
				return new AETE_SolarStormBionicReactionMonitor.Instance(smi.master, new AETE_SolarStormBionicReactionMonitor.Def());
			}
		}
	}
}
