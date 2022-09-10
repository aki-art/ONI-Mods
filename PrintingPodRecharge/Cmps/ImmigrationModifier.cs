using FUtility;
using HarmonyLib;
using KSerialization;
using System;
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

        public bool IsBundleSuperDuplicant()
        {
            return IsOverrideActive && selectedBundle == Bundle.SuperDuplicant;
        }

        public int maxItems = 4;
        public int dupeCount = 1;
        public int itemCount = 3;
        public Color bgColor = Color.white;
        public Color glowColor = Color.white;
        public bool swapAnim = false;
        public bool randomColor = false;
        public KAnimFile[] bgAnim;

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

            if (Mod.IsArtifactsInCarePackagesHere)
            {
                eggInfos.Add(new CarePackageInfo("EggRock", 1, () => GameClock.Instance.GetCycle() >= Mod.Settings.EggCycle && UnityEngine.Random.value < 0.2f));
                eggInfos.Add(new CarePackageInfo("RainbowEggRock", 1, () => GameClock.Instance.GetCycle() >= Mod.Settings.RainbowEggCycle && UnityEngine.Random.value < 0.2f));
            }

            bundles.Add(Bundle.Egg, new CarePackageBundle(eggInfos, 0, 0, 5, Color.yellow, Color.yellow));

            var seedInfos = GetInfosByTag(originalPackages, GameTags.Seed);
            bundles.Add(Bundle.Seed, new CarePackageBundle(seedInfos, 0, 0, 5));

            var metalInfos = GetMetals(originalPackages);

            metalInfos.Add(CreatePackage(SimHashes.Aluminum.ToString(), 100f, 12, false));
            metalInfos.Add(CreatePackage(SimHashes.Lead.ToString(), 300f, 30, false));
            metalInfos.Add(CreatePackage(SimHashes.IronOre.ToString(), 1200f, 24, false));
            metalInfos.Add(CreatePackage(SimHashes.Niobium.ToString(), 100f, 200, true));
            metalInfos.Add(CreatePackage(SimHashes.Gold.ToString(), 500f, 0, false));

            metalInfos.Add(CreateEarlyPackage(SimHashes.Cuprite.ToString(), 500f, 12)); // after 12, 2000
            metalInfos.Add(CreateEarlyPackage(SimHashes.GoldAmalgam.ToString(), 500f, 12)); // after 12, 2000
            metalInfos.Add(CreateEarlyPackage(SimHashes.GoldAmalgam.ToString(), 500f, 12)); // after 12, 2000
            metalInfos.Add(CreateEarlyPackage(SimHashes.Copper.ToString(), 100f, 24)); // after 24, 400
            metalInfos.Add(CreateEarlyPackage(SimHashes.Iron.ToString(), 100f, 24)); // after 12, 400
            metalInfos.Add(CreateEarlyPackage(SimHashes.AluminumOre.ToString(), 50f, 24)); // after 48, 100

            if (DlcManager.FeatureClusterSpaceEnabled())
            {
                metalInfos.Add(CreateLimitedPackage(SimHashes.DepletedUranium.ToString(), 50f, 12, 32));
                metalInfos.Add(CreatePackage(SimHashes.DepletedUranium.ToString(), 300f, 32, false));
            }

            bundles.Add(Bundle.Metal, new CarePackageBundle(metalInfos, 0, 0, 4));

            var foodInfos = GetInfosByComponent(originalPackages, typeof(Edible));

            RemoveTag(foodInfos, FieldRationConfig.ID);
            foodInfos.Add(CreateEarlyPackage(FieldRationConfig.ID, 5f, 20));
            foodInfos.Add(CreateLimitedPackage(FieldRationConfig.ID, 10f, 20, 100));
            foodInfos.Add(CreatePackage(FieldRationConfig.ID, 15f, 100));
            foodInfos.Add(CreatePackage(ColdWheatBreadConfig.ID, 5f, 12));
            foodInfos.Add(CreatePackage(BurgerConfig.ID, 2f, 30));
            foodInfos.Add(CreatePackage(ForestForagePlantConfig.ID, 2f, 0));
            foodInfos.Add(CreatePackage(MushroomWrapConfig.ID, 3f, 50));
            foodInfos.Add(CreatePackage(SpiceBreadConfig.ID, 4f, 30));

            if (DlcManager.FeatureClusterSpaceEnabled())
            {
                foodInfos.Add(CreatePackage(GammaMushConfig.ID, 5f, 30));
            }

            bundles.Add(Bundle.Food, new CarePackageBundle(foodInfos, 0, 0, 5));

            bundles.Add(Bundle.SuperDuplicant, new CarePackageBundle(null, 3, 5, 0, Util.ColorFromHex("604bbe"), Util.ColorFromHex("a976e3")));
            bundles.Add(Bundle.Shaker, new CarePackageBundle(null, 4, 4, 0, Util.ColorFromHex("555555"), Util.ColorFromHex("888888")));

            if (Mod.IsTwitchIntegrationHere)
            {
                var twitchInfos = new List<CarePackageInfo>()
                {
                    CreatePackage($"{GeyserGenericConfig.ID}_{GeyserGenericConfig.SmallVolcano}", 1f, 0),
                    CreatePackage("PropFacilityTable", 1f, 0),
                    CreatePackage("PropFacilityCouch", 1f, 0),
                    CreatePackage(PuftAlphaConfig.ID, 1f, 0),
                    CreatePackage(FoodCometConfig.ID, 1f, 0),
                    CreatePackage(DustCometConfig.ID, 1f, 0),
                    CreatePackage(SimHashes.Corium.ToString(), 300f, 0),
                    CreatePackage(SimHashes.TempConductorSolid.ToString(), 0.001f, 0),
                    CreatePackage(SimHashes.DirtyWater.ToString(), 2f, 0),
                    CreatePackage(SimHashes.Cement.ToString(), 200f, 0),
                    CreatePackage(SimHashes.Mercury.ToString(), 100f, 0),
                    CreatePackage(GlomConfig.ID, 10, 0),
                    CreatePackage(MushBarConfig.ID, 1, 0)
                };

                bundles.Add(Bundle.Twitch, new CarePackageBundle(twitchInfos, 0, 0, 4, Color.white, Color.white) 
                    { 
                        bgAnim = new KAnimFile[] { Assets.GetAnim("rpp_twitch_select_kanim") 
                    } 
                });
                ;
            }
        }

        private CarePackageInfo CreateLimitedPackage(string ID, float amount, int afterCycle, int beforeCycle)
        {
            var time = GameClock.Instance.GetCycle();
            return new CarePackageInfo(ID, amount, () => time < beforeCycle && time >= afterCycle);
        }

        private void RemoveTag(List<CarePackageInfo> infos, string ID)
        {
            infos.RemoveAll(i => i.id == ID);
        }

        private CarePackageInfo CreateEarlyPackage(string ID, float amount, int beforeCycle = 0)
        {
            return new CarePackageInfo(ID, amount, () => GameClock.Instance.GetCycle() < beforeCycle);
        }

        private CarePackageInfo CreatePackage(string ID, float amount, int cycleCondition = 0, bool hasToBeDiscovered = false)
        {
            return new CarePackageInfo(ID, amount, () =>
                GameClock.Instance.GetCycle() >= cycleCondition && (!hasToBeDiscovered || DiscoveredResources.Instance.IsDiscovered(ID)));
        }

        private static bool IsMetal(Tag tag)
        {
            if (ElementLoader.GetElement(tag) is Element element)
            {
                return element.HasTag(GameTags.Metal) || element.HasTag(GameTags.RefinedMetal) || element.HasTag(GameTags.PreciousMetal);
            }

            return false;
        }

        private static List<CarePackageInfo> GetMetals(List<CarePackageInfo> originalPackages)
        {
            return originalPackages.Where(p => IsMetal(p.id)).ToList();
        }

        private static List<CarePackageInfo> GetInfosByComponent(List<CarePackageInfo> originalPackages, Type component)
        {
            return originalPackages.Where(p => Assets.TryGetPrefab(p.id)?.GetComponent(component) != null).ToList();
        }

        private static List<CarePackageInfo> GetInfosByTag(List<CarePackageInfo> originalPackages, Tag tag)
        {
            foreach (var item in originalPackages)
            {
                Log.Debuglog("ORIGINAL " + item.id + " " + string.Join(", ", Assets.TryGetPrefab(item.id)?.GetComponent<KPrefabID>()?.Tags));
            }

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

            randomColor = selectedBundle == Bundle.Shaker;

            dupeCount = current.GetDupeCount();
            itemCount = current.packageCount - dupeCount;
            bgColor = current.printerBgTint;
            glowColor = current.printerBgTintGlow;
            swapAnim = current.replaceAnim;
            bgAnim = current.bgAnim;
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
            if (bundles[selectedBundle]?.info == null)
            {
                Log.Warning("No Package Info!");
                return null;
            }

            var infos = bundles[selectedBundle].info.Where(i => i.requirement == null || i.requirement.Invoke()).ToList();

            if (infos == null)
            {
                Log.Warning("No Valid Package Info!");
                return null;
            }

            Log.Debuglog("Selectiong package from " + infos.Count);

            return infos.GetRandom();
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
            public KAnimFile[] bgAnim;

            public CarePackageBundle(List<CarePackageInfo> info, int dupeCountMin, int dupeCountMax, int packageCount, Color bg, Color fx, string bgAnim = "rpp_greyscale_dupeselect_kanim") : this(info, dupeCountMin, dupeCountMax, packageCount)
            {
                printerBgTint = bg;
                printerBgTintGlow = fx;
                replaceAnim = true;
                this.bgAnim = new KAnimFile[] { Assets.GetAnim(bgAnim) };
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
            SuperDuplicant,
            Metal,
            Food,
            Shaker,
            Twitch
        }

        private int selection = 0;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 300, 200, 500));

            GUILayout.Box("Modifiers");

            GUILayout.Label("Current Modifier: " + selectedBundle.ToString());

            selection = GUILayout.SelectionGrid(selection, Enum.GetNames(typeof(Bundle)), 2);

            if (GUILayout.Button("Set Bundle"))
            {
                SetModifier((Bundle)selection);
            }

            if (GUILayout.Button($"Force Print {(Bundle)selection}"))
            {
                SetModifier((Bundle)selection);

                ImmigrantScreen.InitializeImmigrantScreen(GameUtil.GetActiveTelepad().GetComponent<Telepad>());
                Game.Instance.Trigger((int)GameHashes.UIClear);
            }

            GUILayout.EndArea();
        }
    }
}
