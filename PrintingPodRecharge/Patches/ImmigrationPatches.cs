using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    public class ImmigrationPatches
    {

        [HarmonyPatch(typeof(Immigration), "OnPrefabInit")]
        public class Immigration_OnPrefabInit_Patch
        {
            public static void Postfix(Immigration __instance)
            {
                __instance.gameObject.AddComponent<ImmigrationModifier>();
            }
        }

        [HarmonyPatch(typeof(Immigration), "RandomCarePackage")]
        public class Immigration_RandomCarePackage_Patch
        {
            public static void Postfix(Immigration __instance, ref CarePackageInfo __result)
            {
                if(ImmigrationModifier.Instance.IsOverrideActive)
                {
                    __result = ImmigrationModifier.Instance.GetRandomPackage();
                }
            }   
        }


        [HarmonyPatch(typeof(Immigration), "EndImmigration")]
        public class Immigration_EndImmigration_Patch
        {
            public static void Postfix()
            {
                ImmigrationModifier.Instance.IsOverrideActive = false;
            }
        }
    }
}
