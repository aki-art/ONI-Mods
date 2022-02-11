using FUtility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TrueTiles
{
    internal class ModAssets
    {
        public static Dictionary<string, Dictionary<string, TileTextures>> texturesByName;

        public static void LateLoadAssets()
        {
            string path = Path.Combine(Mod.Path, "tiles");

            if(!Directory.Exists(path))
            {
                Log.Warning("Folder for texture overrides is missing (should be {path}.");
                return;
            }

            LoadOverrides(path);
        }

        private static void LoadOverrides(string path)
        {
            texturesByName = new Dictionary<string, Dictionary<string, TileTextures>>();

            foreach (string item in Directory.EnumerateDirectories(path))
            {
                string id = Path.GetFileName(item);
                Log.Info($"Loading texture overrides for {id}");

                LoadTextureOverridesForID(item, id);
            }
        }

        private static void LoadTextureOverridesForID(string path, string prefabID)
        {
            Dictionary<string, TileTextures> items = new Dictionary<string, TileTextures>();

            foreach (string item in Directory.EnumerateDirectories(path))
            {
                string id = Path.GetFileName(item);
                Log.Info($"    Loaded {id}");

                if(FUtility.Assets.TryLoadTexture(Path.Combine(item, "main.png"), out Texture2D main))
                {
                    Texture2D top = FUtility.Assets.LoadTexture(Path.Combine(item, "spec.png"), false);
                    Texture2D spec = FUtility.Assets.LoadTexture(Path.Combine(item, "top.png"), false);
                    Texture2D topSpec = FUtility.Assets.LoadTexture(Path.Combine(item, "top_spec.png"), false);

                    items.Add(id, new TileTextures(main, top, spec, topSpec));
                }
                else
                {
                    Log.Warning($"Folder for {prefabID} {id} was found, but there is no main.png.");
                }
            }

            texturesByName.Add(prefabID, items);
        }
    }
}
