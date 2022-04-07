using CrittersDropBones.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;
        public static SaveDataManager<RecipesConfig> recipeConfig;

        public static Config Settings => config.Settings;

        public static RecipesConfig Recipes => recipeConfig.Settings;

        public const string PREFIX = "CDB_";

        public static string Prefix(string name)
        {
            return PREFIX + name;
        }

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);

            recipeConfig = new SaveDataManager<RecipesConfig>(path, true, true, "cooker_recipes");

            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
