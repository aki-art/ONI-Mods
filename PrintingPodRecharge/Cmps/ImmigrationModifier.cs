using FUtility;
using KSerialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrintingPodRecharge.Items;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    public class ImmigrationModifier : KMonoBehaviour
    {
        public bool IsOverrideActive;
        public int selectedIndex = 0;
        public int maxItems = 4;
        public int dupeCount = 1;
        public int itemCount = 3;
        public Color bgColor = Color.white;
        public Color glowColor = Color.white;
        public bool swapAnim = false;

        private Bundle selectedBundle = Bundle.None;

        public static ImmigrationModifier Instance { get; private set; }

        private Dictionary<Bundle, CarePackageBundle> bundles;

        public CarePackageBundle CurrentBundle => IsOverrideActive ? null : bundles[selectedBundle];

        public void SetModifier(Bundle bundle)
        {
            Log.Debuglog("SET moDIFIER TO " + bundle.ToString());
            selectedBundle = bundle;

            if (bundle == Bundle.None)
            {
                IsOverrideActive = false;
                return;
            }

            IsOverrideActive = true;

            var current = bundles[selectedBundle];
            dupeCount = current.GetDupeCount();
            itemCount = current.packageCount - dupeCount;
            bgColor = current.printerBgTint;
            glowColor = current.printerBgTintGlow;
            swapAnim = current.replaceAnim;
        }

        public int GetDupeCount(int otherwise)
        {
            return IsOverrideActive ? dupeCount : otherwise;
        }

        public int GetItemCount(int otherwise)
        {
            return IsOverrideActive ? itemCount : otherwise;
        }

        public void LoadBundles()
        {
            bundles = new Dictionary<Bundle, CarePackageBundle>();
            var path = Path.Combine(Utils.ModPath, "data", "packages");

            foreach(var file in Directory.EnumerateFiles(path, "*.json"))
            {
                var text = File.ReadAllText(file);

                if (text.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var bundleData = JsonConvert.DeserializeObject<BundleData>(text);

                var package = bundleData.Packages?.Select(p => CreatePackageInfo(p)).ToArray();

                var bgColor = bundleData.BgColor.IsNullOrWhiteSpace() ? default : Util.ColorFromHex(bundleData.BgColor);
                var fxColor = bundleData.BgColor.IsNullOrWhiteSpace() ? default : Util.ColorFromHex(bundleData.BgColor);

                var bundle = new CarePackageBundle(
                    package, 
                    bundleData.DupeCount.Min, 
                    bundleData.DupeCount.Max, 
                    bundleData.PackageCount);

                if(!bundleData.BgColor.IsNullOrWhiteSpace())
                {
                    if(bundleData.FXColor.IsNullOrWhiteSpace())
                    {
                        Log.Warning($"A Package config must have 0 or 2 colors specified. {bundleData.ID} only has BG color.");
                    }
                    else
                    {
                        bundle.SetColors(Util.ColorFromHex(bundleData.BgColor), Util.ColorFromHex(bundleData.FXColor));
                    }
                }

                bundles[bundleData.ID] = bundle;

            }
        }

        private static CarePackageInfo CreatePackageInfo(BundleData.Package data)
        {
            return new CarePackageInfo(data.PrefabID, data.Amount, null);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            LoadBundles();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public CarePackageInfo GetRandomPackage()
        {
            var infos = bundles[selectedBundle].info;
            if (infos == null)
            {
                return null;
            }
            var index = Random.Range(0, infos.Length);
            return infos[index];
        }

        public class CarePackageBundle
        {
            public CarePackageInfo[] info;
            public int dupeCountMin;
            public int dupeCountMax;
            public int packageCount;
            public Color printerBgTint;
            public Color printerBgTintGlow;
            public bool replaceAnim;

            public void SetColors(Color bg, Color fx)
            {
                printerBgTint = bg;
                printerBgTintGlow = fx;
                replaceAnim = true;
            }

            public CarePackageBundle(CarePackageInfo[] info, int dupeCountMin, int dupeCountMax, int packageCount)
            {
                this.info = info;
                this.dupeCountMin = dupeCountMin;
                this.dupeCountMax = dupeCountMax;
                this.packageCount = packageCount;
            }

            public int GetDupeCount()
            {
                return Random.Range(dupeCountMin, dupeCountMax + 1);
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Bundle
        {
            Egg,
            Seed,
            Duplicant,
            SuperDuplicant,
            Metal,
            None
        }
    }
}
