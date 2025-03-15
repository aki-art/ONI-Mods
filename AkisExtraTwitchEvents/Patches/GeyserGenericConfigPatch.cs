using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content.Defs;

namespace Twitchery.Patches
{
	public class GeyserGenericConfigPatch
	{
		[HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.GenerateConfigs))]
		public class GeyserGenericConfig_GenerateConfigs_Patch
		{
			public static void Postfix(List<GeyserGenericConfig.GeyserPrefabParams> __result)
			{
				TGeyserConfigs.GenerateConfigs(__result);
			}
		}
	}
}
