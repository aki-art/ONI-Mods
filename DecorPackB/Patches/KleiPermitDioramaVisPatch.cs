using Database;
using DecorPackB.Content.ModDb;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class KleiPermitDioramaVisPatch
	{

		[HarmonyPatch(typeof(KleiPermitDioramaVis), "Init")]
		public class KleiPermitDioramaVis_Init_Patch
		{
			public static void Postfix(KleiPermitDioramaVis __instance)
			{
				DPPermitDioramas.Init(__instance);
			}
		}

		[HarmonyPatch(typeof(KleiPermitDioramaVis), "GetPermitVisTarget")]
		public class KleiPermitDioramaVis_GetPermitVisTarget_Patch
		{
			public static void Postfix(KleiPermitDioramaVis __instance, PermitResource permit, ref IKleiPermitDioramaVisTarget __result)
			{
				if (permit.Category != PermitCategory.Artwork)
					return;

				var def = KleiPermitVisUtil.GetBuildingDef(permit);

				if (def.HasValue && DPInventory.useMuseumDefs.Contains(def.Unwrap().PrefabID))
				{
					__result = DPPermitDioramas.museum;
				}
			}
		}
	}
}
