namespace DuctTapePipes.Buildings
{
    public class LiquidInputLeech : Leech
    {
        [MyCmpReq]
        private ConduitConsumer consumer;

        public override void UpdateStorage()
        {
            if (!HostExists())
            {
                Disconnect();
                return;
            }

            storage.Transfer(HostStorage, true, true);
        }

        protected override bool Connect(Storage storage)
        {
            if(base.Connect(storage))
            {
                if (HostStorage.TryGetComponent(out ManualDeliveryKG manualDelivery))
                {
                    manualDelivery.Pause(true, "conduit connected");
                    manualDelivery.enabled = false;
                }

                return true;
            }

            return false;
        }

        public override void Disconnect()
        {
            if (HostStorage.TryGetComponent(out ManualDeliveryKG manualDelivery))
            {
                //manualDelivery.Pause(false, "conduit disconnected");
                manualDelivery.enabled = true;
            }
            base.Disconnect();
        }
    }
}
