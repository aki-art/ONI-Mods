using HarmonyLib;

namespace Moonlet.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				foreach (var mod in Mod.modLoaders)
					mod.translationsLoader.LoadTranslations();
			}
		}
	}
}
