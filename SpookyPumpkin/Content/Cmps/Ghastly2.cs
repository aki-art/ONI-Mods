using Klei.AI;
using SpookyPumpkinSO.Content;
using UnityEngine;

namespace GoldenThrone.Cmps
{
    public class Ghastly2 : GameStateMachine<Ghastly2, Ghastly2.Instance, IStateMachineTarget, Ghastly2.Def>
    {
        public State idle;
        public PumpkinedStates pumpkined;

        public class PumpkinedStates : State
        {
            public State day;
            public State night;
        }

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;

            idle
                .EventHandlerTransition(GameHashes.EffectAdded, pumpkined.day, IsPumpkinSpiced);

            pumpkined
                .DefaultState(pumpkined.day)
                .EventHandlerTransition(GameHashes.EffectRemoved, idle, (smi, data) => !IsPumpkinSpiced(smi, data));

            pumpkined.day
                .EnterTransition(pumpkined.night, smi => GameClock.Instance.IsNighttime())
                .EventTransition(GameHashes.Nighttime, pumpkined.night);

            pumpkined.night
                .Enter(AddVisuals)
                .Exit(RemoveVisuals)
                .EventTransition(GameHashes.NewDay, pumpkined);
        }

        private bool IsPumpkinSpiced(Instance smi, object data)
        {
            return data is Effect effect && effect.Id == SPSpices.PUMPKIN_SPICE_ID;
        }

        private void RemoveVisuals(Instance smi)
        {
            smi.kbac.TintColour = Color.white;
            smi.kbac.HighlightColour = Color.white;
        }

        private void AddVisuals(Instance smi)
        {
            smi.kbac.TintColour = smi.def.transparentTint;
            smi.kbac.HighlightColour = smi.def.ghostlyColor;
        }

        public class Def : BaseDef
        {
            public Color ghostlyColor = new Color(0, 24, 30);
            public Color transparentTint = new Color(1f, 1f, 1f, 0.4f);
        }

        public new class Instance : GameInstance
        {
            public KBatchedAnimController kbac;

            public Instance(IStateMachineTarget master) : base(master)
            {
                kbac = master.GetComponent<KBatchedAnimController>();
            }
        }
    }
}