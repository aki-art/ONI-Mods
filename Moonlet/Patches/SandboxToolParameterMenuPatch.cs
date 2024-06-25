using FUtility;
using HarmonyLib;
using Moonlet.Scripts;

namespace Moonlet.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		[HarmonyPatch(typeof(SandboxToolParameterMenu), nameof(SandboxToolParameterMenu.ConfigureEntitySelector))]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var menu = SandboxUtil.AddModMenu(
					__instance,
					"Moonlet Temporary",
					Def.GetUISprite(Assets.GetPrefab(CookedMeatConfig.ID)),
					obj => obj is KPrefabID id && id.TryGetComponent<MoonletComponentHolder>(out var holder) && holder.addToSandboxMenu);

				SandboxUtil.UpdateOptions(__instance);
			}
		}
	}
}
