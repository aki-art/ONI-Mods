using Harmony;
using System.Collections.Generic;
using System.Linq;

namespace Entropea
{
    class AsteroidSelectionPatches
    {
        [HarmonyPatch(typeof(ColonyDestinationAsteroidData), "GenerateParamDescriptors")]
        class ColonyDestinationAsteroidData_GenerateParamDescriptors_Patch
        {
            public static void Postfix(ColonyDestinationAsteroidData __instance, ref IEnumerable<AsteroidDescriptor> __result)
            {
                if (__instance.worldPath.Contains("worlds/Entropea"))
                {
                    List<AsteroidDescriptor> newDescriptors = new List<AsteroidDescriptor>(__result);
                    var text = string.Format(global::STRINGS.WORLDS.SURVIVAL_CHANCE.TITLE, STRINGS.WORLDS.SURVIVAL_CHANCE.UNKNOWN, "8F8F8F");
                    newDescriptors[newDescriptors.Count - 1] = new AsteroidDescriptor(text, null);
                    __result = newDescriptors;
                }
            }
        }
    }
}
