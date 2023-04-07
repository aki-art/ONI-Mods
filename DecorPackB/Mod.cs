﻿using DecorPackB.Content;
using DecorPackB.Settings;
using FUtility;
using FUtility.Components;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace DecorPackB
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackB_";
        private static SaveDataManager<Config> config;
        private static SaveDataManager<LiteModeSettings> defaultLiteModeConfig;
        public static Components.Cmps<Restorer> restorers = new();
        public static bool DebugMode = true;

        public static bool isBeachedHere;
        public static bool isFullMinerYieldHere;

        public static Config Settings => config.Settings;

        public static LiteModeSettings LiteModeSettings => defaultLiteModeConfig.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            config = new SaveDataManager<Config>(Utils.ModPath);
            defaultLiteModeConfig = new SaveDataManager<LiteModeSettings>(Utils.ModPath, filename: "liteModeSettings");
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach(var mod in mods)
            {
                if(mod.IsEnabledForActiveDlc()) 
                {
                    switch(mod.staticID)
                    {
                        case "BertO.FullMinerYield":
                            isFullMinerYieldHere = true; break;
                        case "Beached":
                            isBeachedHere = true; break;
                    }
                }
            }

            Utils.RegisterDevTool<DPIIDevTool>("Mods/Decor Pack II");
        }
    }
}