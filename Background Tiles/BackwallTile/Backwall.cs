namespace BackgroundTiles.BackwallTile
{
    public class Backwall : KMonoBehaviour
    {
        [MyCmpGet]
        private readonly Building building;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            /*
            building.RunOnArea(cell =>
            {
                Grid.RenderedByWorld[cell] = false;
                Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(cell);
                SimMessages.SetCellProperties(cell, (byte)Sim.Cell.Properties.Transparent);
            });
            */
        }

    }
}
