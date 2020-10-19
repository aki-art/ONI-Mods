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
            public State off;
            public State on;
            public State spooked;

            Color orange = new Color(2, 1.5f, 0.3f, 1.5f);
            Color green = new Color(0.3f, 3f, 1f, 1.5f);

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
                    .ScheduleGoTo(3f, on)
                    .Exit(smi => smi.ClearReactable());
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, Spooks, object>.GameInstance
        {
            private Reactable reactable;
            public SMInstance(Spooks master) : base(master) { }

            public void CreateReactable()
            {
                if (reactable == null)
                {
                    reactable = new EmoteReactable(
                        gameObject: gameObject,
                        id: "SP_Spooked",
                        chore_type: Db.Get().ChoreTypes.Emote,
                        animset: "anim_react_shock_kanim",
                        range_width: 2,
                        range_height: 2,
                        min_reactable_time: 10f,
                        min_reactor_time: 10f)
                    .AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "react",
                        startcb = Spook
                    });

                }
            }

            private void Spook(GameObject reactor)
            {
                var kbac = reactor.GetComponent<KBatchedAnimController>();
                kbac.Queue("fall_pre");
                kbac.Queue("fall_loop");
                kbac.Queue("fall_loop");
                kbac.Queue("fall_pst");

                reactor.GetComponent<Klei.AI.Effects>().Add(ModAssets.spooked, true);
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
        }
    }
}