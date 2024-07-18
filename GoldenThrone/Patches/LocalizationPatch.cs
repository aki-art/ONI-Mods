using FUtility.FLocalization;
using HarmonyLib;

namespace GoldenThrone.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation(typeof(STRINGS), true);

				// copying the same lines, to save any future translators the effort of a copy paste
				Strings.Add("STRINGS.DUPLICANTS.MODIFIERS.GOLDENTHRONE_MONARCH.NAME", STRINGS.DUPLICANTS.STATUSITEMS.GOLDENTHRONE_ROYALLYRELIEVED.NAME);
				Strings.Add("STRINGS.DUPLICANTS.MODIFIERS.GOLDENTHRONE_MONARCH.DESCRIPTION", STRINGS.DUPLICANTS.STATUSITEMS.GOLDENTHRONE_ROYALLYRELIEVED.TOOLTIP);
				Strings.Add("STRINGS.DUPLICANTS.MODIFIERS.GOLDENTHRONE_TOOCOMFORTABLE.NAME", STRINGS.DUPLICANTS.STATUSITEMS.GOLDENTHRONE_TOOCOMFORTABLE.NAME);
				Strings.Add("STRINGS.DUPLICANTS.MODIFIERS.GOLDENTHRONE_TOOCOMFORTABLE.DESCRIPTION", STRINGS.DUPLICANTS.STATUSITEMS.GOLDENTHRONE_TOOCOMFORTABLE.TOOLTIP);
			}
		}
	}
}
