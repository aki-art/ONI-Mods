using Harmony;

namespace PollingOfONIUITest.Acid
{
    public class AcidCorroderPatch
    {
        static int acidIdx = 0;
        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public static class Game_OnSpawn_Patch
        {
            public static void Postfix()
            {
                acidIdx = ElementLoader.GetElementIndex(Acid.AcidSimHash);
                SaveGame.Instance.gameObject.AddComponent<AcidCorroder>();
            }
        }
    }
}
