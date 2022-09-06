using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Content.Buildings.ZiplinePost;
using ZipLine.Content.Cmps;

namespace ZipLine.Content.Entities
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Rope : KMonoBehaviour
    {
        public Tag fiber = BasicFabricConfig.ID;

        [MyCmpReq]
        private LineRenderer lineRenderer;

        [MyCmpReq]
        private Storage storage;

        [MyCmpReq]
        private KSelectable kSelectable;

        [MyCmpReq]
        private OccupyArea occupyArea;

        [Serialize]
        private Ref<ZiplineAnchor> aRef;

        [Serialize]
        private Ref<ZiplineAnchor> bRef;

        private FetchChore chore;

        [SerializeField]
        public ZiplineAnchor a;

        [SerializeField]
        public ZiplineAnchor b;

        [SerializeField]
        public Vector3 offset;

        [Serialize]
        public bool complete;

        public int distance;

        public void Complete()
        {
            if (a == null || b == null)
            {
                Log.Warning("Tried to complete a zipline, but it isn't actually attached to 2 anchors.");
                return;
            }

            complete = true;

            lineRenderer.material.SetTexture("_MainTex", ModAssets.ropeTex);

            var startPos = a.transform.position + offset;
            var endPos = b.transform.position + offset;

            ApplyBiomeTints(Grid.PosToCell(startPos), Grid.PosToCell(endPos));

            // TODO: Add navigation

            kSelectable.AddStatusItem(ModDb.StatusItems.ziplineConnected);
        }

        public void SetArea(List<Vector2I> cells)
        {
            if (cells == null)
            {
                Log.Warning("Tried to set zipline offsets with no actual cells defined.");
                return;
            }

            var pos = Grid.CellToXY(Grid.PosToCell(this));

            if (occupyArea == null)
            {
                Log.Warning("wtf");
                return;
            }

            occupyArea.OccupiedCellsOffsets = new CellOffset[cells.Count];
            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i] - pos;
                occupyArea.OccupiedCellsOffsets[i] = new CellOffset(cell.X, cell.Y);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (aRef != null && bRef != null)
            {
                a = aRef.Get();
                b = bRef.Get();
            }

            if (a == null || b == null)
            {
                Log.Warning("Rope spawned with no anchors.");
                Destroy(this);
                return;
            }

            a.AddConnection(b, true);

            UpdateEndpoints();

            if(!complete)
            {
                CreateFetchChore();
                Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
            }
            else
            {
                Complete();
            }
        }

        private float HowMuchFiber => Mathf.Ceil(distance * Mod.Settings.FiberPerMeterCost);

        private void OnStorageChanged(object _)
        {
            if(complete)
            {
                return;
            }

            if(storage.GetMassAvailable(fiber) >= HowMuchFiber)
            {
                Complete();
            }
        }

        private void CreateFetchChore()
        {
            Log.Debuglog("created fetch chore");
            var tag = new []
            {
                fiber
            };

            chore = new FetchChore(Db.Get().ChoreTypes.BuildFetch, storage, HowMuchFiber, tag);
        }

        public void CancelFetchTask()
        {
            chore.Cancel("Cancel Rope");
            chore = null;
        }

        private void UpdateEndpoints()
        {
            kSelectable.AddStatusItem(ModDb.StatusItems.distance, this);

            var x = Math.Min(a.transform.position.x, b.transform.position.x);
            var y = Math.Min(a.transform.position.y, b.transform.position.y);

            transform.position = new Vector3(x, y);

            var startPos = a.transform.position + offset;
            var endPos = b.transform.position + offset;

            distance = Mathf.CeilToInt(Vector3.Distance(startPos, endPos));

            lineRenderer.SetPositions(new[]
            {
                startPos,
                endPos
            });

            var thickness = lineRenderer.startWidth / 2;
            var difference = Vector3.up * thickness * 5f;


            occupyArea.objectLayers = new[]
            {
                ObjectLayer.FoundationTile
            };

            var cell = Grid.PosToCell(this);
            foreach (var offset in occupyArea.OccupiedCellsOffsets)
            {
                var key = Grid.OffsetCell(cell, offset);
                Grid.ObjectLayers[(int)ObjectLayer.PlasticTile][key] = gameObject;
            }

            var collider = GetComponent<KPolygonCollider2D>();
            var pos = transform.position;

            collider.SetPoints(new[]
            {
                (Vector2)(lineRenderer.GetPosition(0) + difference - pos),
                (Vector2)(lineRenderer.GetPosition(0) - difference - pos),
                (Vector2)(lineRenderer.GetPosition(1) - difference - pos),
                (Vector2)(lineRenderer.GetPosition(1) + difference - pos)
            });

            collider.MarkDirty(true);

#if false
            var go2 = Instantiate(ModAssets.ghostLinePrefab);
            go2.SetActive(true);

            var lineRenderer1 = go2.GetComponent<LineRenderer>();
            lineRenderer1.positionCount = 4;
            go2.GetComponent<LineRenderer>().SetPositions(new[]
            {
                (Vector3)collider.Points[0] + pos,
                (Vector3)collider.Points[1] + pos,
                (Vector3)collider.Points[2] + pos,
                (Vector3)collider.Points[3] + pos
            });
            lineRenderer1.loop = true;
            lineRenderer1.material.SetColor("_TintColor", new Color(1, 0.8f, 0, 0.15f));
#endif
        }

        // make the two ends of the rope take on the biome tints
        private void ApplyBiomeTints(int start, int end)
        {
            if (!Grid.IsValidCell(start) || !Grid.IsValidCell(end))
            {
                return;
            }

            var zoneRenderData = World.Instance.zoneRenderData;

            var startZone = zoneRenderData.GetSubWorldZoneType(start);
            var endZone = zoneRenderData.GetSubWorldZoneType(end);

            var startColor = zoneRenderData.zoneColours[(int)startZone];
            startColor = Helper.Desaturate(startColor, 0.3f);

            var endColor = zoneRenderData.zoneColours[(int)endZone];
            endColor = Helper.Desaturate(endColor, 0.3f);

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }
}
