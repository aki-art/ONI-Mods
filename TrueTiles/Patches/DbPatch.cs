using HarmonyLib;

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

                var go = Global.Instance.gameObject;
                go.AddComponent<TileAssets>();
                go.AddComponent<TileAssetLoader>();
                go.AddComponent<TexturePacksLoader>();

                // Loads pack data
                TexturePacksLoader.Instance.LoadPacks(Mod.ModPath);
                
                // Loads textures
                foreach(var pack in TexturePacksLoader.Instance.packs)
                {
                    if (pack.Enabled)
                    {
                        TileAssetLoader.LoadAssets(pack.Path);
                    }
                }
            }
        }
    }
}
