using HarmonyLib;
using ProcGen;
using UnityEngine;

namespace Moonlet.Patches
{
	public class TEMP
	{
		[HarmonyPatch(typeof(SimMessages), "ModifyCell")]
		public class SimMessages_ModifyCell_Patch
		{
			public static void Prefix(int gameCell, ushort elementIdx)
			{
				if (ElementLoader.elements.Count < elementIdx || elementIdx < 0)
				{
					Log.Warn("Element tried to load idx " + elementIdx);
				}
			}
		}

		[HarmonyPatch(typeof(WorldTrait), "IsValid")]
		public class WorldTrait_isvalid_Patch
		{
			public static void Prefix(WorldTrait __instance, ProcGen.World world)
			{
				// TODO
				int num2 = 0;
				foreach (var item in __instance.globalFeatureMods)
				{
					Log.Debug($"{item.Key}, {item.Value}");
					num2 += Mathf.FloorToInt(world.worldTraitScale * (float)item.Value);
				}

			}
		}
	}
}
