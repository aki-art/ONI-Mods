using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using TileDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, TrueTiles.TileData>>;

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

			TexturePacksManager.Instance.SortPacks();

			LoadEnabledPacks(TexturePacksManager.Instance.packs);

			LoadOverrides();
		}


		public void LoadEnabledPacks(List<PackData> data)
		{
			foreach (var pack in data.Where(p => p.Enabled))
			{
				LoadAssets(pack);
			}
		}

		public override void OnPrefabInit()
		{
			Instance = this;
		}

		public static void LoadAssets(PackData packData)
		{
			var dataPath = Path.Combine(packData.Root, "tiles.json");

			if (!File.Exists(dataPath))
			{
				Log.Warning("No data at " + dataPath);
				return;
			}

			if (packData.AssetBundle.IsNullOrWhiteSpace())
			{
				Instance.OverLoadFromJson(dataPath);
			}
			else
			{
				Instance.OverLoadFromAssetBundle(dataPath, packData.AssetBundle, packData.AssetBundleRoot);
			}

			Log.Info("Loaded tile art overrides from " + dataPath.Normalize().Replace(Path.Combine(Util.RootFolder(), "mods").Normalize(), ""));
		}

		public override void OnCleanUp()
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

			Log.Debug($"Loaded json {tiles.Count} {path}");
		}

		private void OverLoadFromAssetBundle(string path, string assetBundleName, string assetPath)
		{
			var json = File.ReadAllText(path);

			var dataDict = JsonConvert.DeserializeObject<TileDictionary>(json);

			//TurnPathsAbsolute(texturePath, dataDict);

			foreach (var item in dataDict)
			{
				foreach (var entry in item.Value)
				{
					entry.Value.AssetBundle = assetBundleName;
					entry.Value.Root = Path.GetDirectoryName(path);
					entry.Value.AssetRoot = assetPath;
				}
			}

			if (tiles is null)
			{
				tiles = new TileDictionary();
			}

			Merge(tiles, dataDict);

			Log.Debug($"Loaded json {tiles.Count} {path}");
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
					entry.Value.Root = root;
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

				if (TexturePacksManager.Instance.roots.TryGetValue(prefix, out var roots))
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

			var stopWatch = new Stopwatch();
			stopWatch.Start();

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

			foreach (var texturedTile in Assets.GetPrefabsWithTag(ModAssets.Tags.texturedTile))
			{
				texturedTile.RemoveTag(ModAssets.Tags.texturedTile);
			}

			foreach (var tile in tiles)
			{
				Log.Debug($"Loading tile: {tile.Key}");
				var buildingID = tile.Key;
				var buildingDef = Assets.GetBuildingDef(buildingID);

				if (Assets.GetBuildingDef(buildingID) == null)
				{
					Log.Debug("Building ID not present, skipping: " + buildingID);
					continue;
				}

				buildingDef.BuildingComplete.AddTag(ModAssets.Tags.texturedTile);

				foreach (var variant in tile.Value)
				{
					Log.Debug($"   {variant.Key}");
					var element = ElementLoader.GetElement(variant.Key);

					if (element is null)
					{
						continue;
					}

					var tileData = variant.Value;
					var assetBundle = variant.Value.AssetBundle;

					var asset = new TileAssets.TextureAsset
					{
						main = LoadTex(tileData.MainTex, assetBundle, tileData.Root, tileData.AssetRoot),
						specular = LoadTex(tileData.MainSpecular, assetBundle, tileData.Root, tileData.AssetRoot),
						specularColor = GetColor(tileData.MainSpecularColor),
						top = LoadTex(tileData.TopTex, assetBundle, tileData.Root, tileData.AssetRoot),
						topSpecular = LoadTex(tileData.TopSpecular, assetBundle, tileData.Root, tileData.AssetRoot),
						topSpecularColor = GetColor(tileData.TopSpecularColor),
						normalMap = LoadTex(tileData.NormalTex, assetBundle, tileData.Root, tileData.AssetRoot),
						specularFrequency = tileData.Frequency
					};

					TileAssets.Instance.Add(buildingID, element.id, asset);
				}
			}

			stopWatch.Stop();
			var ts = stopWatch.Elapsed;

			var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

			Log.Info($"Loaded texture assets. Loading took {elapsedTime}");

			finishedLoading = true;
		}

		private Color GetColor(float[] values)
		{
			return values == null || values.Length != 4 ? Color.white : new Color(values[0], values[1], values[2], values[3]);
		}

		private Texture2D LoadTex(string path, string assetBundle, string root, string assetFolder)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}

			if (assetBundle.IsNullOrWhiteSpace())
			{
				return path == RESET ? null : FAssets.LoadTexture(path);
			}
			else
			{
				var bundle = FAssets.LoadAssetBundle(assetBundle, root);
				var asset = bundle.LoadAsset<Texture2D>(assetFolder + path + ".png");

				return asset;
			}
		}
	}
}
