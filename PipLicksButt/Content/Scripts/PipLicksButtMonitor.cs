
namespace PipLicksButt.Content.Scripts
{
    public class PipKicksButtMonitor : GameStateMachine<PipKicksButtMonitor, PipKicksButtMonitor.Instance, IStateMachineTarget, PipKicksButtMonitor.Def>
    {
        public static Tag TAG = ModTags.wantsToLickButt;

        public class Def : BaseDef
        {
            public float secondsPerLickMax;

            public float cooldown;

            public void Initialize(int licksPerCycle, float cooldown)
            {
                secondsPerLickMax = Constants.SECONDS_PER_CYCLE / licksPerCycle;
                this.cooldown = cooldown;

                Log.Debug("NAMELICK " + typeof(Def).FullName);
            }
        }

        new public class Instance : GameInstance
        {
            public Def Def { get; private set; }

            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
                Def = def;

                wait = Def.secondsPerLickMax;

                maxWait = Def.secondsPerLickMax - Def.cooldown;
            }

            private float maxWait;

            private float wait;

            public float NextWaitDuration()
            {
                float toNextPart = Def.secondsPerLickMax - wait;

                wait = UnityEngine.Random.Range(toNextPart, toNextPart + maxWait);
                return wait;
            }
        }

        private State wait;

        private State lick;
        private State cooldown;

        public override void InitializeStates(out BaseState defaultState)
        {
            defaultState = wait;

            wait
                .ScheduleGoTo(smi => smi.NextWaitDuration(), lick);

            lick
                .ToggleBehaviour(TAG, smi => true, smi => smi.GoTo(cooldown));

            cooldown
                .ScheduleGoTo(smi => smi.Def.cooldown, wait);
        }
    }
}