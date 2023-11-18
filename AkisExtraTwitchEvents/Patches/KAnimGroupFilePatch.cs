using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace PrintingPodRecharge.Patches
{
	public class KAnimGroupFilePatch
	{
		[HarmonyPatch(typeof(KAnimGroupFile), "Load")]
		public class KAnimGroupFile_Load_Patch
		{
			public static void Prefix(KAnimGroupFile __instance)
			{
				Utils.RegisterBatchTag(
					__instance,
					CONSTS.BATCH_TAGS.INTERACTS,
					new HashSet<HashedString>()
					{
						"aete_interacts_espresso_short_kanim",
						"aete_goop_vomit_kanim"
					});
			}
		}
	}
}