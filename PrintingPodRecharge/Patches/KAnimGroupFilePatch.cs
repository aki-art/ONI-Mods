using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace PrintingPodRecharge.Patches
{
    public class KAnimGroupFilePatch
    {
        private const string BOOK_READING_ANIM = "rpp_interacts_read_book_kanim";
        private const string HAIR = "rrp_bleachedhair_kanim";

        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                MoveAnimGroup(groups, CONSTS.BATCH_TAGS.SWAPS, HAIR);
                MoveAnimGroup(groups, CONSTS.BATCH_TAGS.INTERACTS, BOOK_READING_ANIM);
            }

            private static void MoveAnimGroup(List<KAnimGroupFile.Group> groups, int batchTagHash, string animName)
            {
                if (groups == null)
                {
                    return;
                }

                var anim = Assets.GetAnim(animName);
                if (anim == null)
                {
                    return;
                }

                var animsGroup = KAnimGroupFile.GetGroup(new HashedString(batchTagHash));
                if (animsGroup == null)
                {
                    return;
                }

                groups.RemoveAll(g => g.animNames != null && g.animNames.Count > 0 && g.animNames[0] == animName);

                animsGroup.animFiles.Add(anim);
                animsGroup.animNames.Add(anim.name);
            }
        }
    }
}
