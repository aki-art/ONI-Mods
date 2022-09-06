using FUtility;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Content.Entities;
using ZipLine.Content.Tools;

namespace ZipLine.Content.Buildings.ZiplinePost
{
    // The anchor can connect to other anchors. a Rope entity will spawn between them and handle the actual connection
    public class ZiplineAnchor : KMonoBehaviour, ISidescreenButtonControl
    {
        public static Vector3 verticalOffset = new Vector3(0f, 2.81f);
        public static Vector3 horizontalOffset = new Vector3(1f, 0);

        [Serialize]
        private List<ZiplineAnchor> connections;

        public int Cell => Grid.PosToCell(this);

        public string SidescreenButtonText => STRINGS.UI.ZIPLINE.CONNECT;

        public string SidescreenButtonTooltip => STRINGS.UI.ZIPLINE.CONNECT_TOOLTIP;

        public ZiplineAnchor()
        {
            connections = new List<ZiplineAnchor>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Mod.anchors.Add(this);


            /*
            if (connectionCells != null)
            {
                foreach (var cell in connectionCells)
                {
                    if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var go))
                    {
                        if (go.TryGetComponent(out ZiplineAnchor anchor))
                        {
                            AddConnection(anchor);
                        }

                        Log.Warning($"Zipline anchor used to have a connection at cell {cell}, but it cannot be found anymore. Connection could not be restored.");
                    }
                }
            }
            */
        }

        public void AddConnection(ZiplineAnchor other, bool skipFx = false)
        {
            var offsets = new List<Vector2I>(); // TODO: calculate
            AddConnection(other, offsets, skipFx);
        }

        public void AddConnection(ZiplineAnchor other, List<Vector2I> cellOffsets, bool skipFx = false)
        {
            if (connections.Contains(other))
            {
                return;
            }

            if (this.GetMyWorldId() != other.GetMyWorldId())
            {
                Log.Warning("Trying to connect zipline anchors across different worlds. In the tragedy of this cruel world, this could never work out between these two.");
                return;
            }

            connections.Add(other);
            other.connections.Add(this);

            //TetherVisualizer.Instance.AddTether(Cell, other.Cell);

            var ropePrefab = Assets.GetPrefab(RopeConfig.ID);
            var rope = Instantiate(ropePrefab).GetComponent<Rope>();


            rope.a = this;
            rope.b = other;
            rope.offset = verticalOffset;

            rope.gameObject.SetActive(true);

            rope.SetArea(cellOffsets);

            if (Helper.IsInstantBuilding())
            {
                rope.Complete();
            }

            if(!skipFx)
            {
                var pos = Grid.CellToPosCBC(Cell, Grid.SceneLayer.Building);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, "Connected", null, pos);
                // rope tighten sound
            }

        }

        public void RemoveConnection(ZiplineAnchor other)
        {
            connections.Remove(other);
            other.connections.Remove(this);
        }

        public void RemoveAllConnections()
        {
            for (var i = connections.Count - 1; i >= 0; i--)
            {
                var connection = connections[i];
                connection.RemoveConnection(this);
            }

            connections.Clear();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            RemoveAllConnections();
        }

        public bool SidescreenEnabled()
        {
            return true;
        }

        public bool SidescreenButtonInteractable()
        {
            return true;
        }

        public void OnSidescreenButtonPressed()
        {
            //var snappedPosition = Grid.CellToPos(Cell);
            ZipConnectorTool.Instance.ActivateTool(this, transform.position + verticalOffset, Grid.CellBelow(this.NaturalBuildingCell()));
        }

        public int ButtonSideScreenSortOrder()
        {
            return 999;
        }
    }
}
