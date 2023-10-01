using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
	public class MinionConfigPatch
	{
		[HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
		public class MinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddComponent<FacePaint>();
			}
		}
	}
}
