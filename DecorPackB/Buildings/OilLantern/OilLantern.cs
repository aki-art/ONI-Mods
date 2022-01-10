using HarmonyLib;
using System;

namespace DecorPackB.Buildings.OilLantern
{
    internal class OilLantern : StateMachineComponent<OilLantern.StatesInstance>
    {
        [MyCmpReq]
        private Operational operational;

        [MyCmpReq]
        private ElementConverter elementConverter;

        [MyCmpReq]
        private ConduitConsumer consumer;

        [MyCmpReq]
        private ManualDeliveryKG manualDelivery;

        private Traverse manualDeliveryTraverse;

        private static readonly Operational.Flag requiresFuelFlag = new Operational.Flag("hasFuel", Operational.Flag.Type.Requirement);
        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();

            Subscribe((int)GameHashes.ConduitConnectionChanged, OnConduitConnectionChanged);

            manualDeliveryTraverse = Traverse.Create(manualDelivery).Field("userPaused");
        }

        private bool IsManuallyPaused() => manualDeliveryTraverse.GetValue<bool>();

        private void OnConduitConnectionChanged(object obj)
        {
            if(IsManuallyPaused())
            {
                return;
            }

            manualDelivery.Pause(consumer.IsConnected, "Conduit Connection");
        }

        public class States : GameStateMachine<States, StatesInstance, OilLantern>
        {
            public State disabled;
            public State off;
            public State on;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = disabled;

                disabled
                    .Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
                    .EventTransition(GameHashes.FunctionalChanged, off, smi => smi.master.operational.IsFunctional);
                off
                    .EventTransition(GameHashes.FunctionalChanged, disabled, smi => !smi.master.operational.IsFunctional)
                    .Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
                    .ToggleStatusItem(ModAssets.StatusItems.awaitingFuel, smi => smi.master)
                    .EventTransition(GameHashes.OnStorageChange, on, smi => smi.master.elementConverter.HasEnoughMassToStartConverting());
                on
                    .EventTransition(GameHashes.FunctionalChanged, disabled, smi => !smi.master.operational.IsFunctional)
                    .Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, true))
                    .EventTransition(GameHashes.OnStorageChange, off, smi => !smi.master.elementConverter.HasEnoughMassToStartConverting());
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, OilLantern, object>.GameInstance
        {
            public StatesInstance(OilLantern master) : base(master)
            {
            }
        }
    }
}
