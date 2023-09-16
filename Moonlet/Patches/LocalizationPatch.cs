using HarmonyLib;

namespace Moonlet.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public class Localization_Initialize_Patch
		{
			public static void Postfix() => Mod.translationLoader.RegisterAll();
		}
	}
}
