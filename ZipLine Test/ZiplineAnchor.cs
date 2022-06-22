using FUtility;
using System;
using System.Collections;
using UnityEngine;

namespace ZiplineTest
{
    public class ZiplineAnchor : KMonoBehaviour
    {
        [MyCmpReq]
        public NavTeleporter teleporter;

        private ZiplineAnchor target;
        //private ZiplineReactable reactable;

        private bool tetherOwner;

        [MyCmpReq]
        private Tether tether;

        public bool Connected => target != null;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Mod.ZipLines.Add(this);

            ZiplineTransitionLayer.ziplineLookup.Add(Grid.PosToCell(this), this);

            SetupNavTeleporter();

            //CreateNewReactable();
        }

        private void SetupNavTeleporter()
        {
            Log.Debuglog("SETUP");
            foreach(ZiplineAnchor zipLine in Mod.ZipLines)
            {
                var distance = Vector3.Distance(zipLine.transform.position, transform.position);

                if (zipLine != this && zipLine.target == null) // &&  < 30f)
                {
                    Log.Debuglog("FOUND STRAY LINE");
                    target = zipLine;
                    zipLine.target = this;

                    teleporter.TwoWayTarget(target.teleporter);
                    teleporter.EnableTwoWayTarget(true);

                    Log.Debuglog("SET UP TELEPORTER");

                    /*
                    var positions = new[]
                    {
                        transform.position,
                        target.transform.position
                    };

                    lineRenderer.positionCount = positions.Length;
                    lineRenderer.SetPositions(positions);
                    */

                    Log.Debuglog("distance" + distance);
                    tether.subDivisionCount = Mathf.CeilToInt((distance / tether.segmentLength) / 2.5f);
                    Log.Debuglog("subDivisionCount" + tether.subDivisionCount);

                    tether.SetEnds(transform, target.transform);
                    tetherOwner = true;

                    //tether.Settle(0);
                    StartCoroutine(TetherCoroutine());
                }
            }
        }

        private float tetherElapsed = 0;
        private float tetherSettleDuration = 6f;

        private IEnumerator TetherCoroutine()
        {
            while(tetherElapsed < tetherSettleDuration)
            {
                Log.Debuglog(tetherElapsed);
                tetherElapsed += Time.deltaTime;
                tether.Settle(Time.deltaTime * 2f);
                yield return new WaitForSeconds(0.066f);
            }

            yield return null;
        }

        public Vector3 GetPosition(float t, float z)
        {
            if (target == null)
            {
                return transform.position;
            }

            if (tetherOwner)
            {
                return tether.GetPosition(t, z);
            }
            else
            {
                return target.tether.GetPosition(1f - t, z);
            }
            //return Vector3.Lerp(transform.position, target.transform.position, dt);
        }

        /*
        private void CreateNewReactable()
        {
            if (reactable == null)
            {
                reactable = new ZiplineReactable(this);
            }
        }

        private void OrphanReactable()
        {
            reactable = null;
        }

        private void ClearReactable()
        {
            if (reactable != null)
            {
                reactable.Cleanup();
                reactable = null;
            }
        }

        private class ZiplineReactable : Reactable
        {
            private ZiplineAnchor checkpoint;
            private Navigator reactor_navigator;
            public const string ID = "ZiplineReactable";

            public ZiplineReactable(ZiplineAnchor checkpoint) : base(checkpoint.gameObject, ID, Db.Get().ChoreTypes.Checkpoint, 1, 1, false)
            {
                this.checkpoint = checkpoint;
                preventChoreInterruption = false;
            }


            public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
            {
                if (reactor != null)
                {
                    return false;
                }

                if (checkpoint == null)
                {
                    Cleanup();
                    return false;
                }

                if (!checkpoint.Connected)
                {
                    return false;
                }

                return transition.end == NavType.Teleport;
            }


            protected override void InternalBegin()
            {
                reactor_navigator = reactor.GetComponent<Navigator>();

                KBatchedAnimController component = reactor.GetComponent<KBatchedAnimController>();
                component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
                component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
                component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);

                checkpoint.OrphanReactable();
                checkpoint.CreateNewReactable();
            }


            public override void Update(float dt)
            {
                if (checkpoint == null || !checkpoint.Connected || reactor_navigator == null)
                {
                    Cleanup();
                    return;
                }

                reactor_navigator.AdvancePath(false);

                if (!reactor_navigator.path.IsValid())
                {
                    Cleanup();
                    return;
                }

                NavGrid.Transition nextTransition = reactor_navigator.GetNextTransition();

                if (nextTransition.end == NavType.Teleport)
                {
                    Cleanup();
                }
            }


            protected override void InternalEnd()
            {
                if (reactor != null)
                {
                    reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
                }
            }

            protected override void InternalCleanup()
            {
            }
        }
        */
    }
}
