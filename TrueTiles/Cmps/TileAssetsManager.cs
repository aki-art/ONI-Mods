using FUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTiles.Cmps
{
    internal class TileAssetsManager : KMonoBehaviour
    {
        public static TileAssetsManager Instance { get; private set; }
        private TileAssetsDict tileAssets;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            tileAssets = new TileAssetsDict();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void LoadAssets(PackData packData)
        {
            var dataPath = Path.Combine(packData.CurrentPath, "tiles.json");

            if (!File.Exists(dataPath))
            {
                Log.Warning("No data");
                return;
            }

            OverLoadFromJson(dataPath);
        }

        private void OverLoadFromJson(string path)
        {
            var json = File.ReadAllText(path);
        }

    }
}
