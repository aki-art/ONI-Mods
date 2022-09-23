using CrittersDropBones.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;
        public static SaveDataManager<RecipesConfig> recipeConfig;

        public static Config Settings => config.Settings;

        public static RecipesConfig Recipes => recipeConfig.Settings;

        public const string PREFIX = "CrittersDropBones_";

        public static bool IsSpookyPumpkinHere;

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);

            recipeConfig = new SaveDataManager<RecipesConfig>(path, true, true, "cooker_recipes");

            base.OnLoad(harmony);
            Log.PrintVersion();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach(var mod in mods)
            {
                if(mod.staticID == "SpookyPumpkin" && mod.IsEnabledForActiveDlc())
                {
                    IsSpookyPumpkinHere = true;
                }
            }
        }
    }
}
