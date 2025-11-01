using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class MinionConfigPatch
	{
		[HarmonyPatch(typeof(BaseMinionConfig), "BaseMinion")]
		public class BaseMinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<AETE_MinionStorage>();
			}
		}
	}
}
