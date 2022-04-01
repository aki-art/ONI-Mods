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

    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Postfix()
        {
            var buildingIDs = new List<string>();

            foreach (var building in Assets.BuildingDefs)
            {
                if(building.ShowInBuildMenu && ShouldBeAdded(building))
                {
                    buildingIDs.Add(building.PrefabID);
                }
            }

            // add a category to put the stuff in
            // calling without subcategory throws them in "uncategorized"
            PlanScreen.PlanInfo planInfo = new PlanScreen.PlanInfo(
                new HashedString(Mod.unsortedCategory),
                false,
                buildingIDs);

            TUNING.BUILDINGS.PLANORDER.Add(planInfo);

            Log.Debuglog("Added some buildings to a Misc build menu: ");
            Log.Debuglog(string.Join("\n", buildingIDs));
        }

        // these would be bad to be enabled
        static HashSet<string> disabled = new HashSet<string>()
        {
            // OP
            HeadquartersConfig.ID,
            TemporalTearOpenerConfig.ID,
            GravitasPedestalConfig.ID,
            DevLifeSupportConfig.ID,
            DevGeneratorConfig.ID,

            // DLC content not disabled in vanilla
            RocketEnvelopeWindowTileConfig.ID,

            // rocket interior stuff
            RocketInteriorGasInputConfig.ID,
            RocketInteriorGasInputPortConfig.ID,
            RocketInteriorGasOutputConfig.ID,
            RocketInteriorGasOutputPortConfig.ID,
            RocketInteriorPowerPlugConfig.ID,
            RocketInteriorSolidInputConfig.ID,
            RocketInteriorSolidOutputConfig.ID,
            RocketInteriorLiquidInputConfig.ID,
            RocketInteriorLiquidInputPortConfig.ID,
            RocketInteriorLiquidOutputConfig.ID,
            RocketInteriorLiquidOutputPortConfig.ID,

            // unimplemented / unstable / what even are some of these? why is there an EGG?
            CrewCapsuleConfig.ID,
            WarpPortalConfig.ID,
            AtmoicGardenConfig.ID,
            TeleportalPadConfig.ID,
            StaterpillarGeneratorConfig.ID
        };

        private static bool ShouldBeAdded(BuildingDef building)
        {
            var tag = building.PrefabID;

            // would lead to crashes and weird stuff, so disallow
            if (disabled.Contains(tag))
            {
                return false;
            }

            // prevent spam by my other mod lol
            if(building.BuildingComplete.HasTag("DecorPackA_StainedGlass"))
            {
                return false;
            }

            // no dlc content on vanilla
            if(!DlcManager.IsDlcListValidForCurrentContent(building.RequiredDlcIds)) {
                return false;
            }

            // check if it already exists in a menu
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
