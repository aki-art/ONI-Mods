using FUtility;
using HarmonyLib;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    [SerializationConfig(KSerialization.MemberSerialization.OptIn)]
    public class ImmigrationModifier : KMonoBehaviour
    {
        [Serialize]
        public Bundle selectedBundle = Bundle.None;

        public bool IsOverrideActive;

        public bool IsBundleSuperDuplicant() => IsOverrideActive && selectedBundle == Bundle.SuperDuplicant;

        public int maxItems = 4;
        public int dupeCount = 1;
        public int itemCount = 3;
        public Color bgColor = Color.white;
        public Color glowColor = Color.white;
        public bool swapAnim = false;

        public static ImmigrationModifier Instance { get; private set; }

        private Dictionary<Bundle, CarePackageBundle> bundles;

        public CarePackageBundle CurrentBundle => IsOverrideActive ? null : bundles[selectedBundle];

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        public void CreateBundles()
        {
            bundles = new Dictionary<Bundle, CarePackageBundle>();

            var originalPackages = Traverse.Create(Immigration.Instance).Field<CarePackageInfo[]>("carePackages").Value.ToList();

            var eggInfos = GetInfosByTag(originalPackages, GameTags.Egg);
            bundles.Add(Bundle.Egg, new CarePackageBundle(eggInfos, 0, 0, 5, Color.yellow, Color.yellow));

            var seedInfos = GetInfosByTag(originalPackages, GameTags.Seed);
            bundles.Add(Bundle.Seed, new CarePackageBundle(seedInfos, 0, 0, 5));

            var metalInfos = GetInfosByTag(originalPackages, GameTags.Metal);
            bundles.Add(Bundle.Metal, new CarePackageBundle(metalInfos, 0, 0, 4));

            var foodInfos = GetInfosByTag(originalPackages, GameTags.Edible);
            bundles.Add(Bundle.Food, new CarePackageBundle(foodInfos, 0, 0, 5));

            bundles.Add(Bundle.Duplicant, new CarePackageBundle(null, 4, 5, 0));
            bundles.Add(Bundle.SuperDuplicant, new CarePackageBundle(null, 3, 5, 0));
        }

        private static List<CarePackageInfo> GetInfosByTag(List<CarePackageInfo> originalPackages, Tag tag)
        {
            return originalPackages.Where(p => HasTag(p.id, tag)).ToList();
        }

        private static bool HasTag(string prefabID, Tag tag)
        {
            return Assets.TryGetPrefab(prefabID)?.HasTag(tag) ?? false;
        }


        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (selectedBundle != Bundle.None)
            {
                SetModifier(selectedBundle);
            }
        }

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

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public CarePackageInfo GetRandomPackage()
        {
            var infos = bundles[selectedBundle].info.Where(i => i.requirement.Invoke());

            if (infos == null)
            {
                return null;
            }

            var index = UnityEngine.Random.Range(0, infos.Count());
            return infos.ElementAt(index);
        }

        public class CarePackageBundle
        {
            public List<CarePackageInfo> info;
            public int dupeCountMin;
            public int dupeCountMax;
            public int packageCount;
            public Color printerBgTint;
            public Color printerBgTintGlow;
            public bool replaceAnim;

            public CarePackageBundle(List<CarePackageInfo> info, int dupeCountMin, int dupeCountMax, int packageCount, Color bg, Color fx) : this(info, dupeCountMin, dupeCountMax, packageCount)
            {
                printerBgTint = bg;
                printerBgTintGlow = fx;
                replaceAnim = true;
            }

            public CarePackageBundle(List<CarePackageInfo> info, int dupeCountMin, int dupeCountMax, int packageCount)
            {
                this.info = info;
                this.dupeCountMin = dupeCountMin;
                this.dupeCountMax = dupeCountMax;
                this.packageCount = packageCount;
            }

            public int GetDupeCount()
            {
                return UnityEngine.Random.Range(dupeCountMin, dupeCountMax + 1);
            }
        }

        public enum Bundle
        {
            None = 0,
            Egg,
            Seed,
            Duplicant,
            SuperDuplicant,
            Metal,
            Food
        }
    }
}
