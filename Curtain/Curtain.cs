using KSerialization;
using System;
using UnityEngine;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class Curtain : Workable, ISaveLoadable, ISim200ms
    {
        [MyCmpReq]
        public Building building;
        [Serialize]
        public ControlState CurrentState { get; set; }
        [Serialize]
        public ControlState RequestedState { get; set; }
#pragma warning disable IDE0052
        private WorkChore<Curtain> changeStateChore;
#pragma warning restore IDE0052 
        private readonly KAnimFile[] anims = new KAnimFile[] { Assets.GetAnim("anim_use_remote_kanim") };

        public Curtain()
        {
            SetOffsetTable(OffsetGroups.InvertedStandardTable);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            overrideAnims = anims;
            synchronizeAnims = false;
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            controller = new Controller.Instance(this);
            controller.StartSM();

            SetDefaultCellFlags();
            UpdateState();
            StartPartitioner();
        }

        private void Open()
        {
            CurrentState = ControlState.Open;
            foreach (int cell in building.PlacementCells)
                Dig(cell);
            UpdateController();
        }

        private void Close()
        {
            CurrentState = ControlState.Auto;
            DisplaceElement();
            UpdateController();
        }

        private void Lock()
        {
            CurrentState = ControlState.Locked;
            DisplaceElement();
            UpdateController();
        }

        protected override void OnCleanUp()
        {
            foreach (int cell in building.PlacementCells)
            {
                CleanSim(cell);
                SetCellPassable(cell, false, false);
            }

            GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
            base.OnCleanUp();
        }

        protected override void OnCompleteWork(Worker worker)
        {
            base.OnCompleteWork(worker);
            changeStateChore = null;
            ApplyRequestedControlState();
        }

        private void ApplyRequestedControlState()
        {
            CurrentState = RequestedState;
            UpdateState();
            GetComponent<KSelectable>().RemoveStatusItem(ModAssets.CurtainStatus);
            Trigger((int)GameHashes.DoorStateChanged, this);
        }

        public void QueueStateChange(ControlState state)
        {
            RequestedState = state;
            if (state == CurrentState) return;

            if (DebugHandler.InstantBuildMode)
            {
                CurrentState = RequestedState;
                UpdateState();
                return;
            }

            GetComponent<KSelectable>().AddStatusItem(ModAssets.CurtainStatus, this);

            changeStateChore = new WorkChore<Curtain>(
                chore_type: Db.Get().ChoreTypes.Toggle,
                target: this,
                only_when_operational: false);
        }

        private void OnCopySettings(object obj)
        {
            var curtain = ((GameObject)obj).GetComponent<Curtain>();
            if (curtain != null)
            {
                QueueStateChange(curtain.RequestedState);
                return;
            }

            // Allowing curtains to have their settings copied to doors
            var door = ((GameObject)obj).GetComponent<Door>();
            if (door != null)
                QueueStateChange((ControlState)door.RequestedState);
        }

        public void Sim200ms(float dt)
        {
            if (meltCheck)
            {
                var structureTemperatures = GameComps.StructureTemperatures;
                var handle = structureTemperatures.GetHandle(gameObject);
                if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
                {
                    foreach (int cell in building.PlacementCells)
                    {
                        if (!Grid.Solid[cell])
                        {
                            Util.KDestroyGameObject(this);
                            return;
                        }
                    }
                }
            }
        }

        public enum ControlState
        {
            Auto,
            Open,
            Locked
        }
    }
}
