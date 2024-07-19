using FUtility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TrueTiles.Cmps
{
	public class TexturePacksManager : KMonoBehaviour
	{
		public static TexturePacksManager Instance;
		public List<PackData> packs;
		public Dictionary<string, string> roots;
		public Dictionary<string, PackData> userDefinedData;
		public string exteriorPath;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
			packs = new List<PackData>();
			roots = new Dictionary<string, string>();
			exteriorPath = Mod.GetExternalSavePath();
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void LoadExteriorPacks()
		{
			if (!Directory.Exists(exteriorPath))
			{
				Log.Debug($"This path does not exist: {exteriorPath}");
				return;
			}

			foreach (var item in Directory.GetDirectories(exteriorPath))
				LoadPack(item);

			var savePath = Mod.Settings.SaveExternally ? Mod.GetExternalSavePath() : Mod.GetLocalSavePath();
			Instance.SavePacks(savePath);
		}

		public void LoadAllPacksFromFolder(string path)
		{
			if (!Directory.Exists(path))
			{
				Log.Warning($"This path does not exist: {path}");
				return;
			}

			foreach (var item in Directory.GetDirectories(path))
				LoadPack(item);
		}


		public void ReadUserSettings()
		{
			string path = exteriorPath;
			userDefinedData = [];

			if (Directory.Exists(path))
			{
				var metaDataPath = Path.Combine(path, "metadata.json");
				if (File.Exists(metaDataPath))
				{
					if (FileUtil.TryReadFile(metaDataPath, out var metaDataJson))
					{
						var packData = JsonConvert.DeserializeObject<PackData>(metaDataJson);
						userDefinedData.Add(packData.Id, packData);
					}
				}
			}
		}

		public PackData LoadPack(string path)
		{
			Log.Debug("LOADING " + path);

			if (!Directory.Exists(path))
			{
				Log.Warning($"This path does not exist: {path}");
				return null;
			}

			var metaDataPath = Path.Combine(path, "metadata.json");

			if (!File.Exists(metaDataPath))
			{
				Log.Warning($"Folder marked as texture pack, but has no metadata.json set: {path}");
				return null;
			}

			if (FileUtil.TryReadFile(metaDataPath, out var metaDataJson))
			{
				// TODO: i dont need to read these files twice, definitely needs a fix. but for now i dont expect more then 4-5 per user
				var packData = JsonConvert.DeserializeObject<PackData>(metaDataJson);

				/*				if (userDefinedData.TryGetValue(packData.Id, out var overrideData))
									packData = overrideData;*/


				if (packData.Root.IsNullOrWhiteSpace() || !Directory.Exists(packData.Root))
					packData.Root = path;

				if (Path.GetDirectoryName(packData.Root).EndsWith("tiles"))
					packData.Root = path;

				roots[packData.Id] = path;

				SetTextureCountFromPNGs(packData);

				if (IsPackValid(packData))
				{
					var existingIdx = packs.FindIndex(p => p.Id == packData.Id);

					if (existingIdx != -1)
						packs.RemoveAt(existingIdx);

					var duplicate = packs.Find(p => p.Id == packData.Id);
					if (duplicate != null)
					{
						var thisVersion = packData.GetSystemVersion();
						var existingVersion = duplicate.GetSystemVersion();

						if (thisVersion <= existingVersion)
						{
							Log.Warning($"Duplicate pack found: {packData.Id}. Keeping {duplicate.Root}, discarding {packData.Root}.");
							return null;
						}
						else
						{
							Log.Warning($"Duplicate pack found: {packData.Id}. Keeping {packData.Root}, discarding {duplicate.Root}.");
						}
					}

					TryLoadIcon(packData.Root, packData);

					packs.Add(packData);

					packData.IsValid = true;
					return packData;
				}
				else
				{
					Log.Warning($"Pack missing: {packData.Id}");
				}

				packData.IsValid = false;
			}

			return null;
		}

		public void SortPacks()
		{
			packs.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));
		}

		public void SavePacks(string root)
		{
			foreach (var pack in packs)
			{
				var data = JsonConvert.SerializeObject(pack, Formatting.Indented);
				var path = FileUtil.GetOrCreateDirectory(Path.Combine(root, pack.Id));

				File.WriteAllText(Path.Combine(path, "metadata.json"), data);
			}
		}

		private void SetTextureCountFromPNGs(PackData packData)
		{
			var texturesPath = Path.Combine(packData.Root, "textures");

			packData.TextureCount = !Directory.Exists(texturesPath)
				? 0
				: Directory.GetFiles(texturesPath, "*.png", SearchOption.AllDirectories).Length;
		}

		private void TryLoadIcon(string path, PackData packData)
		{
			var iconPath = Path.Combine(path, "icon.png");
			if (File.Exists(iconPath))
				packData.Icon = FAssets.LoadTexture("icon", path);
		}

		public bool IsPackValid(PackData pack)
		{
			if (!Directory.Exists(pack.Root))
				return false;

			var hasAssetBundleDefined = !pack.AssetBundle.IsNullOrWhiteSpace();

			var bundlePath = hasAssetBundleDefined
				? Path.Combine(pack.Root, pack.AssetBundle)
				: null;

			if (hasAssetBundleDefined)
				Log.Debug("asset bundle pack");
			else
				Log.Debug("loads " + pack.TextureCount + " textures");

			var hasAssetBundleData = !bundlePath.IsNullOrWhiteSpace() && File.Exists(bundlePath);

			Log.Debug($"has asset data: {hasAssetBundleData} {bundlePath}");

			if (pack.TextureCount == 0 && !hasAssetBundleData)
				return false;

			return true;
		}
	}
}
