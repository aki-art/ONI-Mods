using Harmony;
using System.Linq;
using TUNING;

namespace WorldTraitsPlus.Entities
{
    public class Patches
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                CROPS.CROP_TYPES.RemoveAll(c => c.cropId == "WoodLog");
                CROPS.CROP_TYPES.Add(new Crop.CropVal("WoodLog", 0.25f * 600.0f, 1));
            }
        }
    }
}
