using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Content.Buildings.ZiplinePost;

namespace ZipLine.Content.Tools
{
    public class ZipConnectorTool : DragTool
    {
        public static ZipConnectorTool Instance;

        [SerializeField]
        public float maxDistance;

        [SerializeField]
        public Color areaColour;

        [SerializeField]
        public Color invalidColor;

        public Vector3 origin;
        public Vector3 startPoint;
        public Vector3 snappedOrigin;
        public ZiplineAnchor originalAnchor;
        public float distance;
        private Vector3 endPoint = Vector3.zero;
        private int aCell;
        private int bCell;
        private bool verticalConnection;
        private bool visValid;

        [MyCmpAdd]
        private ZipConnectorHoverTextConfiguration hoverConfig;

        private LineRenderer ropeVisualizer;
        private BuildingDef anchorDef;
        private int lastCell = -1;
        private List<OffsetInfo> offsets;
        private bool ropePathValid = true;
        private int unDiggableIdx = -1;

        private List<Vector2I> GetOffsets()
        {
            var result = new List<Vector2I>();

            foreach (var info in offsets)
            {
                result.Add(info.position);
            }

            return result;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            hoverConfig.warning = BuildTool.Instance.GetComponent<HoverTextConfiguration>().HoverTextStyleSettings[1];

            Instance = this;
            interceptNumberKeysForPriority = true;

            offsets = new List<OffsetInfo>();
        }

        public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
        {
            colors = new HashSet<ToolMenu.CellColorData>();

            foreach (var offset in offsets)
            {
                var cell = Grid.PosToCell(offset.position);
#if DEBUG
                if(cell == Grid.PosToCell(startPoint))
                {
                    colors.Add(new ToolMenu.CellColorData(cell, new Color(0, 0, 1, 0.2f)));
                    continue;
                }
                else if (cell == Grid.PosToCell(endPoint))
                {
                    colors.Add(new ToolMenu.CellColorData(cell, new Color(0, 1, 0, 0.2f)));
                    continue;
                }
#endif
                colors.Add(new ToolMenu.CellColorData(cell, offset.valid ? areaColour : invalidColor));
            }
        }

        public static void DestroyInstance()
        {
            Instance = null;
        }

        public override void OnMouseMove(Vector3 cursor_pos)
        {
            if (ropeVisualizer == null)
            {
                return;
            }

            UpdateStartPoint(cursor_pos);

            var cell = Grid.PosToCell(cursor_pos);

            if (cell == lastCell)
            {
                return;
            }

            ropePathValid = true;
            hoverConfig.errors.Clear();

            Vector3 position;

            if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var go) && go.TryGetComponent(out ZiplineAnchor anchor))
            {
                // snap to existing post
                position = anchor.transform.position + ZiplineAnchor.verticalOffset;
                visualizer.gameObject.SetActive(false);
            }
            else
            {
                // Place building preview
                position = Grid.CellToPos(cell) + ZiplineAnchor.verticalOffset;
                visualizer.gameObject.SetActive(true);
            }

            SetRopeEndpoint(position);
            lastCell = cell;
            UpdateVis(cursor_pos);
            UpdatePathVisualizer(position);
        }

        private void UpdatePathVisualizer(Vector3 position)
        {
            offsets.Clear();

            var offsetCells = ProcGen.Util.GetLine(startPoint, position);

            foreach (var offset in offsetCells)
            {
                AddOffset(offset);
                AddOffset(new Vector2I(offset.x, offset.y - 1));
                AddOffset(new Vector2I(offset.x, offset.y - 2));
            }

            // the worldgen util stops one tile too short, so adding those after 
            var pos = Grid.PosToXY(position);
            AddOffset(pos);
            AddOffset(new Vector2I(pos.x, pos.y - 1));
            AddOffset(new Vector2I(pos.x, pos.y - 2));
        }

        private void SetRopeEndpoint(Vector3 position)
        {
            bCell = Grid.OffsetCell(Grid.PosToCell(position), 0, 2);
            //verticalConnection = Grid.PosToXY(position).X == Grid.CellToXY(aCell).X;

            ropeVisualizer.SetPosition(1, position);
        }

        private void AddOffset(Vector2I offset)
        {
            var cell = Grid.PosToCell(offset);

            var dist = Vector2.Distance(offset, startPoint);

            if (!CanRopePassThrough(cell) || dist > maxDistance)
            {
                offsets.Add(new OffsetInfo(offset, false, dist));
                ropePathValid = false;
            }
            else
            {
                offsets.Add(new OffsetInfo(offset, true, dist));
            }
        }

        private bool CanRopePassThrough(int cell)
        {
            if(verticalConnection)
            {
                //return false;
            }

            // allow track to pass through the foundation tiles of the anchors. while technically should block the path, it would be too limiting for gameplay
            if(cell == aCell || cell == bCell)
            {
                return true;
            }

            return Grid.IsValidBuildingCell(cell) &&
                !Grid.Foundation[cell] &&
                !Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].ContainsKey(cell) &&
                Grid.ElementIdx[cell] != unDiggableIdx;
        }

        private void SetColor(Color color)
        {
            ropeVisualizer.material.SetColor("_TintColor", color);
            visualizer.GetComponent<KBatchedAnimController>().TintColour = color;
        }

        public void ActivateTool(ZiplineAnchor originalAnchor, Vector3 origin, int foundationCell)
        {
            this.originalAnchor = originalAnchor;
            this.origin = origin;
            aCell = foundationCell;
            ActivateTool();
        }

        private void UpdateStartPoint(Vector3 cursorPosition)
        {
            startPoint = origin + ((cursorPosition.x > origin.x) ? ZiplineAnchor.horizontalOffset : -ZiplineAnchor.horizontalOffset);
            ropeVisualizer.SetPosition(0, startPoint);
        }

        protected override void OnActivateTool()
        {
            if (visualizer != null)
            {
                Destroy(visualizer);
            }

            unDiggableIdx = ElementLoader.GetElementIndex(SimHashes.Unobtanium);

            if (ropeVisualizer == null)
            {
                ropeVisualizer = Instantiate(ModAssets.ghostLinePrefab).GetComponent<LineRenderer>();
                anchorDef = Assets.GetBuildingDef(ZiplinePostConfig.ID);
            }

            var position = ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
            visualizer = GameUtil.KInstantiate(anchorDef.BuildingPreview, position, Grid.SceneLayer.Ore, "Zipline connector building preview", LayerMask.NameToLayer("Place"));

            if (visualizer.TryGetComponent(out KBatchedAnimController kbac))
            {
                kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
                kbac.isMovable = true;
                kbac.Offset = anchorDef.GetVisualizerOffset();
                kbac.name = kbac.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
            }

            UpdateVis(position);

            ropeVisualizer.SetPosition(0, startPoint);
            ropeVisualizer.gameObject.SetActive(true);

            PlayerController.Instance.ActivateTool(this);
            GridCompositor.Instance.ToggleMajor(true);

            snappedOrigin = Grid.CellToPosCBC(Grid.PosToCell(startPoint), anchorDef.SceneLayer);

            base.OnActivateTool();
        }

        private void UpdateVis(Vector3 pos)
        {
            distance = Vector3.Distance(pos, startPoint);
            distance = Mathf.Ceil(distance);
            hoverConfig.distance = distance;

            endPoint = pos;

            if (distance > maxDistance)
            {
                hoverConfig.errors.Add(STRINGS.UI.ZIPLINE.TOO_LONG.Replace("{distance}", GameUtil.GetFormattedDistance(maxDistance)));
                SetColor(Color.red);
            }

            visValid = anchorDef.IsValidBuildLocation(visualizer, pos, Orientation.Neutral, out var error);

            if (!visValid && !error.IsNullOrWhiteSpace())
            {
               hoverConfig.errors.Add(error);
            }

            if (visValid)
            {
                visValid = anchorDef.IsValidPlaceLocation(visualizer, pos, Orientation.Neutral, out var error2);

                if (!visValid && !error2.IsNullOrWhiteSpace())
                {
                   hoverConfig.errors.Add(error2);
                }
            }

            visValid = visValid && ropePathValid;

            if (visualizer != null)
            {
                SetColor(visValid ? Color.white : Color.red);
            }


            var cell = Grid.PosToCell(pos);
            if (anchorDef != null)
            {
                var snappedPosition = Grid.CellToPosCBC(cell, anchorDef.SceneLayer);
                visualizer.transform.SetPosition(snappedPosition);
                transform.SetPosition(snappedPosition - Vector3.up * 0.5f);

                if (lastCell != cell)
                {
                    lastCell = cell;
                }

                var angle = Vector3.Angle((snappedPosition + ZiplineAnchor.verticalOffset - startPoint).normalized, Vector3.down);
                verticalConnection = angle < Mod.Settings.MinimumAngleAllowed;
                hoverConfig.angle = angle;
            }

            if (!ropePathValid)
            {
               hoverConfig.errors.Add(STRINGS.UI.ZIPLINE.OBSTRUCTED);
            }

            if(verticalConnection)
            {
               hoverConfig.errors.Add(STRINGS.UI.ZIPLINE.VERTICAL);
            }
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);

            ropeVisualizer.gameObject.SetActive(false);

            Destroy(visualizer);
            GridCompositor.Instance.ToggleMajor(false);
            lastCell = -1;

            //SelectTool.Instance.Activate();
            ToolMenu.Instance.PriorityScreen.Show(false);
        }

        protected override Mode GetMode()
        {
            return Mode.Brush;
        }

        public override string GetDeactivateSound()
        {
            return "HUD_Click_Deselect";
        }

        public override void OnLeftClickDown(Vector3 position)
        {
            if (!visValid)
            {
                return;
            }

            var cell = Grid.PosToCell(position);

            if (TryGetAnchor(cell, out var anchor))
            {
                originalAnchor.AddConnection(anchor, GetOffsets());
            }
            else
            {
                PlaceBuilding(position, cell);
                PlayerController.Instance.ActivateTool(SelectTool.Instance);
            }
        }

        private static bool TryGetAnchor(int cell, out ZiplineAnchor anchor)
        {
            anchor = null;
            return Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var go) && go.TryGetComponent(out anchor);
        }

        private void PlaceBuilding(Vector3 position, int cell)
        {
            GameObject gameObject;
            var elements = originalAnchor.GetComponent<Deconstructable>().constructionElements;

            if (Helper.IsInstantBuilding())
            {
                if (CanPlaceAnchor(position))
                {
                    gameObject = anchorDef.Build(cell, Orientation.Neutral, null, elements, 293.15f, false, GameClock.Instance.GetTime());

                    if (gameObject.TryGetComponent(out ZiplineAnchor anchor))
                    {
                        originalAnchor.AddConnection(anchor, GetOffsets());
                    }
                }
            }
            else
            {
                gameObject = anchorDef.TryPlace(visualizer, position, Orientation.Neutral, elements);
                Helper.SetPriority(gameObject);

                if (anchorDef.MaterialsAvailable(elements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
                {
                    PlayPlaceSound(position);
                }
                else
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, position);
                }
            }

            Helper.DigCells(offsets);
        }

        private bool CanPlaceAnchor(Vector3 position)
        {
            return anchorDef.IsValidBuildLocation(visualizer, position, Orientation.Neutral) &&
                anchorDef.IsValidPlaceLocation(visualizer, position, Orientation.Neutral, out _);
        }

        private void PlayPlaceSound(Vector3 position)
        {
            placeSound = GlobalAssets.GetSound("Place_Building_" + anchorDef.AudioSize);
            if (placeSound != null)
            {
                var soundPos = new Vector3(position.x, position.y);
                var instance = SoundEvent.BeginOneShot(placeSound, soundPos);
                SoundEvent.EndOneShot(instance);
            }
        }

        public struct OffsetInfo
        {
            public Vector2I position;
            public bool valid;
            public float distance;

            public OffsetInfo(Vector2I position, bool valid, float distance)
            {
                this.position = position;
                this.valid = valid;
                this.distance = distance;
            }
        }

#if DEBUG
        void Update()
        {
            DebugExtension.DrawPoint(startPoint, Color.green, 0.2f);
            DebugExtension.DrawPoint(endPoint, Color.blue, 0.1f);
            Helper.DrawLine(startPoint, endPoint, Color.red);
        }
#endif
    }
}
