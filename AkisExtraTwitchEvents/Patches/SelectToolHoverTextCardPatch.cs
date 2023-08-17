using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class SelectToolHoverTextCardPatch
	{
		[HarmonyPatch(typeof(Element))]
		[HarmonyPatch(nameof(Element.nameUpperCase), MethodType.Getter)]
		public class Element_nameUpperCase_Getter_Patch
		{
			public static void Postfix(Element __instance, ref string __result)
			{
				if (AkisTwitchEvents.Instance == null || !AkisTwitchEvents.Instance.hotTubActive)
					return;

				if (!__instance.IsSolid)
					__result = global::STRINGS.ELEMENTS.MAGMA.NAME.ToString().ToUpperInvariant();
			}
		}
	}
}
