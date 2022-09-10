using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System.Collections;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class ImmigrantScreenPatch
    {

        [HarmonyPatch(typeof(ImmigrantScreen), "OnRejectionConfirmed")]
        public class ImmigrantScreen_OnRejectionConfirmed_Patch
        {
            public static void Postfix()
            {
                HairDye.rolledHairs.Clear();
            }
        }

        [HarmonyPatch(typeof(CarePackageContainer), "OnSpawn")]
        public class CarePackageContainer_OnSpawn_Patch
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
            if (!ImmigrationModifier.Instance.IsOverrideActive || !ImmigrationModifier.Instance.swapAnim)
            {
                return;
            }

            var animBg = __instance.transform.Find(path);

            if (animBg == null)
            {
                return;
            }

            var kbac = animBg.GetComponent<KBatchedAnimController>();

            kbac.SwapAnims(ImmigrationModifier.Instance.bgAnim);

            var bg = ImmigrationModifier.Instance.bgColor;
            var glow = ImmigrationModifier.Instance.glowColor;

            if (ImmigrationModifier.Instance.randomColor && HairDye.rolledHairs.TryGetValue((__instance as CharacterContainer).Stats, out var color))
            {
                bg = GetComplementaryColor(color, 1f);
                glow = GetComplementaryColor(color, 1.2f);
            }

            kbac.SetSymbolTint("forever", bg);
            kbac.SetSymbolTint("grid_bloom", glow);
            kbac.SetSymbolTint("inside_rough", glow);

            kbac.SetDirty();
            kbac.UpdateAnim(1);
            kbac.Play("crewSelect_bg", KAnim.PlayMode.Loop);
        }

        private static Color GetComplementaryColor(Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out _, out _);

            h = (h + 0.5f) % 1f; // invert hue
            var v = 0.75f; // bright
            var s = 0.55f; // not too saturated. against the blue of the window this looks vibrant enough

            return Color.HSVToRGB(h, s, v);
        }
    }
}
