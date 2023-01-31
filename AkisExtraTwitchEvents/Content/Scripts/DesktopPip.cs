using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DesktopPip : StateMachineComponent<DesktopPip.SMInstance>
    {
        public override void OnSpawn()
        {
            AETEScreenPipmanager.pips.Add(this);
            smi.StartSM();
        }

        public override void OnCleanUp()
        {
            AETEScreenPipmanager.pips.Remove(this);
            base.OnCleanUp();
        }

        public class States : GameStateMachine<States, SMInstance, DesktopPip>
        {
            public State movingToTarget;
            public State occupying;
            public State leaving;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = movingToTarget;

                movingToTarget
                    .Enter(UpdateAnimation)
                    .PlayAnim("floor_floor_1_0_loop", KAnim.PlayMode.Loop)
                    .UpdateTransition(occupying, OnMove, UpdateRate.SIM_33ms);

                occupying
                    .PlayAnim("idle_loop", KAnim.PlayMode.Loop)
                    .ScheduleGoTo(30f, leaving);

                leaving
                    .Enter(smi => smi.kbac.offset = Vector3.down)
                    .PlayAnim("growup_pst")
                    .Exit(smi => Util.KDestroyGameObject(smi.master.gameObject));
            }

            private bool OnMove(SMInstance smi, float dt)
            {
                var nextPosition = Vector3.MoveTowards(
                    smi.transform.parent.position, 
                    smi.targetPosition,
                    dt * smi.speed);

                smi.transform.parent.position = nextPosition;

                return Vector3.Distance(smi.transform.parent.position, smi.targetPosition) < 0.1f;
            }

            private void UpdateAnimation(SMInstance smi)
            {
                smi.kbac.flipY = !smi.floor;
                smi.kbac.flipX = smi.transform.parent.position.x > smi.targetPosition.x;
            }
        }

        public void PickTarget(List<Vector3> nodes, bool floor)
        {
            smi.floor = floor;
            smi.SetFinalTarget(nodes[Random.Range(0, nodes.Count)]);
        }

        public class SMInstance : GameStateMachine<States, SMInstance, DesktopPip, object>.GameInstance
        {
            public KBatchedAnimController kbac;
            public Image targetMarker;
            public float speed = 70f;
            public Vector3 targetPosition;
            public bool floor;

            public SMInstance(DesktopPip master) : base(master)
            {
                kbac = master.GetComponent<KBatchedAnimController>();
                targetMarker = MarkImage(smi.targetPosition);
            }

            public Image MarkImage(Vector3 position)
            {
                var go = new GameObject();
                go.transform.position = position;
                go.transform.SetParent(smi.transform);
                go.SetActive(true);

                var image = go.AddComponent<Image>();
                image.color = new Color(0, 0, 1, 0.4f);
                image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector3.zero);

                go.transform.localScale = new Vector3(0.5f, 0.5f);
                return image;
            }

            public void SetFinalTarget(Vector3 position)
            {
                targetPosition = position;
                floor = targetPosition.y <= 100;
                var x = position.x > Screen.width / 2f ? -100f : Screen.width + 100f;
                smi.master.transform.parent.position = new Vector3(x, targetPosition.y);
            }
        }
    }
}
