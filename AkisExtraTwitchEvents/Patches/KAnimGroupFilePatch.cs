using FUtility;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class KAnimGroupFilePatch
    {
        private const string ESPRESSO_ANIM = "aete_interacts_espresso_short_kanim";

        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(Consts.BATCH_TAGS.INTERACTS));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == ESPRESSO_ANIM);

                // readd to correct group
                var readingBookAnim = Assets.GetAnim(ESPRESSO_ANIM);

                dupeAnimsGroup.animFiles.Add(readingBookAnim);
                dupeAnimsGroup.animNames.Add(readingBookAnim.name);
            }
        }
    }
}