using FUtility;
using HarmonyLib;
using SpookyPumpkin.Foods;
using TUNING;
using UnityEngine;

namespace SpookyPumpkin
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 4f * 600.0f, 1));
            }
        }
    }
}
