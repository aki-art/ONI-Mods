using Backwalls.Integration.Blueprints;

namespace Backwalls.Buildings
{
    public class BackwallPlanner : KMonoBehaviour
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.Cancel, OnCancel);
        }

        private void OnCancel(object obj)
        {
            var cell = this.NaturalBuildingCell();
            if (BluePrintsPatch.wallDataCache.ContainsKey(cell))
            {
                BluePrintsPatch.wallDataCache.Remove(cell);
            }
        }
    }
}
