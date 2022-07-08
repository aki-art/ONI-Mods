namespace CrittersDropBones.Patches
{
    public class KAnimGroupFilePatch
    {
        private const string COOKER_ANIM = "cooker_interact_anim_kanim";

        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(-1371425853));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == COOKER_ANIM);

                // readd to correct group
                var anim = Assets.GetAnim(COOKER_ANIM);

                dupeAnimsGroup.animFiles.Add(anim);
                dupeAnimsGroup.animNames.Add(anim.name);
            }
        }
    }
}
