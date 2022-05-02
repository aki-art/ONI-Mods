using FUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TileDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, TrueTiles.Cmps.TileData>>;

namespace TrueTiles.Cmps
{
    public partial class TileAssetLoader : KMonoBehaviour
    {
        public static TileAssetLoader Instance;

        private TileDictionary tiles;

        private bool finishedLoading;

        private static readonly string[] delimiter = new string[]
        {
            "::"
        };

        private const string RESET = "reset";

        public void ReloadAssets()
        {
            finishedLoading = false;
            tiles = new TileDictionary();

            TileAssets.Instance.UnloadTextures();

            LoadPacksOrdered(TexturePacksManager.Instance.packs);

            LoadOverrides();
        }

        public void LoadPacksOrdered(Dictionary<string, PackData> data)
        {
            var packs = data.Values.ToList();
            packs.RemoveAll(p => !p.Enabled);
            packs.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));

            foreach (var pack in packs)
            {
                LoadAssets(pack);
            }
        }

        protected override void OnPrefabInit()
        {
            Instance = this;
        }

        public static void LoadAssets(PackData packData)
        {
            var dataPath = Path.Combine(packData.Root, "tiles.json");

            if (!File.Exists(dataPath))
            {
                Log.Warning("No data");
                return;
            }

            Instance.OverLoadFromJson(dataPath);

            Log.Info("Loaded tile art overrides from " + dataPath.Normalize().Replace(Path.Combine(Util.RootFolder(), "mods").Normalize(), ""));
        }

        protected override void OnCleanUp()
        {
            tiles = null;
            base.OnCleanUp();
            Instance = null;
        }

        private void OverLoadFromJson(string path)
        {
            var root = Path.GetDirectoryName(path);
            var texturePath = Path.Combine(root, "textures");

            var json = File.ReadAllText(path);

            var dataDict = JsonConvert.DeserializeObject<TileDictionary>(json);

            TurnPathsAbsolute(texturePath, dataDict);

            if (tiles is null)
            {
                tiles = new TileDictionary();
            }

            Merge(tiles, dataDict);

            Log.Debuglog($"Loaded json {tiles.Count} {root}");
        }

        private void Merge(TileDictionary first, TileDictionary second)
        {
            foreach (var buildingID in second)
            {
                if (first.ContainsKey(buildingID.Key))
                {
                    foreach (var elementVariant in buildingID.Value)
                    {
                        var firstBuildingEntry = first[buildingID.Key];

                        if (firstBuildingEntry.ContainsKey(elementVariant.Key))
                        {
                            firstBuildingEntry[elementVariant.Key] = elementVariant.Value;
                        }
                        else
                        {
                            firstBuildingEntry.Add(elementVariant.Key, elementVariant.Value);
                        }
                    }
                }
                else
                {
                    first.Add(buildingID.Key, buildingID.Value);
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
                    entry.Value.NormalTex = GetPathEntry(root, entry.Value.NormalTex);
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
                var prefix = parts[0];

                if(TexturePacksManager.Instance.roots.TryGetValue(prefix, out var roots))
                {
                    return Path.Combine(roots, "textures", path + ".png");
                }
            }

            return Path.Combine(root, path + ".png");
        }

        public void Reload()
        {
            finishedLoading = false;
            TileAssets.Instance.Clear();
            LoadOverrides();
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
                Log.Warning("There are no tile overrides enabled.");
                return;
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

                    TileAssets.Instance.Add(buildingID, element.id, asset);
                }
            }

            finishedLoading = true;
        }

        private Color GetColor(float[] values)
        {
            return values == null || values.Length != 4 ? Color.white : new Color(values[0], values[1], values[2], values[3]);
        }

        private Texture2D LoadTex(string path)
        {
            return string.IsNullOrEmpty(path) || path == RESET ? null : FUtility.Assets.LoadTexture(path);
        }
    }
}
