using HarmonyLib;

namespace Moonlet.Patches
{
	internal class LegacyModMainPatch
	{

		[HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
		public class LegacyModMain_ConfigElements_Patch
		{
			public static void Postfix()
			{
				Mod.elementsLoader.ApplyToActiveTemplates(template => template.ApplyModifiers());
			}
		}
	}
}
