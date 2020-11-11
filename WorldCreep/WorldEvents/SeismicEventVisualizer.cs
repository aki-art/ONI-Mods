using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class SeismicEventVisualizer : KMonoBehaviour
    {
        [MyCmpReq] WorldEvent worldEvent;
        List<Circle> circles = new List<Circle>();
        public GameObject visualizer;
        public int redBandDistance;
        public int yellowbandDistance;
        int resolution = 1;

        public bool Visible { get; private set; } = false;

        public void Show()
        {
            if(!Visible)
            {
                Visible = true;
                visualizer.SetActive(true);
                StartCoroutine(RefreshResolution());
            }
        }

        public void Stop()
        {
            Visible = false;
            visualizer.SetActive(false);
            StopCoroutine(RefreshResolution());
        }

        void UpdateResolution()
        {
            foreach(var circle in circles)
            {
                if (circle.alwaysShow || circle.index % resolution == 0)
                {
                    circle.Show();
                    circle.ThicknessMultiplier = Mathf.Pow(1.5f, (float)resolution);
                }
                else circle.Hide();
            }
        }

        IEnumerator RefreshResolution()
        {
            while(worldEvent != null && worldEvent.Stage != WorldEvent.WorldEventStage.Finished)
            {
                switch(CameraController.Instance.targetOrthographicSize)
                {
                    case float n when n <= 10:
                        resolution = 1;
                        break;
                    case float n when n <= 25:
                        resolution = 2;
                        break;
                    case float n when n <= 125:
                        resolution = 3;
                        break;
                    default:
                        resolution = 4;
                        break;
                }

                UpdateResolution();
                yield return new WaitForSecondsRealtime(0.33f);
            }

            yield return null;
        }


        protected override void OnSpawn()
        {
            visualizer = new GameObject();

            visualizer.transform.position = new Vector3(transform.position.x, transform.position.y, SimDebugView.Instance.transform.position.z - 0.1f);
            visualizer.transform.SetParent(transform);
            float thin = (float)resolution * 0.06f;
            float thick = (float)resolution * 0.2f;

            for (int radius = 0; radius <= worldEvent.radius; radius += resolution)
            {
                if (radius == redBandDistance || radius == yellowbandDistance || radius == worldEvent.radius)
                {
                    continue;
                }

                circles.Add(new Circle(thin, radius, Color.grey, visualizer.transform, radius % 4 + 1, false));
            }

            circles.Add(new Circle(thick, redBandDistance, Color.red, visualizer.transform, 5));
            circles.Add(new Circle(thick, yellowbandDistance, Color.yellow, visualizer.transform, 5));
            circles.Add(new Circle(thick, worldEvent.radius, Color.white, visualizer.transform, 5));

            visualizer.SetLayerRecursively(LayerMask.NameToLayer(nameof(SimDebugView)));
            visualizer.SetActive(false);
        }

        private class Circle
        {
            private readonly float thickness;
            private readonly float radius;
            private readonly Color color;
            private readonly GameObject gameObject;
            private readonly LineRenderer lineRenderer;
            public readonly int index;
            private float thicknessMultiplier;
            public float ThicknessMultiplier
            {
                get => thicknessMultiplier;
                set
                {
                    lineRenderer.widthMultiplier = value;
                    thicknessMultiplier = value;
                }
            }

            public readonly bool alwaysShow;

            public void Hide() => gameObject.SetActive(false);
            public void Show() => gameObject.SetActive(true);

            public Circle(float thickness, int radius, Color color, Transform parent, int index, bool alwaysShow = true)
            {
                this.index = index;

                this.thickness = thickness;
                this.radius = radius;
                this.color = color;
                this.alwaysShow = alwaysShow;

                gameObject = new GameObject();
                gameObject.transform.position = parent.position;
                gameObject.transform.SetParent(parent);

                lineRenderer = Draw();

                ThicknessMultiplier = 1f;

                gameObject.SetActive(true);
            }

            public LineRenderer Draw()
            {
                var segments = 360;
                var line = gameObject.AddComponent<LineRenderer>();
                line.useWorldSpace = false;
                line.startWidth = thickness;
                line.endWidth = thickness;
                line.positionCount = segments;
                line.loop = true;
                line.material = new Material(Shader.Find("Klei/Biome/Unlit Transparent"));
                line.startColor = color;
                line.endColor = color;

                var points = new Vector3[line.positionCount];

                for (int i = 0; i < line.positionCount; i++)
                {
                    var rad = Mathf.Deg2Rad * (i * 360f / segments);
                    points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
                }

                line.SetPositions(points);
                return line;
            }
        }
    }
}
