using FUtility;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class KAnimGroupFilePatch
    {
        private const string HAIR = "rrp_bleachedhair_kanim";

        // this is the important patch
        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                var swapAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(Consts.BATCH_TAGS.SWAPS));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == HAIR);

                // readd to correct group
                var hairSwap = Assets.GetAnim(HAIR);

                swapAnimsGroup.animFiles.Add(hairSwap);
                swapAnimsGroup.animNames.Add(hairSwap.name);
            }
        }
    }
}
