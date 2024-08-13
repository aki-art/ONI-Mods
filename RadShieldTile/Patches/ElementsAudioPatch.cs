using HarmonyLib;
using RadShieldTile.Content;

namespace RadShieldTile.Patches
{
	[HarmonyPatch(typeof(ElementsAudio), "LoadData")]
	public class ElementsAudio_LoadData_Patch
	{
		public static void Postfix(ElementsAudio __instance, ref ElementsAudio.ElementAudioConfig[] ___elementAudioConfigs)
		{
			___elementAudioConfigs = ___elementAudioConfigs.AddRangeToArray(RSTElements.CreateAudioConfigs(__instance));
		}
	}
}
