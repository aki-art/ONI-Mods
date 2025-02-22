using FUtility;
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
				Log.Debug($"rational array: {__instance.RATIONAL_AI_STATE_MACHINES.Length}");
				if (__instance.RATIONAL_AI_STATE_MACHINES.Contains(AddSolarDamageMonitor))
					return;

				__instance.RATIONAL_AI_STATE_MACHINES = [.. __instance.RATIONAL_AI_STATE_MACHINES.AddItem(
					AddSolarDamageMonitor)];
			}

			private static StateMachine.Instance AddSolarDamageMonitor(RationalAi.Instance smi)
			{
				return new AETE_SolarStormBionicReactionMonitor.Instance(smi.master, new AETE_SolarStormBionicReactionMonitor.Def());
			}
		}
	}
}
