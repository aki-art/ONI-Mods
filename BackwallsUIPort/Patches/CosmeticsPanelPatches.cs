using BackwallsUIPort.Scripts;
using HarmonyLib;

namespace BackwallsUIPort.Patches
{
	public class CosmeticsPanelPatches
	{
		[HarmonyPatch(typeof(CosmeticsPanel), "Refresh")]
		public class CosmeticsPanel_Refresh_Patch
		{
			public static void Postfix(CosmeticsPanel __instance)
			{
				__instance.gameObject.AddOrGet<PUIP_CosmeticsPanelExtension>().RefreshUI();
			}
		}
	}
}
