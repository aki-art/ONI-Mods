/*using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpookyPumpkinSO.Integration.MorePlants
{
    public class EntityConfigManagerPatch
    {
        public static void Patch(Harmony harmony)
        {
            var prefix = typeof(EntityConfigManagerPatch).GetMethod("Prefix", BindingFlags.NonPublic | BindingFlags.Static);
            var original = typeof(EntityConfigManager).GetMethod("LoadGeneratedEntities", new[] { typeof(List<Type>) });

            harmony.Patch(original, new HarmonyMethod(prefix));
        }

        private static void Prefix()
        {
            var config = new DecorPumpkinPlantConfig();

            var kPrefabID = config.CreatePrefab().GetComponent<KPrefabID>();
            kPrefabID.prefabInitFn += config.OnPrefabInit;
            kPrefabID.prefabSpawnFn += config.OnSpawn;

            Assets.AddPrefab(kPrefabID);
        }
    }
}
*/