using UnityEngine;
using UnityEngine.UI;

namespace DecorPackB
{
    public class ModAssets
    {
        public static class Prefabs
        {
            public static GameObject circleMarker;
            public static GameObject sparkleCircle;
            public static GameObject cablePrefab;
        }

        private const float SPARKLE_DENSITY = 1000f / (5f * 5f);

        public static void PostDbInit()
        {
            Prefabs.circleMarker = CreateCircleMarker(0.05f, 10f);

            var bundle = FUtility.Assets.LoadAssetBundle("decorpackb");
            LoadParticles(bundle);
            LoadCable(bundle);
        }

        private static void LoadCable(AssetBundle bundle)
        {
            Prefabs.cablePrefab = bundle.LoadAsset<GameObject>("Assets/DecorPackB/cablePrefab.prefab");
            var renderer = Prefabs.cablePrefab.GetComponent<LineRenderer>();
            renderer.material = new Material(Shader.Find("Klei/FallingWater"))
            {
                renderQueue = RenderQueues.Liquid,
                mainTexture = Texture2D.whiteTexture
            };

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
            var segmentCount = 360 + 1;

            var go = new GameObject("DecorpackB_Circlemarker");

            go.SetActive(true);

            var lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
            lineRenderer.startColor = lineRenderer.endColor = Color.cyan;

            lineRenderer.material = new Material(Shader.Find("Klei/BloomedParticleShader"))
            {
                renderQueue = RenderQueues.Liquid
            };

            var points = new Vector3[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segmentCount);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            lineRenderer.positionCount = segmentCount;
            lineRenderer.SetPositions(points);

            go.SetActive(false);

            return go;
		}

		public static Text AddText(Vector3 position, Color color, string msg)
		{
			var gameObject = new GameObject();

			var rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(position);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);

			var text2 = gameObject.AddComponent<Text>();
			text2.font = global::Assets.DebugFont;
			text2.text = msg;
			text2.color = color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;

			return text2;
		}
	}
}
