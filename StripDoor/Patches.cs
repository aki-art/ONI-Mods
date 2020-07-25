using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        /* This is neccessary, otherwise the doors that were open on game start will 
           refuse to update and not actually act like they are open. */
        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
                Debug.Log("Game Onspawn");
                GameScheduler.Instance.Schedule("ForceUpdateStripDoors", .33f, ForceUpdateDoors);
            }

            private static void ForceUpdateDoors(object _)
            {
                var stripDoors = GetBuildingsByID(StripDoorConfig.ID);

                foreach (var sd in stripDoors)
                {
                    Door door = sd.GetComponent<Door>();
                    if (door == null)
                        return;
                    if (door.IsOpen())
                        ForceSetSimState(sd, door);
                }
            }

            private static void ForceSetSimState(BuildingComplete building, Door door)
            {
                try
                {
                    Traverse.Create(door).Method("SetSimState", true, building.PlacementCells).GetValue();
                }
                catch (Exception e)
                {
                    Debug.Log("Could not force stripdoor sim state: " + e);
                }
            }
            private static List<BuildingComplete> GetBuildingsByID(string name)
            {
                name += "Complete";
                return Components.BuildingCompletes.Items.FindAll(b => b.name == name);
            }
        }


        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                string techGroup = "Luxury";
                var techList = new List<string>(Database.Techs.TECH_GROUPING[techGroup]);
                int index = techList.FindIndex(i => i == ManualPressureDoorConfig.ID);
                Debug.Log(index + "index");
                techList.Insert(index == -1 ? techList.Count - 1 : index, StripDoorConfig.ID);

                Database.Techs.TECH_GROUPING[techGroup] = techList.ToArray();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                string ID = StripDoorConfig.ID.ToUpperInvariant();
                string path = $"STRINGS.BUILDINGS.PREFABS.{ID}.";
                string light = STRINGS.UI.FormatAsLink("Light", "LIGHT");
                string decor = STRINGS.UI.FormatAsLink("Decor", "DECOR");

                Strings.Add(path + "NAME",
                    "Strip Door");
                Strings.Add(path + "EFFECT",
                    "A transparent insulating door.");
                Strings.Add(path + "DESC",
                    $"Quarters off dangerous areas and prevents gases from seeping into the colony, while allowing {light} and {decor} to pass through.");

                ModUtil.AddBuildingToPlanScreen("Base", StripDoorConfig.ID);
            }
        }
    }
}