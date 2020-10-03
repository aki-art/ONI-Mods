using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularStorage
{
    class BuildMenuPatches
    {


        [HarmonyPatch(typeof(PlanScreen), "OnPrefabInit")]
        public static class PlanScreen_OnPrefabInit_Patch
        {
            public static void Prefix(Dictionary<HashedString, string> ___iconNameMap)
            {
                TUNING.BUILDINGS.PLANORDER.Add(
                    new PlanScreen.PlanInfo(new HashedString(Tuning.ModularStorageCategory), false, new List<string>
                    {
                        buildings.DebugControllerConfig.ID,
                        buildings.LiquideStorageModuleConfig.ID,
                        buildings.TestModuleConfig.ID,
                    }));

                ___iconNameMap.Add(HashCache.Get().Add(Tuning.ModularStorageCategory), "icon_action_region_disposal");
            }
        }


        /*        [HarmonyPatch(typeof(BuildMenu), "PopulateCategorizedMaps")]
                public static class BuildMenu_PopulateCategorizedMaps_Patch
                {
                    public static void Prefix()
                    {
                        Debug.Log("PopulateCategorizedMaps ");
                        var list = BuildMenu.OrderedBuildings.data as IList<BuildMenu.DisplayInfo>;
                        Debug.Log(list.Count);
                        list.Add(
                            new BuildMenu.DisplayInfo(HashCache.Get().Add("Test"), "icon_category_base", Action.Plan1, KKeyCode.None, new List<BuildMenu.DisplayInfo>
                            {
                                new BuildMenu.DisplayInfo(HashCache.Get().Add("StorageModules"), "icon_category_base", Action.BuildCategoryTiles, KKeyCode.T, new List<BuildMenu.BuildingInfo>
                                {
                                    new BuildMenu.BuildingInfo(buildings.TestModuleConfig.ID,Action.BuildMenuKeyD),
                                    new BuildMenu.BuildingInfo(buildings.LiquideStorageModuleConfig.ID,Action.BuildMenuKeyZ),
                                    new BuildMenu.BuildingInfo(buildings.DebugControllerConfig.ID,Action.BuildMenuKeyX)
                                })
                            }));
                    }
                }*/

    }
}
