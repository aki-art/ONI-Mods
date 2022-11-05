using HarmonyLib;
using SnowSculptures.Content.Buildings;
using UnityEngine;

namespace SnowSculptures.Patches
{
    public class BuildingLoaderPatch
    {
        [HarmonyPatch(typeof(BuildingLoader), "CreateBuildingComplete")]
        public class BuildingLoader_CreateBuildingComplete_Patch
        {
            public static void Postfix(GameObject go, BuildingDef def)
            {
                if(Mod.Settings.GlassCaseSealables.Contains(def.PrefabID))
                {
                    go.AddComponent<GlassCaseSealable>();
                }
            }
        }
    }
}
