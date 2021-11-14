namespace DuctTapePipes.Buildings
{
    public class LiquidOutputLeech : Leech
    {
        [MyCmpReq]
        private ConduitDispenser dispenser;

        [MyCmpReq]
        private Storage placeholderStorage;

        public override void UpdateStorage()
        {
            if (!HostExists())
            {
                Disconnect();
                return;
            }

            HostStorage.Transfer(storage, true, true);
        }
    }
}
