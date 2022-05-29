using Newtonsoft.Json;
using System;
using UnityEngine;

namespace TrueTiles
{
    [Serializable]
    public class PackData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public int Order { get; set; } = 999;

        public bool Enabled { get; set; }

        public string AssetBundle { get; set; }

        public string AssetBundleRoot { get; set; }

        public string Root { get; set; }

        [JsonIgnore]
        public Texture2D Icon { get; set; }

        [JsonIgnore]
        public int TextureCount { get; set; }
    }
}
