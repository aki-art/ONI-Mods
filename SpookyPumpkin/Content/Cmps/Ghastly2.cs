using FUtility;
using Klei.AI;
using System;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Cmps
{
    public class Ghastly2 : GameStateMachine<Ghastly2, Ghastly2.Instance, IStateMachineTarget, Ghastly2.Def>
    {
        public State idle;
        public PumpkinedStates pumpkined;
        public Signal spiceEatenSignal;

        public class PumpkinedStates : State
        {
            public State day;
            public State night;
        }

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;

            idle
                .EnterTransition(pumpkined, HasPumpkinEffect)
                .OnSignal(spiceEatenSignal, pumpkined);
                //.EventHandlerTransition(GameHashes.EffectAdded, pumpkined.day, IsPumpkinSpice);

            pumpkined
                .DefaultState(pumpkined.day)
                .EventHandlerTransition(GameHashes.EffectRemoved, idle, IsPumpkinSpice);

            pumpkined.day
                .UpdateTransition(pumpkined.night, (smi, dt) => IsNightTime());

            pumpkined.night
                .Enter(OnNightFall)
                .Exit(RemoveVisuals)
                .ToggleEffect(SPEffects.GHASTLY)
                .UpdateTransition(pumpkined.day, (smi, dt) => !IsNightTime());
        }

        private void OnNightFall(Instance smi)
        {
            AddVisuals(smi);
            smi.effects.Add(SPEffects.GHASTLY, true);
        }

        private bool HasPumpkinEffect(Instance smi)
        {
            return smi.effects.HasEffect(SPEffects.GHASTLY);
        }

        public static void TryApplyHighlight(GameObject go, float value)
        {
            var ghastly = go.GetSMI<Instance>();
            if (ghastly != null && ghastly.IsGhastly())
            {
                ghastly.kbac.HighlightColour += new Color(ghastly.ghostlyColor.r + value, ghastly.ghostlyColor.g + value, ghastly.ghostlyColor.b + value);
            }
        }

        private bool IsNightTime()
        {
            return GameClock.Instance.IsNighttime();
        }

        private bool IsPumpkinSpice(Instance _, object data)
        {
            Log.Debuglog("SPICE CHECK " + ((Effect)data).Id);
            return data is Effect effect && effect.Id == SPSpices.PUMPKIN_SPICE_ID;
        }

        private void RemoveVisuals(Instance smi)
        {
            smi.kbac.TintColour = Color.white;
            smi.kbac.HighlightColour = Color.black;
        }

        private void AddVisuals(Instance smi)
        {
            smi.kbac.TintColour = smi.transparentTint;
            smi.kbac.HighlightColour = smi.ghostlyColor;
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public KBatchedAnimController kbac;
            public Effects effects;
            public KSelectable kSelectable;
            public Color ghostlyColor = new Color(0, 24, 30);
            public Color transparentTint = new Color(1f, 1f, 1f, 0.4f);
            private Guid statusHandle;

            public Instance(IStateMachineTarget master) : base(master)
            {
                kbac = master.GetComponent<KBatchedAnimController>();
                effects = master.GetComponent<Effects>();
                kSelectable = master.GetComponent<KSelectable>();
            }

            internal void OnPumpkinSpiceConsumed()
            {
                smi.sm.spiceEatenSignal.Trigger(smi);
            }

            public bool IsGhastly()
            {
                return smi.IsInsideState(smi.sm.pumpkined.night);
            }


            public float UpdateEfficiencyBonus(float result, float minimumMultiplier)
            {
                if (effects.HasEffect(SPEffects.GHASTLY))
                {
                    statusHandle = kSelectable.AddStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
                    return Math.Max(result + Mod.Config.GhastlyWorkBonus, minimumMultiplier);
                }
                else
                {
                    return result;
                }
            }
        }
    }
}