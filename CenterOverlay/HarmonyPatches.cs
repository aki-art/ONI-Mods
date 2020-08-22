using FUtility;
using Harmony;
using UnityEngine;

namespace CenterOverlay
{
    public class HarmonyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Log.PrintVersion();
                ModAssets.Initialize(path);
            }
        }


        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }

        // adding modded tag to modded buildings
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                var prefabs = Assets.GetPrefabsWithComponent<Building>();
                foreach (var prefab in prefabs)
                {
                    string name = prefab.name.Replace("Template", "").Replace("Complete", "").Replace("UnderConstruction", "").Replace("Preview", "");
                    if (!ModAssets.vanillaBuildings.Contains(name))
                    {
                        prefab.gameObject.GetComponent<KPrefabID>().AddTag(ModAssets.ModdedBuilding, false);
                    }
                }
            }
        }

        /* UNFINISHED Blows up buildings placed on the wrong side 
        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                var kPrefabID = __instance.GetComponent<KPrefabID>();
                if (kPrefabID.HasTag(ModAssets.ModdedBuilding) && IsOnWrongSide(__instance.transform.position))
                    {
                         __instance.gameObject.AddOrGet<TimeBomb>();
                    }
            }
        } */

        private static bool IsOnModdedSide(Vector3 position)
        {
            var midPoint = Grid.WidthInCells / 2 + ModAssets.Offset;
            return (Grid.PosToXY(position).x > midPoint) ^ ModAssets.moddedOnLeft;
        }

        // Tints the preview when building something
        [HarmonyPatch(typeof(BuildTool), "UpdateVis")]
        public static class BuildTool_UpdateVis_Patch
        {
            public static void Postfix(Vector3 pos, BuildingDef ___def, BuildTool __instance)
            {
                var building = ___def.BuildingComplete.GetComponent<KPrefabID>();
                if (building.HasTag(ModAssets.ModdedBuilding))
                {
                    if (!IsOnModdedSide(pos))
                    {
                        var component = __instance.visualizer.GetComponent<KBatchedAnimController>();
                        if (component != null)
                        {
                            if(!component.TintColour.Equals(Color.red))
                                component.TintColour = ModAssets.moddedFullBright;
                        }
                    }
                }
            }
        }

        // Generates the JSON file with all the buildings currently loading
        /*[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                List<string> buildingIDs = Assets.BuildingDefs.Select(n => n.name).ToList();
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "buildings");
                var filePath = Path.Combine(path, "buildings.json");
                Directory.CreateDirectory(path);
                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = JsonConvert.SerializeObject(buildingIDs, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                }
            }
        }*/
    }
}
