using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace BuildingMenuReenabler
{
    internal class Mod : UserMod2
    {
        public static string unsortedCategory = "unsorted";
    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Postfix()
        {
            var buildingIDs = new List<string>();

            foreach (var building in Assets.BuildingDefs)
            {
                if(building.ShowInBuildMenu && NotInBuildMenu(building))
                {
                    buildingIDs.Add(building.PrefabID);
                }
            }


            // add a category to put the backwalls in
            // calling without subcategory throws them in "uncategorized"
            PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(
                new HashedString(Mod.unsortedCategory),
                false,
                buildingIDs);

            TUNING.BUILDINGS.PLANORDER.Add(planInfo);

            Log.Debuglog("Added some buildings to a Misc build menu: ");
            Log.Debuglog(string.Join("\n", buildingIDs));
        }

        private static bool NotInBuildMenu(BuildingDef building)
        {
            var tag = building.PrefabID;

            // vould lead to crashes and weird stuff, so disallow
            if(tag == HeadquartersConfig.ID)
            {
                return false;
            }

            // prevent spam by my other mod lol
            if(building.BuildingComplete.HasTag("DecorPackA_StainedGlass"))
            {
                return false;
            }
            
            foreach(var plan in TUNING.BUILDINGS.PLANORDER)
            {
                foreach(var data in plan.buildingAndSubcategoryData)
                {
                    if(data.Key == building.PrefabID)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            var iconNameMap = Traverse.Create(typeof(PlanScreen)).Field<Dictionary<HashedString, string>>("iconNameMap").Value;
            iconNameMap.Add(HashCache.Get().Add(Mod.unsortedCategory), "icon_category_morale");
        }
    }


    [HarmonyPatch(typeof(Localization), "Initialize")]
    public class Localization_Initialize_Patch
    {
        public static void Postfix()
        {
            Loc.Translate(typeof(STRINGS), true);
        }
    }
}
