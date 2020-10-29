using Database;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TransparentAluminium
{
    class SpaceDestinationPatches
    {
        const int AQUA_ID = 351;

        [HarmonyPatch(typeof(SpacecraftManager), "OnSpawn")]
        public static class SpacecraftManager_OnSpawn_Patch
        {
            public static void Postfix()
            {
                if (!SpacecraftManager.instance.destinations.Any(d => d.id == AQUA_ID))
                {
                    Debug.Log("adding SpaceDestination");
                    SpaceDestination aqua = new SpaceDestination(AQUA_ID, Db.Get().SpaceDestinationTypes.ForestPlanet.Id, 1);
                    SpacecraftManager.instance.destinations.Add(aqua);
                }
            }
        }


        [HarmonyPatch(typeof(SpaceDestination), "GetDestinationType")]
        public static class SpaceDestination_GetDestinationType_Patch
        {
            public static void Postfix(SpaceDestination __instance, ref SpaceDestinationType __result)
            {
                if (__instance.id == AQUA_ID)
                {
                    __result = ModAssets.AquaPlanet;
                }
            }
        }



        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                var elements = new Dictionary<SimHashes, MathUtil.MinMax>
                {
                    { SimHashes.Aluminum, new MathUtil.MinMax(50f, 100f) },
                    { SimHashes.OxyRock, new MathUtil.MinMax(10f, 20f) },
                    { SimHashes.Water, new MathUtil.MinMax(100f, 200f) }
                };

                var entities = new Dictionary<string, int>
                {
                    { "Pacu", 2 }
                };

                var destination = new SpaceDestinationType(
                    id: "AquaPlanet",
                    parent: Db.Get().SpaceDestinationTypes,
                    name: "Test",
                    description: "Description",
                    iconSize: 16,
                    spriteName: "asteroid",
                    elementTable: elements,
                    recoverableEntities: entities,
                    artifactDropRate: Db.Get().ArtifactDropRates.Bad,
                    cycles: 18);

                ModAssets.AquaPlanet = Db.Get().SpaceDestinationTypes.Add(destination);
            }
        }
    }
}