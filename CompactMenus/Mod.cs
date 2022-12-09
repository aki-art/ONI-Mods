﻿using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace CompactMenus
{
    public class Mod : UserMod2
    {
        public static string buildMenuSearch = "";

        private static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(Utils.ModPath);

            //ModAssets.CreatePrefabs();

        }
    }
}