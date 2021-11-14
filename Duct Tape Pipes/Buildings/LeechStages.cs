using FUtility;

namespace DuctTapePipes.Buildings
{
    public class LeechStages : GameStateMachine<LeechStages, LeechStages.Instance>
    {
        public State disconnected;
        public State unconfigured;
        public ConnectedState connected;

        public Signal disconnect;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = disconnected;

            disconnected
                .Enter(smi => smi.GetComponent<Operational>().SetActive(false))
                .ToggleStatusItem(ModAssets.disconnectedStatus)
                //.EventHandlerTransition(GameHashes.NewBuilding, unconfigured, (smi, obj) => smi.HasStorage(obj))
                .EventTransition(GameHashes.NewBuilding, unconfigured, smi => smi.HasStorage());

            unconfigured
                .ToggleStatusItem(ModAssets.unconfiguredStatus)
                .GoTo(connected)
                .Enter(smi => Log.Debuglog("ENTER UNCONFIGURED"));

            connected
                .DefaultState(connected.unoperational)
                .OnSignal(disconnect, disconnected)
                .Enter(smi => Log.Debuglog("ENTER CONNECTED"))
                .ToggleStatusItem(ModAssets.connectedStatus);

            connected.unoperational
                .Enter(smi => Log.Debuglog("unoperational"))
                .Transition(connected.unoperational, smi => smi.IsHostOperational())
                .EventTransition(GameHashes.OperationalChanged, connected.idle, smi => smi.GetComponent<Operational>().IsOperational);

            connected.idle
                .Enter(smi => Log.Debuglog("idle"))
                .Update((smi, dt) => smi.GetComponent<Leech>().UpdateStorage(), UpdateRate.SIM_200ms)
                .EventTransition(GameHashes.OperationalChanged, connected.unoperational, smi => !smi.GetComponent<Operational>().IsOperational);
        }

        public class ConnectedState : State {
            public State idle;
            public State unoperational;
        }

        public new class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master) : base(master) { }

            public bool HasStorage()
            {
                return GetComponent<Leech>().FindStorage();
            }

            public bool IsHostOperational()
            {
                return GetComponent<Leech>().IsHostOperational();
            }
        }
    }
}
