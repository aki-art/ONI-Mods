using HarmonyLib;
using UnityEngine;

namespace GoldenThrone.Patches
{
	internal class MinionConfigPatch
	{
		[HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
		public class MinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				if (Mod.Settings.CustomCrown)
				{
					__result.GetComponent<SnapOn>().snapPoints.Add(
						new SnapOn.SnapPoint
						{
							pointName = "GoldenThrone_Crown",
							automatic = false,
							context = "",
							buildFile = Assets.GetAnim("gt_crown_kanim"),
							overrideSymbol = "snapTo_hat"
						});
				}
			}
		}
	}
}
