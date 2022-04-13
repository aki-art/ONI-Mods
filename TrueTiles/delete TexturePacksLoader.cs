/*using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TrueTiles
{
    internal class TexturePacksLoader : KMonoBehaviour
    {
        public static TexturePacksLoader Instance;
        const string METADATA_FILENAME = "metadata.json";

        public List<PackData> packs;

        public void LoadPacks(string root, string folder = "data")
        {
            Log.Debuglog("Loading packs from " + root);
            // load from outside if exists
            var externalFolder = Path.Combine(Util.RootFolder(), "mods", "tile_texture_packs");
            var localPath = Path.Combine(root, folder);

            packs = new List<PackData>();

            if (!Directory.Exists(localPath) && !Directory.Exists(externalFolder))
            {
                Log.Warning($"No valid paths to load tile textures from.");
                return;
            }

            SetPaths(localPath);
            CopyToExternal();

            if (Directory.Exists(externalFolder))
            {
                Log.Debuglog("loading external");
                foreach (var packPath in Directory.EnumerateFileSystemEntries(externalFolder))
                {
                    LoadJsonData(packPath);
                }
            }

            packs.Sort((a, b) => a.Order.CompareTo(b.Order));
            Log.Debuglog($"Finished loading {packs.Count} packs.");
        }

        private void SetPaths(string localPath)
        {
            throw new NotImplementedException();
        }

        private void LoadJsonData(string item)
        {
            Log.Debuglog("Loading " + item);

            var texturesPath = Path.Combine(item, "textures");

            if (!Directory.Exists(texturesPath))
            {
                Log.Warning($"Texture pack was defined at {Path.GetFileNameWithoutExtension(item)}, but it has no textures folder associated.");
                return;
            }

            var dataPath = Path.Combine(item, "data");

            if (!Directory.Exists(dataPath))
            {
                Log.Warning($"Texture pack was defined at {Path.GetFileNameWithoutExtension(item)}, but it has no data folder associated.");
                return;
            }

            var metadataPath = Path.Combine(item, METADATA_FILENAME);

            if (File.Exists(metadataPath))
            {
                var metadata = File.ReadAllText(metadataPath);
                var data = JsonConvert.DeserializeObject<PackData>(metadata);

                if (packs.Any(p => p.Id == data.Id))
                {
                    Log.Info($"Skipping loading {data.Id}, already loaded a pack with the same ID.");
                    return;
                }

                if (!data.Preview.IsNullOrWhiteSpace())
                {
                    data.Icon = FUtility.Assets.LoadTexture(data.Preview, item);
                }

                data.Path = item;
                data.TextureCount = Directory.GetFiles(texturesPath, "*.png").Length;

                packs.Add(data);
            }
        }

        private static string GetOrCreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not create directory: " + e.Message);
                return null;
            }
        }

        public void Save()
        {
            foreach (var pack in packs)
            {
                var metaDataPath = Path.Combine(pack.Path, METADATA_FILENAME);
                WriteMetaData(pack, metaDataPath);
            }

            CopyToExternal();
        }

        private void CopyToExternal()
        {
            var externalFolder = GetOrCreateDirectory(Path.Combine(Util.RootFolder(), "mods", "tile_texture_packs"));

            if (externalFolder.IsNullOrWhiteSpace())
            {
                Log.Warning("Something went wrong trying to sync texture pack data. Data will be loaded from local configurations instead.");
                return;
            }

            foreach (var pack in packs)
            {
                var extPackPath = GetOrCreateDirectory(Path.Combine(externalFolder, pack.Id));

                var localMetaDataFilePath = Path.Combine(pack.Path, METADATA_FILENAME);
                var extMetaDataFilePath = Path.Combine(extPackPath, METADATA_FILENAME);
                File.Copy(localMetaDataFilePath, extMetaDataFilePath);

                var localDataFolderPath = Path.Combine(pack.Path, "data");
                var extDataFolderPath = GetOrCreateDirectory(Path.Combine(extPackPath, "data"));
                FileUtil.CopyDirectory(localDataFolderPath, extDataFolderPath, true, false);
            }
        }

        private static void WriteMetaData(PackData pack, string path)
        {
            var data = JsonConvert.SerializeObject(pack, Formatting.Indented);
            WriteJson(data, path);
        }

        private static void WriteJson(string data, string path)
        {
            try
            {
                File.WriteAllText(path, data);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning($"Could not write file at {path}: " + e.Message);
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        [Serializable]
        public class PackData
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Author { get; set; }

            public string Description { get; set; }

            public string Preview { get; set; }

            public int Order { get; set; } = 999;

            public bool Enabled { get; set; }

            public string Path { get; set; }

            [JsonIgnore]
            public Texture2D Icon { get; set; }

            [JsonIgnore]
            public int TextureCount { get; set; }
        }
    }
}
*/