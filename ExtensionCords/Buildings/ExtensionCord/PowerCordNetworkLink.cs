namespace ExtensionCords.Buildings.ExtensionCord
{
    public class PowerCordNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
    {
        public bool IsVirtual { get; private set; }

        public int PowerCell => GetNetworkCell();

        public object VirtualCircuitKey { get; private set; }

        public IUtilityNetworkMgr GetNetworkManager()
        {
            return Game.Instance.electricalConduitSystem;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            //FindObjectOfType<>
        }
    }
}
