using Harmony;
using System.Collections.Generic;

namespace StripDoor
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("[Strip Door] Loaded Strip Door version " + typeof(StripDoor).Assembly.GetName().Version.ToString());
            }
        }  

        // Adding to research tree
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var techList = new List<string>(Database.Techs.TECH_GROUPING["Luxury"])
                {
                    StripDoorConfig.ID
                };

                Database.Techs.TECH_GROUPING["Luxury"] = techList.ToArray();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                string ID = StripDoorConfig.ID.ToUpperInvariant();
                Strings.Add(
                    $"STRINGS.BUILDINGS.PREFABS.{ID}.NAME",
                    "Strip Door");
                Strings.Add(
                    $"STRINGS.BUILDINGS.PREFABS.{ID}.EFFECT",
                    "A transparent insulating door.");
                Strings.Add(
                    $"STRINGS.BUILDINGS.PREFABS.{ID}.DESC",
                    "Quarters off dangerous areas and prevents gases from seeping into the colony, while allowing " + STRINGS.UI.FormatAsLink("Light", "LIGHT") + " and " + STRINGS.UI.FormatAsLink("Decor", "DECOR") + " to pass through.\n\n");

                ModUtil.AddBuildingToPlanScreen("Base", StripDoorConfig.ID);

            }
        }
    }
}