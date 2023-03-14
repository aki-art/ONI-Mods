global using FUtility;
using DecorPackA.Buildings;
using DecorPackA.Patches;
using DecorPackA.Settings;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;

namespace DecorPackA
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackA_";
        public static SaveDataManager<Config> config;
        public static Components.Cmps<FacadeRestorer> facadeRestorers = new();

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);

            Log.PrintVersion();

            ConditionalPatching(harmony);
            base.OnLoad(harmony);
        }

        public void ConditionalPatching(Harmony harmony)
        {
            if (Settings.GlassTile.UseDyeTC)
            {
                AdditionalDetailsPanelPatch.Patch(harmony);
            }
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            config.WriteIfDoesntExist(false, null);
            Integration.BluePrintsMod.TryPatch(harmony);
        }
    }
}
