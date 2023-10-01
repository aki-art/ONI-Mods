using HarmonyLib;
using SpookyPumpkinSO.Content.Foods;
using TUNING;

namespace SpookyPumpkinSO.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				CROPS.CROP_TYPES.Add(new Crop.CropVal(PumpkinConfig.ID, 6f * 600.0f, 1));
			}
		}
	}
}
