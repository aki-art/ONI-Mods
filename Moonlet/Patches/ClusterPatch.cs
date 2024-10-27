using HarmonyLib;
using Moonlet.Scripts;
using ProcGenGame;

namespace Moonlet.Patches
{
	public class ClusterPatch
	{
		[HarmonyPatch(typeof(Cluster), "Generate")]
		public class Cluster_Generate_Patch
		{
			public static void Prefix()
			{
				Log.Debug("Cluster Generate prefix");
				//Moonlet_ZoneTypeTracker.worldgenZoneTypeOverrides?.Clear();
				Moonlet_Mod.worldgenZoneTypeOverrides?.Clear();
			}
		}
	}
}
