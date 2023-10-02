using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;

namespace SpookyPumpkinSO.Patches
{
	public class BuildingFacadePatch
	{
		[HarmonyPatch(typeof(BuildingFacade), "ChangeBuilding")]
		public class BuildingFacade_ChangeBuilding_Patch
		{
			public static void Prefix(BuildingFacade __instance, ref object __state)
			{
				if (__instance.currentFacade != SPFacades.PUMPKINBED)
					return;

				if (__instance.TryGetComponent(out KBatchedAnimController kbac))
				{
					__state = new AnimState()
					{
						anim = kbac.currentAnim,
						mode = kbac.mode,
						hasValue = true
					};

					Log.Debug($"saved state: {kbac.currentAnim} {HashCache.Get().Get(kbac.currentAnim)}");
					Log.Debug(__instance.currentFacade);
				}
			}

			public static void Postfix(BuildingFacade __instance, ref object __state)
			{
				if (__instance.currentFacade != SPFacades.PUMPKINBED)
					return;

				if (__instance.TryGetComponent(out BuildingUnderConstruction _))
				{
					if (__instance.TryGetComponent(out KBatchedAnimController kbac))
					{
						kbac.Play("place");
					}
				}
				else if (__state is AnimState animState && animState.hasValue)
				{
					Log.Debug("restored state");
					if (__instance.TryGetComponent(out KBatchedAnimController kbac))
					{
						kbac.Play(HashCache.Get().Get(animState.anim), animState.mode);
					}
				}
			}
		}

		public struct AnimState
		{
			public HashedString anim;
			public KAnim.PlayMode mode;
			public bool hasValue;
		}
	}
}
