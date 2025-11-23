using DecorPackA.Buildings;
using DecorPackA.Scripts;
using HarmonyLib;
using UnityEngine;

namespace DecorPackA.Patches
{
	public class DiningTableConfigPatch
	{
		[HarmonyPatch(typeof(DiningTableConfig), "ConfigureBuildingTemplate")]
		public class DiningTableConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				FixLayers(go);
				go.AddComponent<DecorPackA_SaltTracker>();
			}
		}

		private static void FixLayers(GameObject go)
		{
			var facade = go.AddOrGet<FacadeRestorer>();
			facade.defaultAnim = "off";

			if (go.TryGetComponent(out KPrefabID kPrefabId))
			{
				kPrefabId.prefabSpawnFn += FGFixer.FixLayers;
			}
		}
	}
}
