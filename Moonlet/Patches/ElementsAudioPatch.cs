using HarmonyLib;

namespace Moonlet.Patches
{
	public class ElementsAudioPatch
	{
		[HarmonyPatch(typeof(ElementsAudio), nameof(ElementsAudio.LoadData))]
		public class ElementsAudio_LoadData_Patch
		{
			public static void Postfix(ElementsAudio __instance)
			{
				Mod.elementsLoader.ApplyToActiveTemplates(template => template.LoadAudioConfigs(__instance));
			}
		}
	}
}
