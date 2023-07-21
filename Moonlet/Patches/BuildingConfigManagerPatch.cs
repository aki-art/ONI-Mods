using HarmonyLib;
using Moonlet.Entities;

namespace Moonlet.Patches
{
	public class BuildingConfigManagerPatch
	{
        [HarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding")]
        public class BuildingConfigManager_RegisterBuilding_Patch
        {
            public static bool Prefix(IBuildingConfig config)
            {
                return !(config is GenericBuildingConfig genericConfig && genericConfig.skipLoading);
            }
        }
	}
}
