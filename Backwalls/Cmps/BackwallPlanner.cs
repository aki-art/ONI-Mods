namespace Backwalls.Cmps
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
            if (BackwallStorage.Instance.data.ContainsKey(cell))
            {
                BackwallStorage.Instance.data.Remove(cell);
            }
        }
    }
}
