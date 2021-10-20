using System.IO;
using System.Reflection;
using UnityEngine;

namespace PollingOfONIUITest
{
    public class ModAssets
    {
        public static string ModPath;
        public static GameObject settingsDialogPrefab;

        public static void LateLoadAssets()
        {
            AssetBundle bundle = LoadAssetBundle("asquaredtest");
            settingsDialogPrefab = bundle.LoadAsset<GameObject>("TwitchOptions");
            TMPConverter.ReplaceAllText(settingsDialogPrefab);
            settingsDialogPrefab.AddComponent<SettingsDemoDialog>();
        }

        public static AssetBundle LoadAssetBundle(string assetBundleName)
        {
            // if two mods added an assetbundle with the same name, the game will crash
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    return bundle;
                }
            }

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Debug.LogWarning($"Failed to load AssetBundle from path {path}");
                return null;
            }

            return assetBundle;
        }
    }
}
