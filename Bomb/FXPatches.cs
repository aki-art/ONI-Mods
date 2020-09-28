using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bomb
{
    class FXPatches
    {
        [HarmonyPatch(typeof(Game), "InitializeFXSpawners")]
        public static class Game_InitializeFXSpawners_Patch
        {
            public static void Prefix(ref SpawnPoolData[] ___fxSpawnData, Dictionary<int, Action<Vector3, float>> ___fxSpawner)
            {
                var spawnData = ___fxSpawnData.ToList();
                var meteorFX = spawnData.FirstOrDefault(d => d.id == SpawnFXHashes.MeteorImpactMetal);

                var megaMeteorImpact = CreateNew(Tuning.FXHashes.HugeExposion, meteorFX, 3f);

                spawnData.Add(megaMeteorImpact);

                ___fxSpawnData = spawnData.ToArray();
            }

            private static SpawnPoolData CreateNew(SpawnFXHashes hash, SpawnPoolData original, float scale = 1f, string newAnim = null)
            {
                return GetData(hash, original, GetNewPrefab(original.fxPrefab, scale, newAnim));
            }

            private static SpawnPoolData GetData(SpawnFXHashes hash, SpawnPoolData original, GameObject prefab)
            {
                return new SpawnPoolData(hash, original.initialCount, Color.white, prefab, original.initialAnim, 
                    original.spawnOffset, original.spawnRandomOffset, original.rotationConfig, original.rotationData);
            }

            private static GameObject GetNewPrefab(GameObject original, float scale = 1f, string newAnim = null)
            {
                GameObject prefab = UnityEngine.Object.Instantiate(original);
                KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
                component.animScale *= scale;
                if (!newAnim.IsNullOrWhiteSpace())
                {
                    component.AnimFiles[0] = Assets.GetAnim(newAnim);
                }

                return prefab;
            }
        }

        // Mirrors an identical struct in Game.cs
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

        // Mirrors an identical enum in Game.cs
        public enum SpawnRotationConfig
        {
            Normal,
            StringName
        }

        // Mirrors an identical struct in Game.cs
        [Serializable]
        public struct SpawnRotationData
        {
            public string animName;
            public bool flip;
        }

    }
}
