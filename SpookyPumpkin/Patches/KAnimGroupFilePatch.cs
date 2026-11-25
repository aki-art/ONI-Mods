using HarmonyLib;

namespace SpookyPumpkinSO.Patches
{
    public class KAnimGroupFilePatch
    {
        [HarmonyPatch(typeof(KGlobalAnimParser), "GetParseCommands")]
        public class KGlobalAnimParser_GetParseCommands_Patch
        {
            public static void Postfix(AnimCommandFile __result, string path)
            {
                Log.Debug($"anim command file {path} {__result?.TargetBuild}");
            }
        }

        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                Log.Debug($"{(HashedString)"sp_ghostpip_kanim"}");
                Log.Debug($"{(HashedString)"sp_ghostpip"}");
                FUtility.Utils.RegisterBatchTag(
                    __instance,
                    938823943, // ghostpip
                    [
                        "sp_pip_licks_butt_kanim",

                    ]);

                FUtility.Utils.RegisterBatchTag(
                    __instance,
                    FUtility.CONSTS.BATCH_TAGS.SWAPS,
                    [
                        "sp_skellingtoncostume_kanim",
                    ]);
            }
        }

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class AssetsPatch
        {
            public static void Postfix()
            {

                foreach (var group in KAnimGroupFile.groupfile.groups)
                {
                    Log.Debug($"group {group.id.hash} - {HashCache.Get().Get(group.id)}");
                    for (int i = 0; i < group.animNames.Count; i++)
                    {
                        HashedString anim = group.animNames[i];
                        if (Assets.TryGetAnim(anim, out var file))
                            Log.Debug($"\t {anim.hash} - {file.name}");
                    }
                }
                Log.Debug("BATCHTAG>" + Assets.GetAnim("sp_ghostpip_kanim").batchTag.hash);
            }
        }

    }
}
