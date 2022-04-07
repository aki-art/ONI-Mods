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
                ModAssets.LateLoadAssets();

                Global.Instance.gameObject.AddComponent<TileAssets>();
                Global.Instance.gameObject.AddComponent<TileAssetLoader>();

                var modPath = Path.Combine(Mod.ModPath, "data", "default");
                TileAssetLoader.LoadAssets(modPath);
            }
        }
    }
}
