using FUtility;
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

        [MyCmpReq]
        public KSelectable kSelectable;

        [Serialize]
        public Tag DeliveryTag
        {
            get => smi.sm.deliveryTag.Get(smi);
            set
            {
                smi.sm.deliveryTag.Set(value, smi);
                //smi.delivery.Pause(value != Tag.Invalid, "Tag changed");

                foreach (var item in storage.items)
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

            Log.Debuglog("TAG IS " + DeliveryTag);

            if(DeliveryTag != Tag.Invalid && !ImmigrationModifier.Instance.IsOverrideActive)
            {
            }
        }

        public void RefreshSideScreen()
        {
            if (kSelectable.IsSelected)
            {
                DetailsScreen.Instance.Refresh(gameObject);
            }
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
                    .ToggleStatusItem("Idle", "")
                    .EnterTransition(delivering, IsValidTag)
                    .ParamTransition(deliveryTag, delivering, IsValidTag);

                delivering
                    .ToggleStatusItem("Delivering", "")
                    .Toggle("ToggleDelivery", StartNewDelivery, StopDelivery)
                    .EventTransition(GameHashes.OnStorageChange, ready, HasEnoughInk)
                    .ParamTransition(deliveryTag, idle, IsInvalidTag);

                ready
                    .Enter(smi => smi.master.RefreshSideScreen())
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

                smi.master.RefreshSideScreen();
            }

            private Tag GetDeliveryTag(SMInstance smi) => smi.sm.deliveryTag.Get(smi);

            private bool HasEnoughInk(SMInstance smi)
            {
                return smi.master.storage.GetMassAvailable(GetDeliveryTag(smi)) >= smi.inkCapacity;
            }

            private bool IsValidTag(SMInstance smi)
            {
                return smi.master.DeliveryTag != Tag.Invalid;
            }


            private bool IsValidTag(SMInstance smi, Tag tag) => tag != Tag.Invalid;

            private bool IsInvalidTag(SMInstance smi, Tag tag) => !IsValidTag(smi, tag);

            private void StartNewDelivery(SMInstance smi)
            {
                Log.Debuglog("Start new delivery");
                smi.master.storage.DropAll();

                Log.Debuglog(smi.master.DeliveryTag);
                smi.delivery.requestedItemTag = smi.master.DeliveryTag;
                Log.Debuglog($"requested tag: {smi.delivery.requestedItemTag}");
                Log.Debuglog($"is paused: {smi.delivery.IsPaused}");
                smi.delivery.RequestDelivery();
                Log.Debuglog($"is fetchlist null: {smi.delivery.DebugFetchList == null}");
                Log.Debuglog($"fl count: {smi.delivery.DebugFetchList?.FetchOrders?.Count}");
                Log.Debuglog($"is fetchlist complete: {smi.delivery.DebugFetchList?.IsComplete}");

                smi.master.RefreshSideScreen();
            }

            private void StopDelivery(SMInstance smi)
            {
                Log.Debuglog("Stopping delivery");
                smi.delivery.requestedItemTag = Tag.Invalid;

                smi.master.RefreshSideScreen();
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, BioPrinter, object>.GameInstance
        {
            [Serialize]
            public Tag bioInkTag;

            public ManualDeliveryKG delivery;

            public float inkCapacity = 2f;

            public SMInstance(BioPrinter master) : base(master) 
            {
                delivery = master.GetComponent<ManualDeliveryKG>();
            }
        }
    }
}
