using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin
{
    public class ModAssets
    {
        public static string ModPath;
        public static GameObject sideScreenPrefab;
        public const string PREFIX = "SP_";
        public const string spooked = "SP_Spooked";

        public static void Initialize(string path)
        {
            ModPath = path;
        }

        internal static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("sp_uiasset");
            sideScreenPrefab = bundle.LoadAsset<GameObject>("GhostPipSideScreen");
            FUtility.FUI.TMPConverter.ReplaceAllText(sideScreenPrefab);
        }
    }
}
