using Database;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class KleiPermitDioaramaVisPatch
	{
		[HarmonyPatch(typeof(KleiPermitDioramaVis), "GetPermitVisTarget")]
		public class KleiPermitDioramaVis_GetPermitVisTarget_Patch
		{
			public static void Postfix(KleiPermitDioramaVis __instance, PermitResource permit, ref IKleiPermitDioramaVisTarget __result)
			{
				if (__result.Equals(__instance.fallbackVis))
				{
					var def = KleiPermitVisUtil.GetBuildingDef(permit);
					if (def != null && def.PrefabID == DoorConfig.ID)
						__result = __instance.buildingOnFloorVis;
				}
			}
		}
	}
}
