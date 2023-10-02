using FUtility;
using HarmonyLib;
using KMod;
using UnityEngine;

namespace ReviveADupe
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			Log.PrintVersion(this);
		}

		[HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
		public class MinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				__result.AddComponent<ReviveADupe_ReviveableCorpse>();
			}
		}
	}
}
