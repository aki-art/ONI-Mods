using FUtility;
using HarmonyLib;

namespace Randomizer.Patches
{
    internal class DestinationSelectPanelPatch
    {

        //[HarmonyPatch(typeof(DestinationSelectPanel), "SelectCluster")]
        public class DestinationSelectPanel_SelectCluster_Patch
        {
            public static bool Prefix(DestinationSelectPanel __instance, string name, int seed, ref ColonyDestinationAsteroidBeltData __result)
            {
                if(name.StartsWith("clusters/Randomizer_PlaceHolder"))
                {
                    __instance.selectedIndex = 0;
                    name = "clusters/Randomizer_PlaceHolderDefault";
                    __instance.asteroidData[name].ReInitialize(seed);
                    __result = __instance.asteroidData[name];

                    return false;
                }

                return true;
            }
        }
    }
}
