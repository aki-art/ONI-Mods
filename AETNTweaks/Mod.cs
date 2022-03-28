using AETNTweaks.Cmps;
using AETNTweaks.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using UnityEngine;

namespace AETNTweaks
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;
        public static Components.Cmps<MassiveHeatSink> AETNs = new Components.Cmps<MassiveHeatSink>();

        public static float GetSqrDistance(MonoBehaviour A, MonoBehaviour B)
        {
            return (A.transform.position - B.transform.position).sqrMagnitude;
        }

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            config = new SaveDataManager<Config>(path);
        }
    }
}
