using HarmonyLib;

namespace DecorPackA.Patches
{
	public class BuildingFacadePatch
	{

		[HarmonyPatch(typeof(BuildingFacade), "ChangeBuilding")]
		public class BuildingFacade_ChangeBuilding_Patch
		{
			public static void Postfix(BuildingFacade __instance)
			{
				if (__instance == null)
					return;

				__instance.Trigger(ModEvents.OnSkinChanged);
			}
		}
	}
}
