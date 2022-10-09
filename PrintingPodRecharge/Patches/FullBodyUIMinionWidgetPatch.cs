using HarmonyLib;
using PrintingPodRecharge.Cmps;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class FullBodyUIMinionWidgetPatch
    {
        // dyes full body dupes to have their proper hair color. such as the skill screen dupes or those in the clothing station
        [HarmonyPatch(typeof(FullBodyUIMinionWidget), "UpdateClothingOverride", typeof(SymbolOverrideController), typeof(MinionIdentity), typeof(StoredMinionIdentity))]
        public class FullBodyUIMinionWidget_UpdateClothingOverride_Patch
        {
            public static void Postfix(FullBodyUIMinionWidget __instance, MinionIdentity identity)
            {
                if (__instance.animController == null || !(__instance.animController is KBatchedAnimController) || !__instance.animController.enabled)
                {
                    return;
                }

                // possibly not compatible with future mods that also try to dye hair.
                // but that will be dealt with when it's neccessary.
                var color = identity.TryGetComponent(out CustomDupe dye) && dye.dyedHair ? dye.hairColor : Color.white;
                CustomDupe.TintHair(__instance.animController, color, true);
            }
        }
    }
}
