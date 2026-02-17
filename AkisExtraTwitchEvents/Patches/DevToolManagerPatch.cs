namespace Twitchery.Patches
{
	public class DevToolManagerPatch
	{
#if DEBUG
		// forces the debug menu to be on by default
		[HarmonyPatch(typeof(DevToolManager), nameof(DevToolManager.UpdateShouldShowTools))]
		public class DevToolManager_UpdateShouldShowTools_Patch
		{
			[HarmonyPriority(Priority.Last)]
			public static void Postfix(ref bool ___showImGui) => ___showImGui = true;
		}
#endif
	}
}