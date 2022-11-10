using FUtility;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class KAnimGroupFileAnimPatch
    {
        private const string BOOK_READING_ANIM = "rpp_interacts_read_book_kanim";

        // this is the important patch
        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(Consts.BATCH_TAGS.INTERACTS));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == BOOK_READING_ANIM);

                // readd to correct group
                var readingBookAnim = Assets.GetAnim(BOOK_READING_ANIM);

                dupeAnimsGroup.animFiles.Add(readingBookAnim);
                dupeAnimsGroup.animNames.Add(readingBookAnim.name);
            }

        }
    }
}
