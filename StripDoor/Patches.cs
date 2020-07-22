using Harmony;

namespace StripDoor
{
    class Patches
    {
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