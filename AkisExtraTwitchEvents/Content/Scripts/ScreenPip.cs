// WIP of a more complex pip that can vome on any walls
// for now using DesktopPip

/*using FUtility;
using KSerialization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ScreenPip : StateMachineComponent<ScreenPip.SMInstance>
    {
        public override void OnSpawn()
        {
            smi.StartSM();
        }

        public void SetPosition(int index)
        {
            transform.parent.position = AETEScreenPipmanager.Instance.nodes[index];
            smi.currentNodeIdx = index;
        }

        public class States : GameStateMachine<States, SMInstance, ScreenPip>
        {
            public State idle;
            public State moving;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;

                idle
                    .Enter(smi =>
                    {
                        if (smi.currentNodeIdx == -1)
                        {
                            smi.currentNodeIdx = Random.Range(0, AETEScreenPipmanager.Instance.nodes.Count);
                            smi.transform.parent.position = AETEScreenPipmanager.Instance.nodes[smi.currentNodeIdx];
                        }

                    })
                    .UpdateTransition(moving, DecidedToMove, UpdateRate.RENDER_1000ms)
                    .PlayAnim("idle_loop");

                moving
                    .PlayAnim("floor_floor_1_0_loop", KAnim.PlayMode.Loop)
                    .UpdateTransition(idle, OnMove2, UpdateRate.SIM_33ms);
            }

            private bool DecidedToMove(SMInstance smi, float _)
            {
                if (Random.value < 0.1f)
                {
                    smi.finalTargetNodeIdx = GetNextTargetIndex(smi.currentNodeIdx);
                    UpdateNextPosition(smi);

                    Log.Debuglog($"targeting {smi.nextNodeIdx}");
                    Log.Debuglog($"currentl at {smi.currentNodeIdx}");
                    return true;
                }

                return false;
            }

            private void UpdateNextPosition(SMInstance smi)
            {
                smi.nextNodeIdx = NextPosition(smi.currentNodeIdx, smi.finalTargetNodeIdx);
                smi.nextPosition = AETEScreenPipmanager.Instance.nodes[smi.nextNodeIdx];
                smi.targetMarker.transform.position = AETEScreenPipmanager.Instance.nodes[smi.finalTargetNodeIdx];

                UpdateAnimation();
            }

            private void UpdateAnimation()
            {
                //var pos = 
            }

            private int GetNextTargetIndex(int currentIdx)
            {
                var count = AETEScreenPipmanager.Instance.nodes.Count;

                var result = Random.Range(0, count);
                if(result == currentIdx)
                {
                    result++;
                    if (result >= count)
                    {
                        result = 0;
                    }
                }

                return result;
            }

            private int NextPosition(int startIndex, int destinationIndex)
            {
                if (startIndex == destinationIndex)
                {
                    return -1;
                }

                var nodes = AETEScreenPipmanager.Instance.nodes;
                var innerDist = Mathf.Abs(startIndex - destinationIndex);

                if(innerDist == 1)
                {
                    return destinationIndex;
                }

                var outerDist = nodes.Count - innerDist;
                int result;

                if (innerDist >= outerDist)
                {
                    result = startIndex > destinationIndex ? startIndex - 1 : destinationIndex + 1;
                }
                else
                {
                    result = startIndex > destinationIndex ? startIndex + 1 : destinationIndex - 1;
                }

                var nodeCount = nodes.Count;

                if (result < 0)
                {
                    result = nodeCount - 1;
                }
                else if(result >= nodeCount)
                {
                    result = 0;
                }

                return result;
            }

            private bool OnMove2(SMInstance smi, float dt)
            {
                if(smi.nextNodeIdx == -1)
                {
                    Log.Debuglog("invalid destination");
                    return true;
                }

                var arrived = Vector3.Distance(smi.transform.parent.position, smi.nextPosition) < 1;
                Log.Debuglog("distance " + Vector3.Distance(smi.transform.parent.position, smi.nextPosition));
                
                if (arrived)
                {
                    smi.currentNodeIdx = smi.nextNodeIdx;

                    if(smi.currentNodeIdx == smi.finalTargetNodeIdx)
                    {
                        Log.Debuglog($"arrived");
                        // arrived
                        return true;
                    }

                    UpdateNextPosition(smi);

                }

                Log.Debuglog($"---");
                Log.Debuglog($"- targeting {smi.nextNodeIdx}");
                Log.Debuglog($"- currently at {smi.currentNodeIdx} {smi.transform.parent.position}");
                Log.Debuglog($"- trying to go to {smi.finalTargetNodeIdx} {AETEScreenPipmanager.Instance.nodes[smi.nextNodeIdx]}");

                var nextPosition = Vector3.MoveTowards(
                    smi.transform.parent.position,
                    AETEScreenPipmanager.Instance.nodes[smi.nextNodeIdx],
                    dt * smi.speed);


                smi.transform.parent.position = nextPosition;


                return false;
            }

            private void UpdateAnimation(SMInstance smi, Vector3 nextPos)
            {
                var velocity = smi.transform.parent.position - nextPos;

                var movingHorizontally = velocity.x > 0;

                if(movingHorizontally)
                {
                    var movingLeft = velocity.x < 0;
                    var OnFloor = smi.transform.parent.position.y == 0;
                    smi.kbac.flipX = movingLeft;
                    smi.kbac.Rotation = OnFloor ? 0 : 180;
                }
                else
                {
                    var movingUp = velocity.y > 0;
                    var onLeftEdge = smi.transform.parent.position.x == 0;

                    smi.kbac.Rotation = onLeftEdge ? 90 : 270;
                    smi.kbac.flipX = movingUp;
                }
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, ScreenPip, object>.GameInstance
        {
            public KBatchedAnimController kbac;
            public Vector3 nextPosition = Vector3.zero;
            public Image targetMarker;
            public float speed = 70f;
            public int currentNodeIdx = -1;
            public int finalTargetNodeIdx = -1;
            public int nextNodeIdx = -1;

            public SMInstance(ScreenPip master) : base(master)
            {
                kbac = master.GetComponent<KBatchedAnimController>();

#if DEBUG
                targetMarker = MarkImage(smi.nextPosition);
#endif
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
        }
    }
}
*/