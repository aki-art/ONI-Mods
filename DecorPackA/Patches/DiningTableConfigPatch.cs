using DecorPackA.Buildings;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace DecorPackA.Patches
{
	internal class DiningTableConfigPatch
	{
		[HarmonyPatch(typeof(DiningTableConfig), "ConfigureBuildingTemplate")]
		public class DiningTableConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go) => FixLayers(go);
		}

		private static void FixLayers(GameObject go)
		{
			var facade = go.AddOrGet<FacadeRestorer>();
			facade.defaultAnim = "off";

			if (go.TryGetComponent(out KPrefabID kPrefabId))
			{
				kPrefabId.prefabSpawnFn += FGFixer.FixLayers;
				kPrefabId.prefabSpawnFn += go => go.GetComponent<MessStation>().StartCoroutine(ForceAnimation(go));
			}
		}

		private static IEnumerator ForceAnimation(GameObject go)
		{
			yield return SequenceUtil.waitForEndOfFrame;

			if (go.TryGetComponent(out MessStation messStation) && go.TryGetComponent(out KBatchedAnimController kbac))
			{
				if (messStation.smi.IsInsideState(messStation.smi.sm.salt))
					kbac.Play("salt");
				else
					kbac.Play("off");
			}
		}
	}
}
