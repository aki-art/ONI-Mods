using DecorPackB.Content.Defs.Buildings;
using DecorPackB.Content.ModDb;
using DecorPackB.Integration.Twitch;
using FUtility;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix(Db __instance)
            {
                RegisterBuildings();

                DPDb.StatusItems.Register();
                DPAccessories.Register(__instance.AccessorySlots);
                DPSkillPerks.Register(__instance.SkillPerks);
                DPSkills.Register(__instance.Skills);
                DPArtableStages.Register(Db.GetArtableStages());

                ModAssets.PostDbInit();
                TwitchEvents.PostDbInit();
            }

            [HarmonyPostfix]
            [HarmonyPriority(Priority.LowerThanNormal)]
            public static void LatePostfix(Db __instance)
            {
                if(Mod.isBeachedHere)
                {
                    var archeologySkill = __instance.Skills.TryGet("Beached_Archeology");
                    archeologySkill?.perks.Add(DPSkillPerks.CanFindTreasures);
                }
            }

            private static void RegisterBuildings()
            {
                // ModUtil.AddBuildingToPlanScreen(FountainConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, FossilDisplayConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.DECOR, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, PotConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.STORAGE, StorageLockerConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, GiantFossilDisplayConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.DECOR, FossilDisplayConfig.ID);
                // BuildingUtil.AddToPlanScreen(OilLanternConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);

                //BuildingUtil.AddToResearch(FountainConfig.ID, Consts.TECH.DECOR.FINEART);
                BuildingUtil.AddToResearch(FossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                //BuildingUtil.AddToResearch(GiantFossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                // BuildingUtil.AddToResearch(OilLanternConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION)
            }
        }
    }
}
