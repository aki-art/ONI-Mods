using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CenterOverlay
{
    class ModAssets
    {
        public static GameObject settingsScreenPrefab;
        public static Settings Settings { get; set; }
        public static void Initialize()
        {
            Settings = new Settings();
        }
    }
}
