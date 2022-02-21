using CrittersDropBones.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;
        public static SaveDataManager<RecipesConfig> recipeConfig;
        public static Config Settings => config.Settings;
        public static RecipesConfig Recipes => recipeConfig.Settings;


        public const string PREFIX = "CDB_";

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);
            config.WriteIfDoesntExist(false);

            recipeConfig = new SaveDataManager<RecipesConfig>(path, true, true, "cooker_recipes");
            recipeConfig.WriteIfDoesntExist(false);

            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
