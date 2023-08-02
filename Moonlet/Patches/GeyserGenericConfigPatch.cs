using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Patches
{
	public class GeyserGenericConfigPatch
	{
		[HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.GenerateConfigs))]
		public class GeyserGenericConfig_GenerateConfigs_Patch
		{
			public static void Postfix(List<GeyserGenericConfig.GeyserPrefabParams> __result)
			{
				foreach (var mod in Mod.modLoaders)
					mod.geysersLoader.RegisterConfigs(ref __result);
			}
		}

		[HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.CreateGeyser))]
		public class GeyserGenericConfig_CreateGeyser_Patch
		{
			public static void Postfix(GameObject __result)
			{
				foreach (var mod in Mod.modLoaders)
					mod.geysersLoader.PostProcessGeyser(__result);
			}
		}
	}
}
