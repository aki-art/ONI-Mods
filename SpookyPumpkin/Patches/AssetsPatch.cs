using HarmonyLib;

namespace SpookyPumpkinSO.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				FUtility.Assets.LoadSprites(__instance);
			}

			public static void Postfix()
			{
				var luxureBedDef = Assets.GetBuildingDef(LuxuryBedConfig.ID);
				//luxureBedDef.BuildingComplete.GetComponent<KPrefabID>().prefabSpawnFn += Utils.FixFacadeLayers;

				/*				// https://forums.kleientertainment.com/klei-bug-tracker/oni/skinned-bed-not-yet-builded-appear-as-if-it-was-after-a-reload-r39445/
								luxureBedDef.BuildingUnderConstruction.GetComponent<KPrefabID>().prefabSpawnFn += go =>
								{
									Log.Debug("playing place anim");
									go.GetComponent<KBatchedAnimController>().Play("place");
								};*/
			}
		}
	}
}
