using KSerialization;
using PrintingPodRecharge.Items;
using System;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BioPrinter : StateMachineComponent<BioPrinter.SMInstance>
    {
        [SerializeField]
        public Storage storage;

        [Serialize]
        public Tag DeliveryTag
        {
            get => smi.sm.deliveryTag.Get(smi);
            set 
            {
                smi.sm.deliveryTag.Set(value, smi);
                foreach(var item in storage.items)
                {
                    if(item.PrefabID() != value)
                    {
                        storage.Drop(item);
                    }
                }
            }
        }

        public bool IsDeliveryActive => DeliveryTag != Tag.Invalid;

        public bool IsReady => smi.IsInsideState(smi.sm.ready);

        public void Print() => smi.sm.print.Trigger(smi);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class States : GameStateMachine<States, SMInstance, BioPrinter>
        {
            public State idle;
            public State delivering;
            public State ready;

            public TagParameter deliveryTag;

            public Signal print;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;

                // TODO: operational

                idle
                    .ParamTransition(deliveryTag, delivering, IsValidTag);

                delivering
                    .Enter(StartNewDelivery)
                    .EventTransition(GameHashes.OnStorageChange, ready, HasEnoughInk)
                    .ParamTransition(deliveryTag, idle, IsInvalidTag)
                    .Exit(StopDelivery);

                ready
                    .Enter(RefreshSideScreen)
                    .OnSignal(print, idle)
                    .ToggleStatusItem("Ink Ready", "")
                    .Exit(RechargeTelepad);
            }

            private void RechargeTelepad(SMInstance smi)
            {
                ImmigrationModifier.Instance.IsOverrideActive = true;
                Immigration.Instance.timeBeforeSpawn = 0;

                var items = smi.master.storage.GetItems();
                foreach(var item in items)
                {
                    if(item.TryGetComponent(out BundleModifier modifier))
                    {
                        ImmigrationModifier.Instance.SetModifier(modifier.bundle);
                        break;
                    }
                }

                smi.master.storage.ConsumeAllIgnoringDisease();
                smi.master.DeliveryTag = Tag.Invalid;

                RefreshSideScreen(smi);
            }

            private Tag GetDeliveryTag(SMInstance smi) => smi.sm.deliveryTag.Get(smi);

            private bool HasEnoughInk(SMInstance smi)
            {
                return smi.master.storage.GetMassAvailable(GetDeliveryTag(smi)) >= smi.inkCapacity;
            }

            public void RefreshSideScreen(SMInstance smi)
            {
                if (smi.kSelectable.IsSelected)
                {
                    DetailsScreen.Instance.Refresh(smi.gameObject);
                }
            }

            private bool IsValidTag(SMInstance smi, Tag tag) => tag != Tag.Invalid;

            private bool IsInvalidTag(SMInstance smi, Tag tag) => !IsValidTag(smi, tag);

            private void StartNewDelivery(SMInstance smi)
            {
                smi.master.storage.DropAll();

                smi.delivery.requestedItemTag = smi.master.DeliveryTag;
                smi.delivery.RequestDelivery();

                RefreshSideScreen(smi);
            }

            private void StopDelivery(SMInstance smi)
            {
                smi.delivery.requestedItemTag = Tag.Invalid;
                RefreshSideScreen(smi);
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, BioPrinter, object>.GameInstance
        {
            [Serialize]
            public Tag bioInkTag;

            public ManualDeliveryKG delivery;
            public KSelectable kSelectable;

            public float inkCapacity = 2f;

            public SMInstance(BioPrinter master) : base(master) 
            {
                delivery = master.GetComponent<ManualDeliveryKG>();
                kSelectable = master.GetComponent<KSelectable>(); 
            }
        }
    }
}
