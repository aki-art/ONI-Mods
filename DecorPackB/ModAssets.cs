using FUtility;
using System;
using UnityEngine;
using static STRINGS.ELEMENTS;
using static UnityEngine.ParticleSystem;

namespace DecorPackB
{
    public class ModAssets
    {
        public static class Prefabs
        {
            public static GameObject circleMarker;
            public static GameObject sparkleCircle;
        }

        private const float SPARKLE_DENSITY = 1000f / (5f * 5f);

        public static void PostDbInit()
        {
            Prefabs.circleMarker = CreateCircleMarker(0.05f, 10f);

            var bundle = FUtility.Assets.LoadAssetBundle("decorpackb");
            LoadParticles(bundle);
        }

        private static void LoadParticles(AssetBundle bundle)
        {
            var prefab = bundle.LoadAsset<GameObject>("Assets/bioinks/CircleofSparkle.prefab");
            prefab.SetLayerRecursively(Game.PickupableLayer);
            prefab.SetActive(false);

            var texture = bundle.LoadAsset<Texture2D>("Assets/bioinks/simple_sphere.png");

            var material = new Material(Shader.Find("Klei/BloomedParticleShader"))
            {
                mainTexture = texture,
                renderQueue = RenderQueues.Liquid
            };

            var renderer = prefab.GetComponent<ParticleSystemRenderer>();
            renderer.material = material;

            Prefabs.sparkleCircle = prefab;
        }

        public static void ConfigureSparkleCircle(GameObject sparkles, float radius, Color color)
        {
            if(sparkles.TryGetComponent(out ParticleSystem particles))
            {
                var scale = particles.shape.scale;
                scale.x = radius;
                scale.y = radius;

                var emission = particles.emission;
                emission.rateOverTimeMultiplier = (float)(radius * radius) * SPARKLE_DENSITY;

                var main = particles.main;
                main.startColor = color;
            }
        }

        private static GameObject CreateCircleMarker(float lineWidth, float radius)
        {
            Log.Debuglog("creating circle marker");

            var segmentCount = 360 + 1;

            var go = new GameObject("DecorpackB_Circlemarker");

            go.SetActive(true);

            var lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
            lineRenderer.startColor = lineRenderer.endColor = Color.cyan;

            Log.Debuglog("created circle marker");
            lineRenderer.material = new Material(Shader.Find("Klei/BloomedParticleShader"))
            {
                renderQueue = RenderQueues.Liquid
            };

            Log.Assert("lineRenderer.material", lineRenderer.material);
            var points = new Vector3[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segmentCount);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            lineRenderer.positionCount = segmentCount;
            lineRenderer.SetPositions(points);

            Log.Assert("go", go);

            go.SetActive(false);

            return go;
        }
    }
}
