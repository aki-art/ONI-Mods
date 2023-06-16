using HarmonyLib;
using Twitchery.Content.Defs;

namespace Twitchery.Patches
{
    public class LegacyModMainPatch
    {

/*        [HarmonyPatch(typeof(LegacyModMain), "Load")]
        public class LegacyModMain_Load_Patch
        {
            public static void Postfix()
            {
                var screenPip = new ScreenPipConfig();
                var def = screenPip.CreatePrefab();

                Assets.AddPrefab(def.GetComponent<KPrefabID>());
            }
        }*/
    }
}
