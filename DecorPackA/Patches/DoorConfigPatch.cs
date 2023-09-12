using DecorPackA.Buildings;
using HarmonyLib;
using UnityEngine;

namespace DecorPackA.Patches
{
	public class DoorConfigPatch
	{
		[HarmonyPatch(typeof(DoorConfig), "DoPostConfigureComplete")]
		public class DoorConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(GameObject go) => FixLayers(go);
		}

		[HarmonyPatch(typeof(DoorConfig), "DoPostConfigureUnderConstruction")]
		public class DoorConfig_DoPostConfigureUnderConstruction_Patch
		{
			public static void Postfix(GameObject go) => FixLayers(go);
		}

		private static void FixLayers(GameObject go)
		{
			var facade = go.AddOrGet<FacadeRestorer>();
			facade.defaultAnim = null;// "closed";

			go.GetComponent<KPrefabID>().prefabSpawnFn += FGFixer.FixLayers;
		}
	}
}
