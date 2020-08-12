using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationVolI.Buildings.MoodLamp
{
    class Patches
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<MoodLampSideScreen>(
                    "Mood Lamp Side Screen",
                    "MonumentSideScreen",
                    typeof(MonumentSideScreen));
            }
        }


        [HarmonyPatch(typeof(MonumentSideScreen), "OnSpawn")]
        public static class MonumentSideScreen_OnSpawn_Patch
        {
            public static void Postfix(RectTransform ___buttonContainer)
            {
                Debug.Log(___buttonContainer.name);
                Debug.Log(___buttonContainer.gameObject.name);
            }
        }

    }
}
