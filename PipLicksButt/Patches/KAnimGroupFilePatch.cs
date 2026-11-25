using HarmonyLib;

namespace PipLicksButt.Patches
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
                    -223989833, // pips
                    [
                        "plb_pip_licks_butt_kanim"
                    ]);

            }
        }
    }
}
