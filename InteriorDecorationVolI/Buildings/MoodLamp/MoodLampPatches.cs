using Harmony;

namespace InteriorDecorationv1.Buildings.MoodLamp
{
    class MoodLampPatches
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<MoodLampSideScreen>(
                    "Mood Lamp Side Screen",
                    "MonumentSideScreen",
                    typeof(MonumentSideScreen));
            }
        }
    }
}
