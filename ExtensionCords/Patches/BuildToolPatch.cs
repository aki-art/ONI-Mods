using HarmonyLib;

namespace ExtensionCords.Patches
{
    public class BuildToolPatch
    {
        [HarmonyPatch(typeof(BuildTool), "OnPrefabInit")]
        public class BuildTool_OnPrefabInit_Patch
        {
            public static void Postfix(BuildTool __instance)
            {
                __instance.gameObject.AddComponent<ReelTool>();
            }
        }
    }
}
