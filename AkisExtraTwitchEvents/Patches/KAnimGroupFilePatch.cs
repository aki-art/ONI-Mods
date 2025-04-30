using HarmonyLib;

namespace Twitchery.Patches
{
	public class KAnimGroupFilePatch
	{
		[HarmonyPatch(typeof(KAnimGroupFile), "Load")]
		public class KAnimGroupFile_Load_Patch
		{
			public static void Prefix(KAnimGroupFile __instance)
			{
				FUtility.Utils.RegisterBatchTag(
					__instance,
					CONSTS.BATCH_TAGS.INTERACTS,
					[
						"aete_interacts_espresso_short_kanim",
						"aete_goop_vomit_kanim"
					]);

				FUtility.Utils.RegisterBatchTag(
					__instance,
					CONSTS.BATCH_TAGS.SWAPS,
					[
						"aete_bleachedhair_kanim"
					]);
			}
		}
	}
}