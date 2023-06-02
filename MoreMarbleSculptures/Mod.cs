using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using MoreMarbleSculptures.FUtilityArt.Components;
using MoreMarbleSculptures.Settings;
using System.Collections.Generic;

namespace MoreMarbleSculptures
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<ArtOverrideRestorer> artRestorers = new Components.Cmps<ArtOverrideRestorer>();
        public static List<string> myOverrides = new List<string>();
        public static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(path);
        }
    }
}
