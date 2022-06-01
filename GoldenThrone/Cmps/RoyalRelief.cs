using UnityEngine;

namespace GoldenThrone.Cmps
{
    public class RoyalRelief : GameStateMachine<RoyalRelief, RoyalRelief.Instance, IStateMachineTarget, RoyalRelief.Def>
    {
        public State idle;
        public State relieving;
        public State relieved;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;

            root
                .EventTransition((GameHashes)ModHashes.BeganUsingGoldToilet, relieving);

            idle
                .DoNothing();

            relieving
                .ToggleEffect(ModAssets.Effects.TooComfortable)
                .EventTransition((GameHashes)ModHashes.FinishedUsingGoldToilet, relieved);

            relieved
                .Enter(AddVisuals)
                .Exit(RemoveVisuals)

                .ToggleEffect(ModAssets.Effects.RoyallyRelievedID)
                .ToggleExpression(ModAssets.relievedExpression)

                .ScheduleGoTo(Mod.Settings.RoyallyRelievedDurationInSeconds, idle);
        }

        private void RemoveVisuals(Instance smi)
        {
            if (Mod.Settings.CustomCrown)
            {
                smi.snapOn.DetachSnapOnByName(smi.kanimName);

                // restore original hat
                var resume = smi.GetComponent<MinionResume>();
                MinionResume.ApplyHat(resume.CurrentHat, smi.GetComponent<KBatchedAnimController>());
            }

            StopSparkle(smi);
        }

        private void AddVisuals(Instance smi)
        {
            if (Mod.Settings.CustomCrown)
            {
                smi.snapOn.AttachSnapOnByName(smi.kanimName);
            }

            BeginSparkle(smi);
        }

        private void BeginSparkle(Instance smi)
        {
            if(!Mod.Settings.UseParticles)
            {
                return;
            }

            smi.goldSparklesFX = Object.Instantiate(ModAssets.Prefabs.goldSparkleParticles);
            smi.goldSparklesFX.transform.position = smi.master.transform.GetPosition() + smi.offset;
            smi.goldSparklesFX.transform.SetParent(smi.master.transform);
            smi.goldSparklesFX.SetActive(true);
        }

        private void StopSparkle(Instance smi)
        {
            if (!Mod.Settings.UseParticles)
            {
                return;
            }

            Object.Destroy(smi.goldSparklesFX);
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public SnapOn snapOn;
            public string kanimName = "GoldenThrone_Crown";
            public GameObject goldSparklesFX;
            public Vector3 offset = new Vector3(0f, 1f, 0.1f);

            public Instance(IStateMachineTarget master) : base(master)
            {
                snapOn = master.GetComponent<SnapOn>();
            }
        }
    }
}