using FUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
    public class GiantFossilCableVisualizer : KMonoBehaviour, ISim4000ms
    {
        public const int MAX_CABLE_LENGTH = 32;
        public const int MAX_CABLE_COUNT = 8;
        public static readonly HashedString cableOriginMarker = "cableorigin_marker";

        [SerializeField] public GameObject linePrefab;
        [SerializeField] public bool updatePosition;
        [SerializeField] public Color color;
        [SerializeField] public string anim;

        private Building building;
        private KBatchedAnimController kbac;

        public List<Vector3> startPoints;
        public List<LineRenderer> lineRenderers;
        public int width;
        public Vector3 previousPosition;

        public List<CellOffset> monitorCells;
        private List<HandleVector<int>.Handle> partitionerEntries = new();
        public float z;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            kbac = transform.parent.GetComponent<KBatchedAnimController>();
            building = transform.parent.GetComponent<Building>();

            z = Grid.GetLayerZ(Grid.SceneLayer.Front);

            transform.parent.gameObject.Subscribe((int)ModHashes.FossilStageSet, _ => ArtStageChanged());
            ArtStageChanged();

            if (updatePosition)
            {
                StartCoroutine(UpdateCables());
            }

            SetCableColor(color);
        }

        private void SetCableColor(Color color)
        {
            foreach(var cable in lineRenderers)
            {
                cable.startColor = cable.endColor = color;
            }
        }

        private float GetCeilingDistance(int x)
        {
            Grid.PosToXY(transform.position, out var xo, out var  yo);
            for (var y = 0; y < MAX_CABLE_LENGTH; y++)
            {
                var cell = Grid.XYToCell(x + xo, y + yo);
                if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell))
                {
                    return y;
                }
            }

            return -1;
        }

        public void CollectMarkers()
        {
            startPoints = new List<Vector3>();

            var batch = kbac.GetBatch();

            if (batch != null)
            {
                var frame = batch.group.data.GetFrame(0);
                if (frame != KAnim.Anim.Frame.InvalidFrame)
                {
                    for (var frameElementIdx = 0; frameElementIdx < frame.numElements; ++frameElementIdx)
                    {
                        var offsetIdx = frame.firstElementIdx + frameElementIdx;
                        if (offsetIdx < batch.group.data.frameElements.Count)
                        {
                            var frameElement = batch.group.data.frameElements[offsetIdx];
                            if (frameElement.symbol == cableOriginMarker)
                            {
                                var vector3 = (kbac.GetTransformMatrix() * frameElement.transform)
                                    .MultiplyPoint(Vector3.zero) - transform.GetPosition();

                                startPoints.Add(vector3 with { z = z });
                            }
                        }
                    }
                }
            }
        }

        private void SetupLines()
        {
            lineRenderers = new();

            for (int i = 0; i < startPoints.Count; i++)
            {
                var lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();
                lineRenderer.transform.position = transform.position;
                lineRenderer.transform.parent = transform;
                lineRenderer.name = $"cable {i}";
                lineRenderer.startColor = lineRenderer.endColor = updatePosition ? Color.white : Color.black;
                lineRenderer.gameObject.SetActive(true);
                lineRenderers.Add(lineRenderer);
            }
        }

        private IEnumerator UpdateCables()
        {
            while (true)
            {
                Draw();
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < startPoints.Count; i++)
            {
                var point = startPoints[i];
                var ceilingDistance = GetCeilingDistance(Mathf.FloorToInt(startPoints[i].x));

                lineRenderers[i].SetPosition(0, point);
                lineRenderers[i].SetPosition(1, new Vector3(point.x, ceilingDistance, z));
            }

            previousPosition = transform.position;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            DestroyCables();
        }

        private void DestroyCables()
        {
            if (lineRenderers != null)
            {
                for (int i = 0; i < lineRenderers.Count; i++)
                {
                    Destroy(lineRenderers[i]);
                }
            }
        }

        public void ArtStageChanged()
        {
            DestroyCables();
            CollectMarkers();
            kbac.SetSymbolVisiblity(cableOriginMarker, false);

            // not hangable
            if (startPoints == null || startPoints.Count == 0)
            {
                return;
            }

            width = building.Def.WidthInCells;
            SetupLines();
            Draw();
        }

        public void Sim4000ms(float dt)
        {
            Draw();
        }
    }
}
