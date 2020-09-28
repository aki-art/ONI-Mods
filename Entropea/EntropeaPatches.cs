using FUtility;
using Harmony;
using ProcGen;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Entropea.Gen;
using static ProcGen.SettingsCache;
using Klei;
using Klei.CustomSettings;
using UnityEngine;
using System.Linq;

namespace Entropea
{
    class EntropeaPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
            }
        }
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                Buildings.RegisterSingleBuilding(typeof(TileConfig));
            }
        }
        [HarmonyPatch(typeof(Localization), "Initialize")]
        class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }


        [HarmonyPatch(typeof(Comet), "GetVolume")]
        public static class Comet_GetVolume_Patch
        {
            public static void Postfix(GameObject gameObject, ref float __result)
            {

                if (gameObject == null) return;
                if (gameObject.name.Contains(WorldTraits.MegaMeteorConfig.ID))
                {
                    __result *= 5f;
                    Debug.Log("increased volume");
                }
                //if (CameraController.Instance.IsAudibleSound(pos, sound))
            }
        }

        internal static SpawnFXHashes megaMeteorFX = (SpawnFXHashes)453;
        [HarmonyPatch(typeof(Game), "InitializeFXSpawners")]
        public static class Game_InitializeFXSpawners_Patch
        {
            public static void Prefix(ref SpawnPoolData[] ___fxSpawnData, Dictionary<int, Action<Vector3, float>> ___fxSpawner)
            {
                var spawnData = ___fxSpawnData.ToList();
                var oxy = spawnData.FirstOrDefault(d => d.id == SpawnFXHashes.MeteorImpactMetal);

                var newPrefab = UnityEngine.Object.Instantiate(oxy.fxPrefab);
                KBatchedAnimController component = newPrefab.GetComponent<KBatchedAnimController>();
                component.animScale *= 3;

                //component.AnimFiles[0] = Assets.GetAnim("carbolitefx_kanim");
                var carbolite = new SpawnPoolData(
                    (SpawnFXHashes)453,
                    oxy.initialCount,
                    Color.white,
                    newPrefab,
                    oxy.initialAnim,
                    oxy.spawnOffset,
                    oxy.spawnRandomOffset,
                    oxy.rotationConfig,
                    oxy.rotationData);

                spawnData.Add(carbolite);
                ___fxSpawnData = spawnData.ToArray();
            }
        }

        [Serializable]
        public struct SpawnPoolData
        {
            [HashedEnum]
            public SpawnFXHashes id;
            public int initialCount;
            public Color32 colour;
            public GameObject fxPrefab;
            public string initialAnim;
            public Vector3 spawnOffset;
            public Vector2 spawnRandomOffset;
            public SpawnRotationConfig rotationConfig;
            public SpawnRotationData[] rotationData;

            public SpawnPoolData(SpawnFXHashes id, int initialCount, Color32 colour, GameObject fxPrefab, string initialAnim, Vector3 spawnOffset, Vector2 spawnRandomOffset, SpawnRotationConfig rotationConfig, SpawnRotationData[] rotationData)
            {
                this.id = id;
                this.initialCount = initialCount;
                this.colour = colour;
                this.fxPrefab = fxPrefab;
                this.initialAnim = initialAnim;
                this.spawnOffset = spawnOffset;
                this.spawnRandomOffset = spawnRandomOffset;
                this.rotationConfig = rotationConfig;
                this.rotationData = rotationData;
            }
        }

        public enum SpawnRotationConfig
        {
            Normal,
            StringName
        }

        [Serializable]
        public struct SpawnRotationData
        {
            public string animName;
            public bool flip;
        }

    }
}
