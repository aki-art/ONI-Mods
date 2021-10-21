using FUtility.Helpers;
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

        public static void YeetRandomly(GameObject go, bool onlyUp, float minDistance, float maxDistance, bool rotate)
        {
            var vec = Random.insideUnitCircle.normalized;
            if (onlyUp)
                vec.y = Mathf.Abs(vec.y);
            vec += new Vector2(0f, Random.Range(0, 1f));
            vec *= Random.Range(minDistance, maxDistance);

            Yeet(go, minDistance, rotate, vec);
        }

        public static void YeetAtAngle(GameObject go, float angle, float distance, bool rotate)
        {
            var vec = DegreeToVector2(angle) * distance;
            Yeet(go, distance, rotate, vec);
        }

        private static void Yeet(GameObject go, float distance, bool rotate, Vector2 vec)
        {
            if (GameComps.Fallers.Has(go))
                GameComps.Fallers.Remove(go);

            GameComps.Fallers.Add(go, vec);

            if (rotate)
            {
                Rotator rotator = go.AddOrGet<Rotator>();
                rotator.minDistance = distance;
                rotator.SetVec(vec);
            }
        }

        public static Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        public static Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
