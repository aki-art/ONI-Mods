using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace PrintingPodRecharge.Patches
{
	public class KAnimGroupFilePatch
	{
		private const string ESPRESSO_ANIM = "aete_interacts_espresso_short_kanim";
		private const string HULK_HEAD = "aete_hulk_head_kanim";
		private const string HULK_BODY = "aete_hulk_body_kanim";

		[HarmonyPatch(typeof(KAnimGroupFile), "Load")]
		public class KAnimGroupFile_Load_Patch
		{
			public static void Prefix(KAnimGroupFile __instance)
			{
				var groups = __instance.GetData();
				SwapGroup(groups, Consts.BATCH_TAGS.INTERACTS, ESPRESSO_ANIM);
				SwapGroup(groups, Consts.BATCH_TAGS.SWAPS, HULK_HEAD);
				SwapGroup(groups, Consts.BATCH_TAGS.SWAPS, HULK_BODY);
			}

			private static void SwapGroup(List<KAnimGroupFile.Group> groups, int batch, string animFile)
			{
				var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(batch));

				// remove the wrong group
				groups.RemoveAll(g => g.animNames[0] == animFile);

				// readd to correct group
				var anim = Assets.GetAnim(animFile);

				dupeAnimsGroup.animFiles.Add(anim);
				dupeAnimsGroup.animNames.Add(anim.name);
			}
		}
	}
}
