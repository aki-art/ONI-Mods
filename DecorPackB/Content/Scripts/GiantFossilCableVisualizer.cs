using FUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackB.Content.Scripts
{
    public class GiantFossilCableVisualizer : KMonoBehaviour, ISim4000ms, IRender200ms
    {
        public const int MAX_CABLE_LENGTH = 16;
        public const int MAX_CABLE_COUNT = 8;
        public static readonly HashedString cableOriginMarker = "cableorigin_marker";

        [SerializeField] public GameObject linePrefab;
        [SerializeField] public bool updatePositionEveryFrame;
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
        public bool isHangable;
        public static bool isActivePreviewHangable;

        private Text debugText;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            kbac = transform.parent.GetComponent<KBatchedAnimController>();
            building = transform.parent.GetComponent<Building>();

            z = Grid.GetLayerZ(Grid.SceneLayer.Front);

            transform.parent.gameObject.Subscribe((int)ModHashes.FossilStageSet, _ => ArtStageChanged());
            ArtStageChanged();

            if(!updatePositionEveryFrame)
            {
                // only run for the build menu preview
                SimAndRenderScheduler.instance.render200ms.Remove(this);
            }

            if(updatePositionEveryFrame || !isHangable)
            {
                // runs for placed preview and actual building
                SimAndRenderScheduler.instance.sim4000ms.Remove(this);
            }

            SetCableColor(color);

            if(Mod.DebugMode)
            {
                debugText = Utils.AddText(transform.position + new Vector3(3, 2.5f), Color.yellow, "X");
            }
        }

        public bool IsGrounded()
        {
            return BuildingDef.CheckFoundation(Grid.PosToCell(this), building.Orientation, BuildLocationRule.OnFloor, building.Def.WidthInCells, building.Def.HeightInCells);
        }

        public bool IsHangable()
        {
            if(!isHangable)
            {
                return false;
            }

            for (int i = 0; i < startPoints.Count; i++)
            {
                var ceilingDistance = GetCeilingDistance(Mathf.FloorToInt(startPoints[i].x));

                if (ceilingDistance < 0 || ceilingDistance > MAX_CABLE_LENGTH)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetCableColor(Color color)
        {
            foreach (var cable in lineRenderers)
            {
                cable.startColor = cable.endColor = color;
            }
        }

        private float GetCeilingDistance(int x)
        {
            Grid.PosToXY(transform.position, out var xo, out var yo);
            for (var y = building.Def.HeightInCells; y < MAX_CABLE_LENGTH; y++)
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
                var frame = batch.group.data.GetAnimFrames()[0];
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

        private void SetupLines()
        {
            lineRenderers = new();

            for (int i = 0; i < startPoints.Count; i++)
            {
                var lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();
                lineRenderer.transform.position = transform.position;
                lineRenderer.transform.parent = transform;
                lineRenderer.name = $"cable {i}";
                lineRenderer.startColor = lineRenderer.endColor = updatePositionEveryFrame ? Color.white : Color.black;
                lineRenderer.gameObject.SetActive(true);
                lineRenderers.Add(lineRenderer);
            }
        }

        public void Draw()
        {
            if (previousPosition == transform.position)
            {
                return;
            }

            isActivePreviewHangable = true;

            for (int i = 0; i < startPoints.Count; i++)
            {
                var point = startPoints[i];
                var ceilingDistance = GetCeilingDistance(Mathf.FloorToInt(startPoints[i].x));

                if(debugText != null)
                {
                    debugText.text = ceilingDistance.ToString();
                }

                lineRenderers[i].SetPosition(0, point);
                lineRenderers[i].SetPosition(1, new Vector3(point.x, ceilingDistance, z));

                if(ceilingDistance < 0 || ceilingDistance > MAX_CABLE_LENGTH)
                {
                    isActivePreviewHangable = false;
                }
            }

            ToggleLines(isActivePreviewHangable);
            previousPosition = transform.position;
        }

        private void ToggleLines(bool enabled)
        {
            foreach(var lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = enabled;
            }
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
            isHangable = startPoints != null && startPoints.Count > 0;
            var go = transform.parent.gameObject;

            if(isHangable && GameComps.RequiresFoundations.Has(go))
            {
                GameComps.RequiresFoundations.Remove(go);
            }

            if (!isHangable)
            {
                if (!GameComps.RequiresFoundations.Has(go))
                {
                    GameComps.RequiresFoundations.Add(go);
                }

                return;
            }

            width = building.Def.WidthInCells;
            SetupLines();
            Draw();
        }

        public void Sim4000ms(float dt)
        {
            if(!updatePositionEveryFrame)
            {
                return;
            }

            Draw();
        }

        public void Render200ms(float dt)
        {
            Draw();
        }
    }
}
