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
        public string exteriorPath;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            packs = new List<PackData>();
            roots = new Dictionary<string, string>();
            exteriorPath = Mod.GetExternalSavePath();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void LoadExteriorPacks()
        {
            if (!Directory.Exists(exteriorPath))
            {
                Log.Debuglog($"This path does not exist: {exteriorPath}");
                return;
            }

            foreach (var item in Directory.GetDirectories(exteriorPath))
            {
                LoadPack(item);
            }
        }

        public void LoadAllPacksFromFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Log.Warning($"This path does not exist: {path}");
                return;
            }

            foreach (var item in Directory.GetDirectories(path))
            {
                LoadPack(item);
            }
        }

        public PackData LoadPack(string path)
        {
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
                var packData = JsonConvert.DeserializeObject<PackData>(metaDataJson);

                if (packData.Root.IsNullOrWhiteSpace() || !Directory.Exists(packData.Root))
                {
                    packData.Root = path;
                }

                roots[packData.Id] = path;

                TryLoadIcon(packData.Root, packData);
                SetTextureCount(packData);

                var existingIdx = packs.FindIndex(p => p.Id == packData.Id);

                if (existingIdx != -1)
                {
                    packs.RemoveAt(existingIdx);
                }

                packs.Add(packData);

                return packData;
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

        private void SetTextureCount(PackData packData)
        {
            var texturesPath = Path.Combine(packData.Root, "textures");

            if (!Directory.Exists(texturesPath))
            {
                packData.TextureCount = 0;
            }
            else
            {
                packData.TextureCount = Directory.GetFiles(texturesPath, "*.png", SearchOption.AllDirectories).Length;
            }
        }

        private void TryLoadIcon(string path, PackData packData)
        {
            var iconPath = Path.Combine(path, "icon.png");
            if (File.Exists(iconPath))
            {
                packData.Icon = FUtility.Assets.LoadTexture("icon", path);
            }
        }
    }
}
