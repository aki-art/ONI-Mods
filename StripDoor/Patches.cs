using Harmony;
using System;
using System.Collections.Generic;

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
                    "Quarters off dangerous areas and prevents gases from seeping into the colony." + STRINGS.UI.FormatAsLink("Light", "LIGHT") + " and " + STRINGS.UI.FormatAsLink("Decor", "DECOR") + " to pass through.\n\n");

                ModUtil.AddBuildingToPlanScreen("Base", StripDoorConfig.ID);
            }
        }

/*        [HarmonyPatch(typeof(DoorToggleSideScreen), "InitButtons")]
        public static class DoorToggleSideScreen_InitButtons_Patch
        {
            public static void Postfix(ref List<object> ___buttonList)
            {
                var lockButton = ___buttonList[___buttonList.Count - 1];
                KToggle lockKToggle = Traverse.Create(lockButton).Field("button").GetValue<KToggle>();
                //lockKToggle.gameObject.SetActive(false);
                lockKToggle.interactable = false;
                //___buttonList.RemoveAt(___buttonList.Count - 1);
            }
        }*/
    }
}