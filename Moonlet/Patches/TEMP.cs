using HarmonyLib;
using ProcGen;
using UnityEngine;

namespace Moonlet.Patches
{
	public class TEMP
	{
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
