using FUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;
using TileDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, TrueTiles.TileAssetLoader.TileData>>;

namespace TrueTiles
{
    public class TileAssetLoader : KMonoBehaviour
    {
        // will be nulled once things are loaded
        public static TileAssetLoader Instance;

        private TileDictionary tiles;
        private JObject serializedData;


        private bool finishedLoading;

        private static readonly string[] delimiter = new string[]
        {
            "::"
        };

        private const string RESET = "reset";

        protected override void OnPrefabInit()
        {
            serializedData = new JObject();
            Instance = this;
        }

        public static void LoadAssets(string root)
        {
            if (Instance == null)
            {
                Log.Warning($"TileAssetLoader isn't initialized. Was LoadAssets called too early?");
                return;
            }

            if (!Directory.Exists(root))
            {
                Log.Warning($"Path does not exist: {root}.");
                return;
            }

            foreach (var item in Directory.EnumerateFiles(root))
            {
                if (Path.GetExtension(item).ToLowerInvariant() == ".json")
                {
                    Instance.OverLoadFromJson(root, File.ReadAllText(item));
                }
            }

            Log.Info("Loaded tile art overrides from " + root.Normalize().Replace(Path.Combine(Util.RootFolder(), "mods").Normalize(), ""));
        }

        protected override void OnCleanUp()
        {
            tiles = null;
            serializedData = null;
            base.OnCleanUp();
            Instance = null;
        }

        private void OverLoadFromJson(string root, string json)
        {
            var dataDict = JsonConvert.DeserializeObject<TileDictionary>(json);

            TurnPathsAbsolute(root, dataDict);

            if(tiles is null)
            {
                tiles = new TileDictionary();
            }

            Merge(tiles, dataDict);

            Log.Debuglog($"Loaded json {tiles.Count} {root}");
        }

        private void Merge(TileDictionary first, TileDictionary second)
        {
            foreach(var tile in second)
            {
                if(first.ContainsKey(tile.Key))
                {
                    foreach (var variant in tile.Value)
                    {
                        if(first[tile.Key].ContainsKey(variant.Key))
                        {
                            first[tile.Key][variant.Key] = variant.Value;
                        }
                        else
                        {
                            first[tile.Key].Add(variant.Key, variant.Value);
                        }
                    }
                }
                else
                {
                    first.Add(tile.Key, tile.Value);
                }
            }
        }


        private void TurnPathsAbsolute(string root, TileDictionary dataDict)
        {
            foreach (var item in dataDict)
            {
                foreach (var entry in item.Value)
                {
                    entry.Value.MainTex = GetPathEntry(root, entry.Value.MainTex);
                    entry.Value.MainSpecular = GetPathEntry(root, entry.Value.MainSpecular);
                    entry.Value.TopTex = GetPathEntry(root, entry.Value.TopTex);
                    entry.Value.TopSpecular = GetPathEntry(root, entry.Value.TopSpecular);
                }
            }
        }

        private string GetPathEntry(string root, string path)
        {
            return path == RESET ? path : GetAbsolutePath(root, path);
        }

        private string GetAbsolutePath(string root, string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return null;
            }

            var parts = path.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            if (parts != null && parts.Length > 1)
            {
                if (parts[0] == "truetiles")
                {
                    return Path.Combine(Utils.ModPath, "data", "tiles", "textures", path + ".png");
                }
            }

            return Path.Combine(root, path + ".png");
        }

        private void DeserializeData()
        {
            tiles = serializedData.ToObject<TileDictionary>();
        }

        public void LoadOverrides()
        {
            if (finishedLoading)
            {
                Log.Warning("LoadOverrides is being called a second time, this should not happen!");
                return;
            }

            if (tiles is null)
            {
                //DeserializeData();
                Log.Debuglog("TILES IS NULL????");
            }

            if (ElementLoader.elements == null || ElementLoader.elements.Count == 0)
            {
                Log.Warning("Loading overrides too early, ElementLoader.elements don't exist yet");
                return;
            }

            foreach (var tile in tiles)
            {
                Log.Debuglog($"Loading tile: {tile.Key}");
                var buildingID = tile.Key;

                foreach (var variant in tile.Value)
                {
                    Log.Debuglog($"   {variant.Key}");
                    var element = ElementLoader.GetElement(variant.Key);

                    if (element is null)
                    {
                        continue;
                    }

                    var tileData = variant.Value;

                    var asset = new TileAssets.TextureAsset
                    {
                        main = LoadTex(tileData.MainTex),
                        specular = LoadTex(tileData.MainSpecular),
                        specularColor = GetColor(tileData.MainSpecularColor),
                        top = LoadTex(tileData.TopTex),
                        topSpecular = LoadTex(tileData.TopSpecular),
                        topSpecularColor = GetColor(tileData.TopSpecularColor),
                        normalMap = LoadTex(tileData.NormalTex),
                        specularFrequency = tileData.Frequency
                    };

                    Log.Debuglog($"Registered tile: {tile.Key} {variant.Key} {tileData.MainTex}");

                    TileAssets.Instance.Add(buildingID, element.id, asset);
                }
            }

            finishedLoading = true;
            CleanUp();
        }

        private void CleanUp()
        {
            Destroy(this);
        }

        private Color GetColor(string hex)
        {
            return string.IsNullOrEmpty(hex) ? Color.white : Util.ColorFromHex(hex);
        }

        private Texture2D LoadTex(string path)
        {
            return string.IsNullOrEmpty(path) || path == RESET ? null : FUtility.Assets.LoadTexture(path);
        }

        public class TileData
        {
            public string MainTex { get; set; }

            public string TopTex { get; set; }

            public string MainSpecular { get; set; }

            public string TopSpecular { get; set; }

            public string MainSpecularColor { get; set; }

            public string TopSpecularColor { get; set; }

            public string NormalTex { get; set; }

            public float Frequency { get; set; }

            public bool Transparent { get; set; }
        }
    }
}
