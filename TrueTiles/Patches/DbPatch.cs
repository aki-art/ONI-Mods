using FUtility;
using HarmonyLib;
using System.IO;

namespace TrueTiles.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                Global.Instance.gameObject.AddComponent<TileAssets>();
                Global.Instance.gameObject.AddComponent<TileAssetLoader>();

                string modPath = Path.Combine(Mod.ModPath, "data", "tiles");
                TileAssetLoader.LoadAssets(modPath);
            }
        }
    }
}
