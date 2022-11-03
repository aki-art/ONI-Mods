using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using SnowSculptures.Patches;
using SnowSculptures.Settings;

namespace SnowSculptures
{
    public class Mod : UserMod2
    {
        public static Config Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Settings = new SaveDataManager<Config>(path).Settings;
            Log.PrintVersion(this);

            if(Settings.PreventInstantDebugMelt)
            {
                BuildToolPatch.Patch(harmony);
            }
        }
    }
}
