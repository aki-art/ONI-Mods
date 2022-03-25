using AETNTweaks.Buildings.PyrositePylon;
using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AETNTweaks.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class PyrositeController : StateMachineComponent<PyrositeController.SMInstance>
    {
        [SerializeField]
        public int range;

        [SerializeField]
        public float extraConsumptionPerPyrosite;

        public Transform TetherAnchor { get; private set; }

        [MyCmpReq]
        ConduitConsumer consumer;

        private List<Pyrosite> attachedPyrosites;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            attachedPyrosites = new List<Pyrosite>();
            TetherAnchor = CreateAnchor();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
            TetherAnchor.transform.position = transform.position + new Vector3(0.5f, 3.5f);
        }

        private Transform CreateAnchor()
        {
            var goRight = new GameObject();
            goRight.transform.parent = transform;
            goRight.SetActive(true);
            return goRight.transform;
        }

        protected override void OnCleanUp()
        {
            foreach(Pyrosite pyrosite in attachedPyrosites)
            {
                DetachPyrosite(pyrosite);
            }

            Destroy(TetherAnchor.gameObject);
            base.OnCleanUp();

        }

        public void AttachPyrosite(Pyrosite pyrosite)
        {
            if (!attachedPyrosites.Contains(pyrosite))
            {
                attachedPyrosites.Add(pyrosite);
                consumer.capacityKG += extraConsumptionPerPyrosite;
            }

            OnConnectionsChanged();
        }

        public void DetachPyrosite(Pyrosite pyrosite)
        {
            if (attachedPyrosites.Remove(pyrosite))
            {
                consumer.capacityKG -= extraConsumptionPerPyrosite;
                pyrosite.Detach();
            }

            OnConnectionsChanged();
        }

        private void OnConnectionsChanged()
        {
            Log.Debuglog("CONNECTIONS CHANGED", attachedPyrosites.Count > 0);
            smi.sm.hasConnections.Set(attachedPyrosites.Count > 0, smi);
        }

        public class States : GameStateMachine<States, SMInstance, PyrositeController>
        {
            public State alone;
            public State beingAParent;

            public BoolParameter hasConnections;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = alone;

                alone
#if DEBUG
                    .ToggleStatusItem("Alone", "")
#endif
                    .ParamTransition(hasConnections, beingAParent, IsTrue);

                beingAParent
#if DEBUG
                    .ToggleStatusItem("Parenting", "")
#endif
                    .ParamTransition(hasConnections, alone, IsFalse)
                    .Update(TransferFuel, UpdateRate.RENDER_200ms);
            }

            private void TransferFuel(SMInstance smi, float dt)
            {
                FUtility.Log.Debuglog("TRANSFERRING");

                foreach(var pyrosite in smi.master.attachedPyrosites)
                {
                    FUtility.Log.Debuglog(pyrosite.name);
                    var targetStorage = pyrosite.storage;

                    if (targetStorage.IsFull()) continue;

                    //var mass = Mathf.Min(0.1f * dt, targetStorage.capacityKg - targetStorage.MassStored());

                    FUtility.Log.Debuglog(smi.storage.Transfer(targetStorage, smi.fuelTag, 0.1f * dt));
                }
            }
        }

        [SerializationConfig(MemberSerialization.OptIn)]
        public class SMInstance : GameStateMachine<States, SMInstance, PyrositeController, object>.GameInstance
        {
            public Storage storage;
            public Tag fuelTag;

            public SMInstance(PyrositeController master) : base(master)
            {
                storage = master.GetComponent<Storage>();
                fuelTag = SimHashes.Hydrogen.CreateTag();
            }
        }
    }
}
