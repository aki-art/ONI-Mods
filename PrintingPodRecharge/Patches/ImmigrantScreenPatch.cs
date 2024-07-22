using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections;
using UnityEngine;

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

		[HarmonyPatch(typeof(CarePackageContainer), "GenerateCharacter")]
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
		}

		private static void TintBG(KScreen __instance, string path)
		{
			if (!(__instance is CharacterContainer character))
				return;

			var randoDupe = character?.Stats != null && CustomDupe.stats.Contains(character.Stats);

			if (!(ImmigrationModifier.Instance.IsOverrideActive || randoDupe))
				return;

			var activeBundle = randoDupe ? ImmigrationModifier.Instance.GetBundle(Bundle.Shaker) : ImmigrationModifier.Instance.GetActiveCarePackageBundle();

			if (activeBundle == null || !activeBundle.replaceAnim)
			{
				Log.Debug("Not replaceable anim for " + activeBundle?.bgAnim);
				return;
			}

			var animBg = __instance.transform.Find(path);

			if (animBg == null)
				return;

			var kbac = animBg.GetComponent<KBatchedAnimController>();

			if (kbac == null)
				return;

			if (activeBundle.bgAnim != null)
			{
				kbac.SwapAnims(activeBundle.bgAnim);
			}

			var bg = activeBundle.printerBgTint;
			var glow = activeBundle.printerBgTintGlow;

			if (ImmigrationModifier.Instance.randomColor || randoDupe)
			{
				var color = GetRandomHairColorLegacy();
				bg = GetComplementaryColor(color);
				glow = GetComplementaryColor(color);
			}

			kbac.SetSymbolTint("forever", bg);
			kbac.SetSymbolTint("grid_bloom", glow);
			kbac.SetSymbolTint("inside_rough", glow);

			kbac.SetDirty();
			kbac.UpdateAnim(1);
			kbac.Play("crewSelect_bg", KAnim.PlayMode.Loop);
		}

		private static Color GetRandomHairColorLegacy() => Random.ColorHSV(0, 1, 0f, 0.9f, 0.1f, 1f);

		private static Color GetComplementaryColor(Color color)
		{
			Color.RGBToHSV(color, out var h, out _, out _);

			h = (h + 0.5f) % 1f; // invert hue
			var s = 0.55f; // not too saturated. against the blue of the window this looks vibrant enough
			var v = 0.75f; // bright

			return Color.HSVToRGB(h, s, v);
		}
	}
}
