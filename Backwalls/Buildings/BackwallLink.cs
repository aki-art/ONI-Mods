namespace Backwalls.Buildings
{
    public class BackwallLink : KMonoBehaviour
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
                var cell = Grid.PosToCell(base.transform.GetPosition());
                var tileLayer = ObjectLayer.Backwall;

                if (Grid.Objects[cell, (int)tileLayer] == gameObject)
                {
                    Grid.Objects[cell, (int)tileLayer] = null;
                }
            }

            base.OnCleanUp();
        }

        private void OnSelectionChanged(object data)
        {
            var enabled = (bool)data;
            Mod.renderer.SelectCell(Grid.PosToCell(transform.GetPosition()), enabled);
        }

        private void OnHighlightChanged(object data)
        {
            var enabled = (bool)data;
            Mod.renderer.HighlightCell(Grid.PosToCell(transform.GetPosition()), enabled);
        }
    }
}
