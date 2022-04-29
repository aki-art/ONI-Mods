using FUtility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TrueTiles
{
    public class TexturePacksManager : KMonoBehaviour
    {
        public static TexturePacksManager Instance;
        public Dictionary<string, PackData> packs;
        public string exteriorPath;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            packs = new Dictionary<string, PackData>();
            exteriorPath = Path.Combine(Util.RootFolder(), "mods", "tile_texture_packs");
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void LoadExteriorPacks()
        {
            LoadAllPacksFromFolder(exteriorPath); ;
        }

        public void LoadAllPacksFromFolder(string path)
        {
            if(!Directory.Exists(path))
            {
                Log.Warning($"This path does not exist: {path}");
                return;
            }

            foreach(var item in Directory.GetDirectories(path))
            {
                LoadPack(item);
            }
        }

        public void LoadPack(string path)
        {
            if(!Directory.Exists(path))
            {
                Log.Warning($"This path does not exist: {path}");
                return;
            }

            var metaDataPath = Path.Combine(path, "metadata.json");

            if(!File.Exists(metaDataPath))
            {
                Log.Warning($"Folder marked as texture pack, but has no metadata.json set: {path}");
                return;
            }

            var dataPath = Path.Combine(path, "data");

            if (!Directory.Exists(dataPath))
            {
                Log.Warning($"Tried to load pack data from this path, but there is no data folder: {path}");
                return;
            }

            if(FileUtil.TryReadFile(metaDataPath, out var metaDataJson))
            {
                var packData = JsonConvert.DeserializeObject<PackData>(metaDataJson);

                if(packData.Root.IsNullOrWhiteSpace())
                {
                    packData.Root = path;
                }

                TryLoadIcon(path, packData);
                SetTextureCount(packData);

                packs[packData.Id] = packData;
            }
        }

        public void SavePacks()
        {
            foreach (var pack in packs)
            {
                var data = JsonConvert.SerializeObject(pack.Value);
                var path = FileUtil.GetOrCreateDirectory(Path.Combine(exteriorPath, pack.Key));

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
