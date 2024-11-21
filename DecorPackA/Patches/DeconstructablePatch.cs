using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Patches
{
	public class DeconstructablePatch
	{
		// sets the dye temperature to default
		[HarmonyPatch(typeof(Deconstructable), "SpawnItemsFromConstruction", typeof(float), typeof(byte), typeof(int), typeof(WorkerBase))]
		public class Deconstructable_SpawnItemsFromConstruction_Patch
		{
			public static void Postfix(Deconstructable __instance, List<GameObject> __result)
			{
				if (__instance.HasTag(ModAssets.Tags.stainedGlass) && __result != null && __result.Count >= 2)
				{
					var pe = __result[1].GetComponent<PrimaryElement>();
					pe.Temperature = pe.Element.defaultValues.temperature;
				}
			}
		}
	}
}
