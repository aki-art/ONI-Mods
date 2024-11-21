using Backwalls.Cmps;
using Backwalls.UI;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class DetailsScreenPatch
	{
		[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				FUtility.FUI.SideScreen.AddCustomSideScreen<BackwallSidescreen>("BackwallSideScreen", ModAssets.wallSidescreenPrefab);
			}
		}

		[HarmonyPatch(typeof(DetailsScreen), "OnKeyUp")]
		public class DetailsScreen_OnKeyUp_Patch
		{
			public static bool Prefix(DetailsScreen __instance, KButtonEvent e)
			{
				return __instance.target == null
					|| !__instance.target.TryGetComponent(out Backwall _)
					|| BackwallSidescreen.Instance == null
					|| !BackwallSidescreen.Instance.ShouldPreventRightClick();
			}
		}
	}
}
