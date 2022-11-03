using FUtility;
using HarmonyLib;
using SnowSculptures.Content.Buildings;

namespace SnowSculptures.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                SnowSculptureConfig.RegisterArtableStages(Db.Get().ArtableStages);

                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, SnowSculptureConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, IceSculptureConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.UTILITIES, GlassCaseConfig.ID, Consts.SUB_BUILD_CATEGORY.Utilities.TEMPERATURE);
            }
        }
    }
}
