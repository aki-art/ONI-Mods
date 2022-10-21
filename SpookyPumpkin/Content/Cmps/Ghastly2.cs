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
#if DEBUG
            public State debug;
#endif
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
                .ToggleStatusItem(SPStatusItems.ghastlyLitBonus)
                .UpdateTransition(pumpkined.day, (smi, dt) => !IsNightTime());
#if DEBUG
            pumpkined.debug
                .Enter(OnNightFall)
                .Exit(RemoveVisuals)
                .ToggleEffect(SPEffects.GHASTLY)
                .ToggleStatusItem(SPStatusItems.ghastlyLitBonus);
#endif
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
            Log.Debuglog("trying to apply highlight" + go.name);
            var ghastly = go.GetSMI<Instance>();
            if (ghastly != null && ghastly.IsGhastly())
            {
                Log.Debuglog("valid");
                ghastly.kbac.HighlightColour += new Color(ModAssets.Colors.ghostlyColor.r + value, ModAssets.Colors.ghostlyColor.g + value, ModAssets.Colors.ghostlyColor.b + value);
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
            smi.kbac.TintColour = ModAssets.Colors.transparentTint;
            smi.kbac.HighlightColour = ModAssets.Colors.ghostlyColor;
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public KBatchedAnimController kbac;
            public Effects effects;
            public KSelectable kSelectable;
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
#if DEBUG
                if(smi.IsInsideState(smi.sm.pumpkined.debug))
                {
                    return true;
                }
#endif
                return smi.IsInsideState(smi.sm.pumpkined.night);
            }


            public float UpdateEfficiencyBonus(float result, float minimumMultiplier)
            {
                return effects.HasEffect(SPEffects.GHASTLY) ? Math.Max(result + Mod.Config.GhastlyWorkBonus, minimumMultiplier) : result;
            }
        }
    }
}
