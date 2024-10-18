using HarmonyLib;

namespace Moonlet.Patches
{
	public class AudioSheetsPatch
	{
		[HarmonyPatch(typeof(AudioSheets), "Initialize")]
		public class AudioSheets_Initialize_Patch
		{
			public static void Prefix(AudioSheets __instance)
			{
				Mod.kanimExtensionsLoader.LoadAudioSheets(__instance);
			}
		}
	}
}
