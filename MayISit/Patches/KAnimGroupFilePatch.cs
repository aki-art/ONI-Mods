using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayISit.Patches
{
    public class KAnimGroupFilePatch
    {
        private const string TEST = "anim_interacts_sit_kanim";

        // this is the important patch
        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                // no clue what the human string for that hash is. it was set in unity so the string value is never making it to the hash cache
                // but its the one that has all the interactions and reactions
                var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(-1371425853));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == TEST);

                // readd to correct group
                var testAnim = Assets.GetAnim(TEST);

                dupeAnimsGroup.animFiles.Add(testAnim);
                dupeAnimsGroup.animNames.Add(testAnim.name);
            }
        }
    }
}
