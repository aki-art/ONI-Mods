using UnityEngine;

namespace Backwalls.Buildings
{
    public class BackwallLink : KMonoBehaviour//, IBlockTileInfo
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();

            Subscribe((int)GameHashes.SelectObject, OnSelectionChanged);
            Subscribe((int)GameHashes.HighlightObject, OnHighlightChanged);
        }

        protected override void OnCleanUp()
        {
            if (TryGetComponent(out Building building))
            {
                int cell = Grid.PosToCell(base.transform.GetPosition());
                var tileLayer = building.Def.TileLayer;

                if (Grid.Objects[cell, (int)tileLayer] == gameObject)
                {
                    Grid.Objects[cell, (int)tileLayer] = null;
                }

                //TileVisualizer.RefreshCell(cell, tileLayer, building.Def.ReplacementLayer);
            }

            base.OnCleanUp();
        }

        private void OnSelectionChanged(object data)
        {
            bool enabled = (bool)data;
            //World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
            Mod.renderer.SelectCell(Grid.PosToCell(transform.GetPosition()), enabled);
        }

        private void OnHighlightChanged(object data)
        {
            bool enabled = (bool)data;
            Mod.renderer.HighlightCell(Grid.PosToCell(transform.GetPosition()), enabled);
        }
    }
}
