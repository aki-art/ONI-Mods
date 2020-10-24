using FUtility.Helpers;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FUtility
{
    public class Utils
    {
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary> Spawns one entity by tag.</summary>
        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            var prefab = global::Assets.GetPrefab(tag);
            if (prefab == null) return null;
            var go = GameUtil.KInstantiate(global::Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);
            return go;
        }

        /// <summary> Spawns one entity by tag. </summary>
        public static GameObject Spawn(Tag tag, GameObject atGO, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            return Spawn(tag, atGO.transform.position, sceneLayer, setActive);
        }

        /// <summary> Throws a gameObject in the air. </summary>
        public static void Yeet(GameObject go, bool onlyUp, float minDistance, float maxDistance, bool rotate)
        {
            var vec = Random.insideUnitCircle.normalized;
            if (onlyUp)
                vec.y = Mathf.Abs(vec.y);
            vec += new Vector2(0f, Random.Range(0, 1f));
            vec *= Random.Range(minDistance, maxDistance);

            if (GameComps.Fallers.Has(go))
                GameComps.Fallers.Remove(go);

            GameComps.Fallers.Add(go, vec);
            if(rotate)
            {
                Rotator rotator = go.AddOrGet<Rotator>();
                rotator.minDistance = minDistance;
                rotator.SetVec(vec);
            }
        }

        public static string GetNameForTag(Tag tag)
        {
            GameObject prefab = global::Assets.TryGetPrefab(tag);
            if (prefab != null)
                return prefab.GetProperName();
            else return null;
        }
    }
}
