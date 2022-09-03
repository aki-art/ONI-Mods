using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Buildings.ZiplinePost;

namespace ZipLine.Tools
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
        public ZiplineAnchor originalAnchor;
        public float distance;

        [MyCmpAdd]
        private ZipConnectorHoverTextConfiguration hoverConfig;

        private LineRenderer ropeVisualizer;
        private BuildingDef anchorDef;
        private int lastCell = -1;
        private List<OffsetInfo> offsets;
        private bool ropePathValid = true;
        private int unDiggableIdx = -1;


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

            var cell = Grid.PosToCell(cursor_pos);

            if (cell == lastCell)
            {
                return;
            }

            ropePathValid = true;
            hoverConfig.errorMessage = null;

            Vector3 position;

            if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var go) && go.TryGetComponent(out ZiplineAnchor anchor))
            {
                // snap to existing post
                position = anchor.transform.position + ZiplineAnchor.offset;
                visualizer.gameObject.SetActive(false);
            }
            else
            {
                // Place building preview
                position = Grid.CellToPos(cell) + ZiplineAnchor.offset;
                visualizer.gameObject.SetActive(true);
            }

            SetRopeEndpoint(position);
            UpdatePathVisualizer(position);
            lastCell = cell;
            UpdateVis(cursor_pos);
        }

        private void UpdatePathVisualizer(Vector3 position)
        {
            offsets.Clear();

            var offsetCells = ProcGen.Util.GetLine(origin, position);

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
            ropeVisualizer.SetPosition(1, position);
        }

        private void AddOffset(Vector2I offset)
        {
            var cell = Grid.PosToCell(offset);

            var dist = Vector2.Distance(offset, origin);

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

        public void ActivateTool(ZiplineAnchor originalAnchor, Vector3 origin)
        {
            this.originalAnchor = originalAnchor;
            this.origin = origin;
            ActivateTool();
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

            ropeVisualizer.SetPosition(0, origin);
            ropeVisualizer.gameObject.SetActive(true);

            PlayerController.Instance.ActivateTool(this);
            GridCompositor.Instance.ToggleMajor(true);

            base.OnActivateTool();
        }


        private void UpdateVis(Vector3 pos)
        {
            distance = Vector3.Distance(pos, origin);
            distance = Mathf.Floor(distance);
            hoverConfig.distance = distance;

            if (distance > maxDistance)
            {
                hoverConfig.errorMessage = STRINGS.UI.ZIPLINE.TOO_LONG.Replace("{distance}", GameUtil.GetFormattedDistance(maxDistance));
                SetColor(Color.red);
            }
            else
            {
                var visValid = anchorDef.IsValidBuildLocation(visualizer, pos, Orientation.Neutral, out string error);

                if (!visValid && !error.IsNullOrWhiteSpace())
                {
                    hoverConfig.errorMessage = error;
                }

                if (visValid)
                {
                    visValid = anchorDef.IsValidPlaceLocation(visualizer, pos, Orientation.Neutral, out string error2);

                    if (!visValid && !error2.IsNullOrWhiteSpace())
                    {
                        hoverConfig.errorMessage = error2;
                    }
                }

                visValid = visValid && ropePathValid;

                if (visualizer != null)
                {
                    SetColor(visValid ? Color.white : Color.red);
                }
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
            }

            if (!ropePathValid)
            {
                hoverConfig.errorMessage += "\n";
                hoverConfig.errorMessage += STRINGS.UI.ZIPLINE.OBSTRUCTED;
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

        protected override Mode GetMode() => Mode.Brush;

        public override string GetDeactivateSound() => "HUD_Click_Deselect";

        public override void OnLeftClickDown(Vector3 position)
        {
            if (!ropePathValid)
            {
                return;
            }

            var cell = Grid.PosToCell(position);

            if (TryGetAnchor(cell, out var anchor))
            {
                originalAnchor.AddConnection(anchor);
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
                    
                    if(gameObject.TryGetComponent(out ZiplineAnchor anchor))
                    {
                        originalAnchor.AddConnection(anchor);
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
    }
}
