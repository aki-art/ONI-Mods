using FUtility;
using HarmonyLib;
using KMod;

namespace KAnimTests
{
    public class CustomInteractsAnim 
    {
        private const string FISTBUMP_ORIGINAL = "anim_fistbump_kanim";
        private const string FISTBUMP_CLONE = "cloned_anim_fistbump_kanim";
        private const string FISTBUMP_NEW = "fistbump_copy_kanim";

        private const string TEST = "cooker_interact_anim_kanim";

        // this is the important patch
        [HarmonyPatch(typeof(KAnimGroupFile), "Load")]
        public class KAnimGroupFile_Load_Patch
        {
            public static void Prefix(KAnimGroupFile __instance)
            {
                var groups = __instance.GetData();
                var dupeAnimsGroup = KAnimGroupFile.GetGroup(new HashedString(-1371425853));

                // remove the wrong group
                groups.RemoveAll(g => g.animNames[0] == TEST); 

                // readd to correct group
                var testAnim = Assets.GetAnim(TEST);

                dupeAnimsGroup.animFiles.Add(testAnim);
                dupeAnimsGroup.animNames.Add(testAnim.name);
            }

            // just printing stuff here
            public static void Postfix(KAnimGroupFile __instance)
            {
                Log.Debuglog("LOADED KANIM GROUPS RESOURCE FILE");
                foreach (var group in __instance.GetData())
                {
                    Log.Debuglog($"{group.id} - {group.id.HashValue} - {HashCache.Get().Get(group.id.HashValue)}");
                    Log.Debuglog("ANIM NAMES");
                    foreach (var animName in group.animNames)
                    {
                        Log.Debuglog($"\t\t{animName} - {HashCache.Get().Get(animName.HashValue)}");
                    }
                    Log.Debuglog("\tANIM FILES");
                    foreach (var animFile in group.animFiles)
                    {
                        Log.Debuglog($"\t\t{animFile.name}");
                    }
                }
            }
        }

        // TESTING
        // this plays the animation on all dupes when pressing Debug Cheer Anim (default: ctrl + C)
        [HarmonyPatch(typeof(DebugHandler), "OnKeyDown")]
        public class DebugHandler_OnKeyDown_Patch
        {
            // in a real mod this method should not be skipped, but this is just for testing
            public static bool Prefix(KButtonEvent e)
            {
                if (e.TryConsume(Action.DebugCheerEmote))
                {
                    for (int i = 0; i < Components.MinionIdentities.Count; i++)
                    {
                        FistBump(i);
                        FistBump(i);
                        FistBump(i);
                    }
                    return false;
                }

                return true;
            }


            private static void FistBump(int i)
            {
                new EmoteChore(
                    Components.MinionIdentities[i].GetComponent<ChoreProvider>(),
                    Db.Get().ChoreTypes.EmoteHighPriority,
                    TEST,
                    new HashedString[] { "working_pre" },
                    null);
            }

            private static void Cheer(int i)
            {
                new EmoteChore(Components.MinionIdentities[i].GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
                {
                    "cheer_pre",
                    "cheer_loop",
                    "cheer_pst"
                }, null);
            }
        }
    }
}
