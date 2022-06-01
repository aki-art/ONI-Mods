using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class ImmigrantScreenPatch
    {
        [HarmonyPatch(typeof(CarePackageContainer), "OnSpawn")]
        public class CarePackageContainer_OnSpawn_Patch
        {
            public static void Postfix(CarePackageContainer __instance)
            {
                __instance.StartCoroutine(TintCarePackageColorCoroutine(("Details/PortraitContainer/BG", __instance)));
            }
        }

        [HarmonyPatch(typeof(CharacterContainer), "OnSpawn")]
        public class CharacterContainer_OnSpawn_Patch
        {
            public static void Postfix(CharacterContainer __instance)
            {
                __instance.StartCoroutine(TintCarePackageColorCoroutine(("Details/Top/PortraitContainer/BG", __instance)));
            }
        }

        private static IEnumerator TintCarePackageColorCoroutine((string Path, KScreen Instance) args)
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
                Log.Debuglog("ANIM BG IS NULL");

                FUtility.FUI.Helper.ListChildren(__instance.transform);
                return;
            }

            var kbac = animBg.GetComponent<KBatchedAnimController>();

            kbac.SwapAnims(new KAnimFile[] { Assets.GetAnim("rpp_greyscale_dupeselect_kanim") });

            kbac.SetSymbolTint("forever", ImmigrationModifier.Instance.bgColor);
            kbac.SetSymbolTint("grid_bloom", ImmigrationModifier.Instance.glowColor);
            kbac.SetSymbolTint("inside_rough", ImmigrationModifier.Instance.glowColor);

            kbac.SetDirty();
            kbac.UpdateAnim(1);
            kbac.Play("crewSelect_bg", KAnim.PlayMode.Loop);
        }
    }
}
