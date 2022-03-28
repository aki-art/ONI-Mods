using AETNTweaks.Cmps;
using UnityEngine;

namespace AETNTweaks.Buildings.PyrositePylon
{
    public class Pyrosite : StateMachineComponent<Pyrosite.SMInstance>
    {
        [SerializeField]
        public float activeDuration;

        private Extents extents;

        [MyCmpReq]
        public Storage storage;

        public PyrositeController ParentController
        {
            get => smi.sm.masterAETN.Get(smi)?.GetComponent<PyrositeController>();
            set => smi.sm.masterAETN.Set(value, smi);
        }

        public static Operational.Flag flag = new Operational.Flag("hasParentAETN", Operational.Flag.Type.Functional);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
            extents = new Extents(this.NaturalBuildingCell(), Mod.Settings.PyrositeAttachRadius);
            //CheckForAETNNearby();

            GameScheduler.Instance.ScheduleNextFrame("", obj => CheckForAETNNearby());
        }


        public void AttachTo(PyrositeController controller)
        {
            ParentController = controller;
            ParentController.AttachPyrosite(this);
        }

        private void CheckForAETNNearby()
        {
            ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList list = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(smi.master.extents, GameScenePartitioner.Instance.completeBuildings, list);

            foreach (ScenePartitionerEntry scenePartitionerEntry in list)
            {
                if (scenePartitionerEntry.obj is BuildingComplete go && go.TryGetComponent(out PyrositeController controller))
                {
                    AttachTo(controller);
                }
            }

            list.Recycle();
        }

        public void Detach()
        {
            ParentController = null;
        }

        protected override void OnCleanUp()
        {
            ParentController?.DetachPyrosite(this);
            base.OnCleanUp();
        }

        public class States : GameStateMachine<States, SMInstance, Pyrosite>
        {
            public State attached;
            public State stray;

            public TargetParameter masterAETN;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = stray;

                root
                    .InitializeOperationalFlag(flag, false);

                stray
                    .Enter(StartPartitioner)
                    .Exit(StopPartitioner)
                    .ParamTransition(masterAETN, attached, HasController)
                    .ToggleStatusItem("Not Connected to an AETN", "");

                attached
                    .Enter(ShowTether)
                    .ScheduleActionNextFrame("", smi => UpdateTether(smi, 1f))
                    .Exit(HideTether)
                    .OnTargetLost(masterAETN, stray)
                    .ToggleOperationalFlag(flag)
                    .ToggleStatusItem("Attached", "")
                    .Update(UpdateTether, UpdateRate.SIM_4000ms);
            }

            private void UpdateTether(SMInstance smi, float dt)
            {
                if (smi.transform.position != smi.lastPosition)
                {
                    smi.tether.Settle(dt);
                    smi.lastPosition = smi.transform.position;
                }
            }

            private bool HasController(SMInstance smi, GameObject go)
            {
                return go != null && go.GetComponent<PyrositeController>() != null;
            }

            private void ShowTether(SMInstance smi)
            {
                smi.tether.enabled = true;
                smi.tether.SetEnds(smi.master.ParentController.TetherAnchor, smi.transform);
                smi.tether.Settle(0);
            }

            private void HideTether(SMInstance smi)
            {
                //smi.tether.Stop();
                smi.tether.enabled = false;
            }

            private static void StartPartitioner(SMInstance smi)
            {
                int cell = Grid.CellBelow(Grid.PosToCell(smi));
                ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)ObjectLayer.Building];

                smi.buildingPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresAETN.Add", smi, smi.master.extents, layer, smi.OnBuildingsChanged);
            }

            public static void StopPartitioner(SMInstance smi)
            {
                GameScenePartitioner.Instance.Free(ref smi.buildingPartitionerEntry);
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, Pyrosite, object>.GameInstance
        {
            internal HandleVector<int>.Handle buildingPartitionerEntry;
            public Tether tether;

            public Vector3 lastPosition;

            public SMInstance(Pyrosite master) : base(master)
            {
                tether = master.GetComponent<Tether>();
            }

            protected override void OnCleanUp()
            {
                base.OnCleanUp();
                GameScenePartitioner.Instance.Free(ref smi.buildingPartitionerEntry);
            }

            internal void OnBuildingsChanged(object obj)
            {
                if (obj is GameObject go && go.TryGetComponent(out PyrositeController controller))
                {
                    smi.master.AttachTo(controller);
                }
            }
        }
    }
}