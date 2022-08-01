namespace Backwalls.Cmps
{
    // mostly was meant to be used with blueprints, so if a planned but not yet build backwall with pattern data is cancelled, the pattern is forgotten too
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
            if (BackwallStorage.Instance.data.ContainsKey(cell))
            {
                BackwallStorage.Instance.data.Remove(cell);
            }
        }
    }
}
