using HarmonyLib;

namespace SketchPad.Patches
{
    internal class WorldPatch
    {
        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
              //  SketchBook sketchBook = SaveLoader.Instance.gameObject.AddComponent<SketchBook>();
               // sketchBook.linePrefab = ModAssets.linePrefab;
            }
        }
    }
}
