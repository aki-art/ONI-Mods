using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using TrueTiles.Cmps;

namespace TrueTiles.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                // my own UI assets
                ModAssets.LateLoadAssets();

                // initializing components
                var go = Global.Instance.gameObject;
                go.AddComponent<TileAssets>();
                go.AddComponent<TileAssetLoader>();
                go.AddComponent<TexturePacksManager>();

                // Loads pack data
                TexturePacksManager.Instance.LoadAllPacksFromFolder(Path.Combine(Mod.ModPath, "tiles"));

                // Load modded packs, if any
                if(Mod.addonPacks != null)
                {
                    foreach (var pack in Mod.addonPacks)
                    {
                        TexturePacksManager.Instance.LoadPack(pack);
                    }
                }

                // Should be loaded very last
                TexturePacksManager.Instance.LoadExteriorPacks();

                // Load actual assets and textures
                foreach (var pack in TexturePacksManager.Instance.packs.Values)
                {
                    if (pack.Enabled)
                    {
                        TileAssetLoader.LoadAssets(pack);
                    }
                }
            }
        }
    }
}
