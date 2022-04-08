using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            packs = new List<PackData>();
            var path = Path.Combine(root, folder);

            if (!Directory.Exists(path))
            {
                Log.Warning($"Path does not exist: {path}.");
                return;
            }

            foreach (var item in Directory.EnumerateFileSystemEntries(path))
            {
                Log.Debuglog(item);

                var texturesPath = Path.Combine(item, "textures");

                if(!Directory.Exists(texturesPath))
                {
                    Log.Warning($"Texture pack was defined at {Path.GetFileNameWithoutExtension(item)}, but it has no textures folder associated.");
                    continue;
                }

                var dataPath = Path.Combine(item, "data");

                if (!Directory.Exists(dataPath))
                {
                    Log.Warning($"Texture pack was defined at {Path.GetFileNameWithoutExtension(item)}, but it has no data folder associated.");
                    continue;
                }

                var metadataPath = Path.Combine(item, METADATA_FILENAME);

                if (File.Exists(metadataPath))
                {
                    var metadata = File.ReadAllText(metadataPath);
                    var data = JsonConvert.DeserializeObject<PackData>(metadata);
                    
                    if(!data.Preview.IsNullOrWhiteSpace())
                    {
                        data.Icon = FUtility.Assets.LoadTexture(data.Preview, item);
                    }

                    data.Path = item;
                    data.TextureCount = Directory.GetFiles(texturesPath, "*.png").Length;

                    packs.Add(data);
                }
            }

            packs.Sort((a, b) => a.Order.CompareTo(b.Order));

            Log.Debuglog($"Finished loading {packs.Count} packs.");
        }

        public void Save()
        {
            foreach(var pack in packs)
            {
                var data = JsonConvert.SerializeObject(pack, Formatting.Indented);
                File.WriteAllText(Path.Combine(pack.Path, METADATA_FILENAME), data);
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

            [JsonIgnore]
            public Texture2D Icon { get; set; }

            [JsonIgnore]
            public int TextureCount { get; set; }

            [JsonIgnore]
            public string Path { get; set; }
        }
    }
}
