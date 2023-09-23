using DecorPackA.UI;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class DetailsScreenPatch
	{
		[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				FUtility.FUI.SideScreen.AddClonedSideScreen<DecorPackA_MoodLampSideScreen>("DecorPackA Mood Lamp Side Screen", typeof(MonumentSideScreen));

				FUtility.FUI.SideScreen.AddCustomSideScreen<DecorPackA_RotatableSideScreen>("DecorPackA Rotatable Sidescreen", ModAssets.Prefabs.rotatableMoodlampSidescreen);

				FUtility.FUI.SideScreen.AddCustomSideScreen<DecorPackA_TintableSideScreen>("DecorPackA Tintable Sidescreen", ModAssets.Prefabs.tintableMoodlampSidescreen);

				FUtility.FUI.SideScreen.AddCustomSideScreen<DecorPackA_ParticleSelectorSideScreen>("DecorPackA Particle Selector Sidescreen", ModAssets.Prefabs.particleSelectorSidescreen);
			}
		}
	}
}
