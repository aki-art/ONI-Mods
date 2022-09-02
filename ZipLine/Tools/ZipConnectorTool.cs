using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Buildings.ZiplinePost;

namespace ZipLine.Tools
{
    public class ZipConnectorTool : DragTool
    {
        public static ZipConnectorTool Instance;
        public Vector3 origin;
        private LineRenderer lineVis;
        private BuildingDef def;
        private int lastCell = -1;
        private List<OffsetInfo> offsets;
        private bool ropePathValid = true;
        public ZiplineAnchor originalAnchor;
        public float distance;

        [SerializeField]
        public float maxDistance;

        [MyCmpAdd]
        private ZipConnectorHoverTextConfiguration hoverConfig;

        private Color areaColour = new Color(1, 1, 1, 0.05f);
        private Color invalidColor = new Color(1, 0, 0, 0.2f);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            hoverConfig.HoverTextStyleSettings = new [] {
                hoverConfig.Styles_Warning.Standard,
                hoverConfig.Styles_Warning.Selected
            };

            Instance = this;
            interceptNumberKeysForPriority = true;

            offsets = new List<OffsetInfo>();
        }

        public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
        {
            colors = new HashSet<ToolMenu.CellColorData>();

            foreach(var offset in offsets)
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
            if (lineVis != null)
            {
                ropePathValid = true;

                var pos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
                var cell = Grid.PosToCell(pos);

                if (cell == lastCell)
                {
                    return;
                }

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

                lineVis.SetPosition(1, position);

                offsets.Clear();

                var offsetCells = ProcGen.Util.GetLine(origin, position);

                foreach (var offset in offsetCells)
                {
                    AddOffset(offset);
                    AddOffset(new Vector2I(offset.x, offset.y - 1));
                    AddOffset(new Vector2I(offset.x, offset.y - 2));
                }

                lastCell = cell;
                UpdateVis(pos);

                if(!ropePathValid)
                {
                    hoverConfig.errorMessage += "\nRope path must be unobstructed."; // TODO STRING
                }
            }
        }

        private void AddOffset(Vector2I offset)
        {
            var cell = Grid.PosToCell(offset);

            if (!CanRopePassThrough(cell))
            {
                offsets.Add(new OffsetInfo(offset, false));
                ropePathValid = false;
            }
            else
            {
                offsets.Add(new OffsetInfo(offset, true));
            }
        }

        private bool CanRopePassThrough(int cell)
        {
            if(!Grid.IsValidBuildingCell(cell))
            {
                return false;
            }

            return !Grid.Foundation[cell] && !Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].ContainsKey(cell);
        }

        public struct OffsetInfo
        {
            public Vector2I position;
            public bool valid;

            public OffsetInfo(Vector2I position, bool valid)
            {
                this.position = position;
                this.valid = valid;
            }
        }

        private void SetColor(Color color)
        {
            lineVis.material.SetColor("_TintColor", color);
            visualizer.GetComponent<KBatchedAnimController>().TintColour = color;
        }

        public void ActivateTool(ZiplineAnchor originalAnchor, Vector3 origin)
        {
            this.originalAnchor = originalAnchor;
            this.origin = origin;
            ActivateTool();
        }

        protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
        {
            position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
            position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);

            return position;
        }

        protected override void OnActivateTool()
        {
            if (visualizer != null)
            {
                Destroy(visualizer);
            }

            if (lineVis == null)
            {
                lineVis = Instantiate(ModAssets.ghostLinePrefab).GetComponent<LineRenderer>();
                def = Assets.GetBuildingDef(ZiplinePostConfig.ID);

                areaColour = Util.ColorFromHex(Mod.Settings.RopePathVisualizerColor);
                invalidColor = Util.ColorFromHex(Mod.Settings.RopePathVisualizerInvalidColor);
            }

            var vector = ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
            visualizer = GameUtil.KInstantiate(def.BuildingPreview, vector, Grid.SceneLayer.Ore, "Zipline connector building preview", LayerMask.NameToLayer("Place"));

            if (visualizer.TryGetComponent(out KBatchedAnimController kbac))
            {
                kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
                kbac.isMovable = true;
                kbac.Offset = def.GetVisualizerOffset();
                kbac.name = kbac.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
            }

            UpdateVis(vector);

            lineVis.SetPosition(0, origin);
            lineVis.gameObject.SetActive(true);

            PlayerController.Instance.ActivateTool(this);
            GridCompositor.Instance.ToggleMajor(true);

            base.OnActivateTool();
        }


        private void UpdateVis(Vector3 pos)
        {
            distance = Vector3.Distance(pos, origin);
            hoverConfig.distance = distance;

            if(distance > maxDistance)
            {
                hoverConfig.errorMessage = "Zipline too long.";
                SetColor(Color.red);
            }
            else
            {
                var visValid = def.IsValidBuildLocation(visualizer, pos, Orientation.Neutral, out string error);

                if (!visValid && !error.IsNullOrWhiteSpace())
                {
                    hoverConfig.errorMessage = error;
                }

                if (visValid)
                {
                    visValid = def.IsValidPlaceLocation(visualizer, pos, Orientation.Neutral, out string error2);

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
            if (def != null)
            {
                var snappedPosition = Grid.CellToPosCBC(cell, def.SceneLayer);
                visualizer.transform.SetPosition(snappedPosition);
                transform.SetPosition(snappedPosition - Vector3.up * 0.5f);

                if (lastCell != cell)
                {
                    lastCell = cell;
                }
            }
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);

            lineVis.gameObject.SetActive(false);

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

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            if(!ropePathValid)
            {
                return;
            }

            var pos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
            var cell = Grid.PosToCell(pos);
            
            if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var go) && go.TryGetComponent(out ZiplineAnchor anchor))
            {
                originalAnchor.AddConnection(anchor);
            }
            else
            {
                GameObject gameObject;
                var elements = originalAnchor.GetComponent<Deconstructable>().constructionElements;

                if (DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild))
                {
                    if (def.IsValidBuildLocation(visualizer, cursor_pos, Orientation.Neutral) && def.IsValidPlaceLocation(visualizer, cursor_pos, Orientation.Neutral, out var text))
                    {
                        gameObject = def.Build(cell, Orientation.Neutral, null, elements, 293.15f, false, GameClock.Instance.GetTime());
                        originalAnchor.AddConnection(gameObject.GetComponent<ZiplineAnchor>());
                    }
                }
                else
                {
                    gameObject = def.TryPlace(visualizer, cursor_pos, Orientation.Neutral, elements);

                    if (gameObject != null)
                    {
                        var prioritizable = gameObject.GetComponent<Prioritizable>();
                        if (prioritizable != null)
                        {

                            if (BuildMenu.Instance != null)
                            {
                                prioritizable.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
                            }

                            if (PlanScreen.Instance != null)
                            {
                                prioritizable.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
                            }
                        }
                    }

                    if (def.MaterialsAvailable(elements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
                    {
                        placeSound = GlobalAssets.GetSound("Place_Building_" + def.AudioSize, false);
                        if (placeSound != null)
                        {
                            var soundPos = new Vector3(cursor_pos.x, cursor_pos.y);
                            var instance = SoundEvent.BeginOneShot(placeSound, soundPos, 1f, false);
                            SoundEvent.EndOneShot(instance);
                        }
                    }
                    else
                    {
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, cursor_pos);
                    }
                }

                PlayerController.Instance.ActivateTool(SelectTool.Instance);
                // assign connection to it
            }
        }
    }
}
