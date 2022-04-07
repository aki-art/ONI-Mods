using HarmonyLib;

namespace CrittersDropBones.Patches
{
    internal class AssetsPatch
    {
        [HarmonyPatch(typeof(Assets), "LoadAnims")]
        public class Assets_LoadAnims_Patch
        {
            public static void Postfix()
            {
                var anim = Assets.GetAnim("anim_interacts_cookingpot_kanim");
                var reference = Assets.GetAnim("body_comp_default_kanim");

                var file = KGlobalAnimParser.Get().GetFile(anim);
                //file.batchTag = KGlobalAnimParser.Get().GetFile(reference).batchTag;

                Traverse.Create(anim).Field("batchTag").SetValue(reference.batchTag);
                //anim.batchTag = reference.batchTag;
            }
        }
    }
}
