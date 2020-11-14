using FUtility;
using Harmony;
using System.IO;

namespace Asphalt
{
    public class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                ModAssets.ExternalSettingsPath = Path.Combine(Util.RootFolder(), "mods", "settings", "AsphaltTiles");
                ModSettings.Read();
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix() => Buildings.RegisterBuildings(typeof(AsphaltTileConfig));
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Postfix() => ModAssets.LateLoadAssets();
        }
    }
}