using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;

namespace PrintingPodRecharge.Patches
{
	public class ImmigrantScreenPatch
	{
		[HarmonyPatch(typeof(ImmigrantScreen), "OnPrefabInit")]
		public class ImmigrantScreen_OnPrefabInit_Patch
		{
			public static void Postfix(KButton ___rejectButton)
			{
				if (BioInksD6Manager.Instance.button == null)
				{
					var gameObject = Util.KInstantiate(___rejectButton.gameObject, ___rejectButton.transform.parent.gameObject);
					BioInksD6Manager.Instance.SetButton(gameObject);
				}
			}
		}

		/*		[HarmonyPatch(typeof(CarePackageContainer), "GenerateCharacter")]
				public class CarePackageContainer_GenerateCharacter_Patch
				{
					public static void Postfix(CarePackageContainer __instance)
					{
						__instance.StartCoroutine(TintCarePackageColorCoroutine(("Details/PortraitContainer/BG", __instance)));
					}
				}

				// need to wait just a little, or something goes wrong and the background will be offset and weird
				public static IEnumerator TintCarePackageColorCoroutine((string Path, KScreen Instance) args)
				{
					yield return new WaitForEndOfFrame();
					TintBG(args.Instance, args.Path);
				}*/
	}
}
