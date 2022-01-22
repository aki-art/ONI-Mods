using HarmonyLib;

namespace DecorPackB.Patches
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                // Make Fossil Nodule tag usable as a building material
                GameTags.MaterialBuildingElements.Add(ModAssets.Tags.FossilNodule);
            }
        }
    }
}
