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
        public static DropsConfig dropsConfig = new DropsConfig();
        public static SaveDataManager<RecipesConfig> recipeConfig;

        public static RecipesConfig Recipes => recipeConfig.Settings;

        public const string PREFIX = "CrittersDropBones_";

        public static bool IsSpookyPumpkinHere;

        public override void OnLoad(Harmony harmony)
        {
            recipeConfig = new SaveDataManager<RecipesConfig>(Utils.ModPath, filename: "cooker_recipes");

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
