using AETNTweaks.Cmps;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AETNTweaks.Buildings.PyrositePylon
{
    public class TetherPlaceVisualizerOld : StateMachineComponent<TetherPlaceVisualizerOld.SMInstance>
    {
        [MyCmpReq]
        private Tether tether;

        [SerializeField]
        public Vector3 offset;

        private PyrositeController targetSink;
        private List<PyrositeController> controllers;
        private Transform targetAnchor;
        private float sqrDistance;

        public GameObject MasterAETN
        {
            get => smi.sm.masterAETN.Get(smi);
            set => smi.sm.masterAETN.Set(value, smi);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            sqrDistance = Mod.Settings.PyrositeAttachRadius * Mod.Settings.PyrositeAttachRadius;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            GameObject target = new GameObject("target");
            target.SetActive(true);
            targetAnchor = target.transform;
            tether.SetEnds(targetAnchor, transform);

            controllers = new List<PyrositeController>();
            foreach (BuildingComplete building in global::Components.BuildingCompletes)
            {
                if (building.TryGetComponent(out PyrositeController sink))
                {
                    controllers.Add(sink);
                }
            }
;
        }
        private void StopUpdatingTether()
        {
            StopCoroutine(SettleTether());
        }

        private void StartUpdatingTether()
        {
            StartCoroutine(SettleTether());
        }

        private IEnumerator SettleTether()
        {
            while(true)
            {
                tether.Settle(Time.deltaTime);
                yield return new WaitForSecondsRealtime(0.033f);
            }
        }

        private void RefreshTarget()
        {
            if (targetSink != null)
            {
                // check if it is still within range
                float dist = (targetSink.transform.position - transform.position).sqrMagnitude;
                if (dist > sqrDistance)
                {
                    targetSink = null;
                }
            }

            if (targetSink == null)
            {
                foreach (PyrositeController heatSink in controllers)
                {
                    if (heatSink != null && (heatSink.transform.position - transform.position).sqrMagnitude <= sqrDistance)
                    {
                        targetSink = heatSink;
                        targetAnchor.position = targetSink.transform.position + offset;
                        tether.SetEnds(targetAnchor, transform);
                        break;
                    }
                }
            }
        }

        protected override void OnCleanUp()
        {
            controllers = null;

            if (targetAnchor != null)
            {
                Destroy(targetAnchor.gameObject);
            }

            base.OnCleanUp();
        }

        public class States : GameStateMachine<States, SMInstance, TetherPlaceVisualizerOld>
        {
            public State searchForMaster;
            public State connected;

            public TargetParameter masterAETN;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = searchForMaster;

                searchForMaster
                    .Update(LookForMaster, UpdateRate.SIM_200ms)
                    .ParamTransition(masterAETN, connected, IsNotNull);

                connected
                    .Enter(InitTether)
                    .Exit(DisableTether)
                    .Update(UpdateMaster, UpdateRate.SIM_200ms)
                    .OnTargetLost(masterAETN, searchForMaster);
            }

            private void DisableTether(SMInstance smi)
            {
                smi.tether.enabled = false;
                smi.master.StopUpdatingTether();
            }

            private void InitTether(SMInstance smi)
            {
                smi.master.targetAnchor.position = smi.master.MasterAETN.transform.position + smi.master.offset;

                smi.tether.enabled = true;
                smi.tether.SetEnds(smi.master.MasterAETN.transform, smi.transform);

                smi.master.StartUpdatingTether();
            }

            private void UpdateMaster(SMInstance smi, float dt)
            {
                var dist = (smi.master.transform.position - smi.master.MasterAETN.transform.position).sqrMagnitude;

                if (dist > smi.maxDistance)
                {
                    smi.master.MasterAETN = null;
                }
            }

            private void LookForMaster(SMInstance smi, float dt)
            {
                throw new NotImplementedException();
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, TetherPlaceVisualizerOld, object>.GameInstance
        {
            public Tether tether;
            public float maxDistance;

            public SMInstance(TetherPlaceVisualizerOld master) : base(master)
            {
                tether = master.GetComponent<Tether>();
                maxDistance = Mod.Settings.PyrositeAttachRadius * Mod.Settings.PyrositeAttachRadius;
            }
        }
    }
}
