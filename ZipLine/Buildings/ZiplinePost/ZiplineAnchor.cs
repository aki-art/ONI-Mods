using FUtility;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Tools;

namespace ZipLine.Buildings.ZiplinePost
{
    public class ZiplineAnchor : KMonoBehaviour, ISidescreenButtonControl
    {
        public static Vector3 offset = new Vector3(0.6f, 2.81f);

        [Serialize]
        private List<ZiplineAnchor> connections;

        public int Cell => Grid.PosToCell(this);

        public string SidescreenButtonText => "Connect";

        public string SidescreenButtonTooltip => "";

        public ZiplineAnchor()
        {
            connections = new List<ZiplineAnchor>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();


            // TEST 
            var ziplineAnchors = Mod.anchors.GetWorldItems(this.GetMyWorldId());
            if(ziplineAnchors != null && ziplineAnchors.Count > 0)
            {
                var randomOther = ziplineAnchors.GetRandom();
                if (randomOther != null)
                {
                    AddConnection(randomOther);
                }
            }

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

        public void AddConnection(ZiplineAnchor other)
        {
            if(connections.Contains(other))
            {
                return;
            }

            if(this.GetMyWorldId() != other.GetMyWorldId())
            {
                Log.Warning("Trying to connect zipline anchors across different worlds. In the tragedy of this cruel world, this could never work out between these two.");
                return;
            }

            connections.Add(other);
            other.connections.Add(this);

            TetherVisualizer.Instance.AddTether(Cell, other.Cell);

            var pos = Grid.CellToPosCBC(Cell, Grid.SceneLayer.Building);
            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, "Connected", null, pos);

            // rope tighten sound
        }

        public void RemoveConnection(ZiplineAnchor other)
        {
            connections.Remove(other);
            other.connections.Remove(this);

            TetherVisualizer.Instance.RemoveTether(Cell, other.Cell);
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

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public void OnSidescreenButtonPressed()
        {
            var snappedPosition = Grid.CellToPos(Cell);
            ZipConnectorTool.Instance.ActivateTool(this, snappedPosition + offset);
        }

        public int ButtonSideScreenSortOrder() => 999;
    }
}
