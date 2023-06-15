using HarmonyLib;
using UnityEngine;
using static KAnim;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
	public class HeatCubePatch
	{
		[HarmonyPatch(typeof(HeatCubeConfig), "CreatePrefab")]
		public class HeatCubeConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				var primaryElement = __result.AddOrGet<PrimaryElement>();
				primaryElement.Temperature = 773.15f; // 500C

				var kbac = __result.AddOrGet<KBatchedAnimController>();
				kbac.AnimFiles = new[]
				{
					 Assets.GetAnim("rrp_heatcube_replacer_kanim")
				};

				kbac.initialAnim = "idle_tallstone";

				var stt = __result.GetComponent<SimTemperatureTransfer>();
				stt.SurfaceArea = 10f;
				stt.Thickness = 0.001f;

				__result.AddOrGet<InfoDescription>().description += "\n\n" +
					"Spawns at 500C, and rapidly exchanges heat with it's environment.";

				__result.AddTag(GameTags.PedestalDisplayable);
			}
		}
	}
}