/*using KSerialization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ResearchTypes;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DesktopPip : StateMachineComponent<DesktopPip.SMInstance>
    {
        [MyCmpReq]
        public KBatchedAnimController kbac;

        [SerializeField]
        public float animScale = 2f;

        [SerializeField]
        public Transform blocker;


        public override void OnSpawn()
        {
            AETEScreenPipmanager.pips.Add(this);
            kbac.SetSymbolVisiblity("snapto_pivot", false);
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
            public State falling;
            public State fallingPst;
            public State sliding;
            public State slidingPst;
            public State occupying;
            public State occupyingAlt;
            public State leaving;
            public State leavingPst;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = movingToTarget;

                root
                    .Update((smi, dt) => smi.lifeTime += dt);

                movingToTarget
                    .Enter(UpdateAnimation)
                    .PlayAnim("floor_floor_1_0_loop", KAnim.PlayMode.Loop)
                    .UpdateTransition(occupying, OnMove, UpdateRate.SIM_33ms);

                occupying
                    .PlayAnim("idle_loop", KAnim.PlayMode.Loop)
                    .EventHandler(ModEvents.OnScreenResize, OnScreenResizedWhileSitting)
                    .Transition(occupyingAlt, _ => Random.value < 0.01f)
                    .Transition(leaving, smi => smi.lifeTime > smi.minimumLife && (smi.lifeTime > smi.maximumLife || Random.value < 0.002f));

                occupyingAlt
                    .PlayAnim("queue_loop")
                    .EventHandler(ModEvents.OnScreenResize, OnScreenResizedWhileSitting)
                    .OnAnimQueueComplete(occupying);

                leaving
                    .Enter(smi => smi.transform.parent.position += Vector3.down)
                    .PlayAnim("growup_pst")
                    .OnAnimQueueComplete(leavingPst);

                leavingPst
                    .Enter(smi => Util.KDestroyGameObject(smi.master.gameObject));
            }

            private void OnScreenResizedWhileSitting(SMInstance smi)
            {
                smi.SetPosition(smi.targetPosition);
            }

            private bool OnMove(SMInstance smi, float dt)
            {
                var nextPosition = Vector2.MoveTowards(
                    smi.transform.parent.position,
                    smi.targetPosition,
                    dt * smi.speed);

                smi.SetPosition(nextPosition);
                if(smi.targetMarker != null)
                {
                    smi.targetMarker.transform.position = smi.targetPosition;
                }

                return Vector2.Distance(smi.transform.parent.position, smi.targetPosition) < 0.1f;
            }


            private void UpdateAnimation(SMInstance smi)
            {
                smi.kbac.flipY = !smi.floor;
                smi.kbac.flipX = smi.transform.parent.position.x > smi.targetPosition.x;
            }
        }

        public void UpdatePositionAndTarget()
        {
            var nodes = AETEScreenPipmanager.Instance.floorNodes; // TODO, temp
            if(nodes.Count <= smi.targetPositionIdx)
            {
                smi.targetPositionIdx = nodes.Count - 1;
            }

            smi.targetPosition = nodes[smi.targetPositionIdx];
            smi.master.blocker.localScale = new Vector3(0.6f, 1.5f);
            smi.PutToFloor();
        }

        public void PickTarget(List<Vector3> nodes, bool floor)
        {
            smi.floor = floor;
            var index = Random.Range(0, nodes.Count);
            smi.SetFinalTarget(nodes[index], index);
        }

        public class SMInstance : GameStateMachine<States, SMInstance, DesktopPip, object>.GameInstance
        {
            public KBatchedAnimController kbac;
            public Image targetMarker;
            public float speed = 70f;
            public Vector3 targetPosition;
            public int targetPositionIdx;
            public bool floor;
            public float lifeTime;
            public Vector3 animOffset;
            public float minimumLife = 30f;
            public float maximumLife = 90f;

            public SMInstance(DesktopPip master) : base(master)
            {
                kbac = master.GetComponent<KBatchedAnimController>();

                animOffset = (kbac.GetTransformMatrix() * kbac.GetSymbolLocalTransform("snapto_pivot", out _))
                    .MultiplyPoint(Vector3.zero) - transform.GetPosition();

                kbac.offset = new Vector3(0, -10f);

                if (AETEScreenPipmanager.Instance.debugMode)
                {
                    targetMarker = MarkImage(smi.targetPosition);
                }
            }

            public Image MarkImage(Vector3 position)
            {
                var go = new GameObject();
                go.transform.position = position;
                go.transform.SetParent(smi.transform.parent);
                go.SetActive(true);

                var image = go.AddComponent<Image>();
                image.color = new Color(0, 0, 1, 0.4f);
                image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector3.zero);

                go.transform.localScale = new Vector3(0.5f, 0.5f);
                return image;
            }

            public void PutToFloor()
            {
                smi.master.transform.parent.position = new Vector3(
                    smi.master.transform.parent.position.x,
                    0,
                    smi.master.transform.parent.position.z);
            }

            public void SetPosition(Vector3 position)
            {
                smi.master.transform.parent.position = position;
                if(smi.master.blocker != null)
                {
                    smi.master.blocker.position = position;
                }
            }

            public void SetFinalTarget(Vector3 position, int idx = -1)
            {
                targetPosition = position;
                targetPositionIdx = idx;
                floor = targetPosition.y <= 100;
                var x = position.x > Screen.width / 2f ? -100f : Screen.width + 100f;
                SetPosition(new Vector3(x, targetPosition.y, targetPosition.z - 0.1f));
            }
        }
    }
}
*/