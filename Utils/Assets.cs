using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
    public class Assets
    {
        public static GameObject LoadUIPrefab(string assetBundleName, string rootObject)
        {
            Log.Info("Loading asset files... ");

            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    Log.Info($"Asset bundle {assetBundleName} loaded multiple times. Ignoring duplicates.");
                    return null;
                }
            }

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return null;
            }

            return assetBundle.LoadAsset<GameObject>(rootObject);
        }
    }
}
