using FUtility;
using HarmonyLib;
using System;

namespace DecorPackA.Integration
{
    public class BluePrintsMod
    {
        public static void TryPatch(Harmony harmony)
        {
            var UtilitiesType = Type.GetType("Blueprints.Utilities, Blueprints", false, false);
            if (UtilitiesType is object)
            {
                var original = UtilitiesType.GetMethod("IsBuildable");
                var postfix = typeof(Blueprints_Utilities_IsBuildable_Patch).GetMethod("Postfix");
                harmony.Patch(original, null, new HarmonyMethod(postfix));

                Log.Info("Patched Blueprints mod's method Utility.IsBuildable to accept Stained Glass Tiles.");
            }
        }

        public static class Blueprints_Utilities_IsBuildable_Patch
        {
            // Check if the building is my tile, and make sure it is accepted as a blueprintable building
            public static void Postfix(BuildingDef buildingDef, ref bool __result)
            {
                if (buildingDef.BuildingComplete.HasTag(ModAssets.Tags.stainedGlass))
                {
                    __result = true;
                }
            }
        }
    }
}
