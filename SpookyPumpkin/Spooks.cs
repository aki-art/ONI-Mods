using SpookyPumpkin.Settings;
using System;
using UnityEngine;

namespace SpookyPumpkin
{
    internal class Spooks : StateMachineComponent<Spooks.SMInstance>
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class States : GameStateMachine<States, SMInstance, Spooks>
        {

#pragma warning disable 649
            public State off;
            public State on;
            public State spooked;
#pragma warning restore

            Color orange = new Color(2, 1.5f, 0.3f, 2f);
            Color green = new Color(0.3f, 3f, 1f, 2f);

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;
                off
                    .Enter(smi => smi.ClearReactable())
                    .PlayAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
                on
                    .PlayAnim("on")
                    .Enter(smi =>
                    {
                        smi.CreateReactable();
                        smi.GetComponent<Light2D>().Color = orange;
                    })
                    .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational);
                spooked
                    .PlayAnim("spook", KAnim.PlayMode.Once)
                    .Enter(smi => smi.GetComponent<Light2D>().Color = green)
                    .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
                    .ScheduleGoTo(3f, on);
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, Spooks, object>.GameInstance
        {
            private Reactable reactable;

            public SMInstance(Spooks master) : base(master) { }

            public void CreateReactable()
            {
                if (reactable == null && SpookyTime())
                {
                    reactable = new EmoteReactable(
                        gameObject: gameObject,
                        id: "SP_Spooked",
                        chore_type: Db.Get().ChoreTypes.Emote,
                        animset: "anim_react_shock_kanim",
                        range_width: 3,
                        range_height: 2)
                    .AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "react",
                        startcb = Spook
                    });
                }
            }

            private void Spook(GameObject reactor)
            {
                if (!SpookyTime()) return;

                var kbac = reactor.GetComponent<KBatchedAnimController>();
                kbac.Queue("fall_pre");
                kbac.Queue("fall_loop");
                kbac.Queue("fall_loop");
                kbac.Queue("fall_pst");

                PlaySound(GlobalAssets.GetSound("dupvoc_03_voice_wailing"));
                PlaySound(GlobalAssets.GetSound("dupvoc_02_voice_destructive_enraging"));

                reactor.GetComponent<Klei.AI.Effects>().Add(ModAssets.spookedEffectID, true);

                smi.GoTo(smi.sm.spooked);
            }

            public void ClearReactable()
            {
                if (reactable != null)
                {
                    reactable.Cleanup();
                    reactable = null;
                }
            }

            private bool IsItOctober()
            {
                var date = System.DateTime.Now;
                return date.Month == 10 || (date.Month == 11 && date.Day <= 1);
            }

            private bool SpookyTime()
            {
                return
                    ModSettings.Settings.Spooks == UserSettings.SpooksSetting.Always ||
                    (ModSettings.Settings.Spooks == UserSettings.SpooksSetting.October && IsItOctober());
            }
        }
    }
}