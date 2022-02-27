using Database;
using HarmonyLib;
using System;

namespace Beached.Patches.Germs
{
    internal class DiseasesPatch
    {

        [HarmonyPatch(typeof(Diseases), MethodType.Constructor, new Type[] { typeof(ResourceSet), typeof(bool) })]
        public class Diseases_Constructor_Patch
        {
            public static void Prefix()
            {
            }

            public static void Postfix(Diseases __instance, bool statsOnly)
            {
                ModAssets.Diseases.Register(__instance, statsOnly);
            }
        }
    }
}
