using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PrintingPodRecharge.Patches
{
    internal class ImmigrantScreenPatch
    {

        [HarmonyPatch(typeof(ImmigrantScreen), "Initialize")]
        public class ImmigrantScreen_OnPrefabInit_Patch
        {
            public static void Postfix(ImmigrantScreen __instance)
            {
                if(__instance != null)
                ListChildren(__instance.transform);
            }

        }

        public static void ListChildren(Transform parent, int level = 0, int maxDepth = 10)
        {
            if (level >= maxDepth)
                return;

            foreach (Transform child in parent)
            {
                if (child.GetComponent<KBatchedAnimController>() is KBatchedAnimController kbac)
                {
                    Console.WriteLine(string.Concat(Enumerable.Repeat(' ', level)) + "ˇHAS ANIM ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ");
                    Console.WriteLine(string.Concat(Enumerable.Repeat(' ', level)) + kbac.AnimFiles[0]?.name);


                }
                Console.WriteLine(string.Concat(Enumerable.Repeat('-', level)) + child.name);
                ListChildren(child, level + 1);
            }
        }

        private static void ReplaceSymbols(KBatchedAnimController kbac)
        {

        }

        [HarmonyPatch(typeof(CarePackageContainer), "OnSpawn")]
        public class CarePackageContainer_OnSpawn_Patch
        {
            public static void Postfix(CarePackageContainer __instance)
            {
                GameScheduler.Instance.ScheduleNextFrame("", obj => TintBG(__instance, "Details/PortraitContainer/BG"));
            }
        }

        [HarmonyPatch(typeof(CharacterContainer), "OnSpawn")]
        public class CharacterContainer_OnSpawn_Patch
        {
            public static void Postfix(CharacterContainer __instance)
            {
                GameScheduler.Instance.ScheduleNextFrame("", obj => TintBG(__instance, "Details/Top/PortraitContainer/BG"));
            }
        }

        private static void TintBG(KScreen __instance, string path)
        {
            if (!ImmigrationModifier.Instance.IsOverrideActive || !ImmigrationModifier.Instance.swapAnim)
            {
                return;
            }

            var animBg = __instance.transform.Find(path);
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
