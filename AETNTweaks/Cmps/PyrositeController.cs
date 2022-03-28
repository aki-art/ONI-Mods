using AETNTweaks.Buildings.PyrositePylon;
using FUtility;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace AETNTweaks.Cmps
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
        private ConduitConsumer consumer;

        private List<Pyrosite> attachedPyrosites;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            TetherAnchor = CreateAnchor();
            attachedPyrosites = new List<Pyrosite>();
        }

        protected override void OnSpawn()
        {
            Log.Debuglog("SETTING POSITION HERE");
            TetherAnchor.position = transform.position + new Vector3(0.5f, 3.5f);
            base.OnSpawn();
            smi.StartSM();
        }

        private Transform CreateAnchor()
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.SetActive(true);

            return go.transform;
        }

#if DEBUG
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(200, 200, 400, 500));

            if(TetherAnchor == null)
            {
                GUILayout.Label($"Tether anchor does not exist yet");
                GUILayout.Label(attachedPyrosites?.Count.ToString());
                return;
            }

            GUILayout.Label($"Anchor pos: {TetherAnchor.position}");
            GUILayout.Space(10);

            foreach(Pyrosite pyrosite in attachedPyrosites)
            {
                var tether = pyrosite.GetComponent<Tether>();
                GUILayout.Label($"{tether.A.position} -> {tether.B.position}");
            }

            GUILayout.EndArea();
        }
#endif

        protected override void OnCleanUp()
        {
            foreach (Pyrosite pyrosite in attachedPyrosites)
            {
                consumer.capacityKG -= extraConsumptionPerPyrosite;
                pyrosite.Detach();
            }

            attachedPyrosites.Clear();
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
                    .ToggleStatusItem(
                        "{0} Pyrosites connected", 
                        "", 
                        resolve_string_callback: (str, obj) => (obj.master.attachedPyrosites.Count * 0.1f) + " g/s")
                    .ParamTransition(hasConnections, alone, IsFalse)
                    .Update(TransferFuel, UpdateRate.SIM_200ms);
            }

            private void TransferFuel(SMInstance smi, float dt)
            {
                if (smi.master.attachedPyrosites == null) return;

                foreach (Pyrosite pyrosite in smi.master.attachedPyrosites)
                {
                    Storage targetStorage = pyrosite.storage;
                    if (targetStorage.IsFull()) continue;
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
