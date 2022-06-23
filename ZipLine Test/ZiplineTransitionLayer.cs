using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZiplineTest
{
    internal class ZiplineTransitionLayer : TransitionDriver.OverrideLayer
    {
        private const float TILE_PER_SECOND = 8f; // constant for now. maybe it could respect the incline of the rope

        public static Dictionary<int, ZiplineAnchor> ziplineLookup = new Dictionary<int, ZiplineAnchor>();

        private bool isUsingZipline = false;
        private ZiplineAnchor entrance;
        private float elapsedTime = 0;
        private float durationSeconds;
        private Direction direction;
        private float targetX;

        [HarmonyPatch(typeof(MinionConfig), "OnSpawn")]
        public class MinionConfig_OnSpawn_Patch
        {
            public static void Postfix(GameObject go)
            {
                var navigator = go.GetComponent<Navigator>();
                navigator.transitionDriver.overrideLayers.Add(new ZiplineTransitionLayer(navigator));
            }
        }

        public ZiplineTransitionLayer(Navigator navigator) : base(navigator)
        {
        }

        public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
        {
            base.BeginTransition(navigator, transition);

            if(transition.start == NavType.Teleport)
            {
                if (ziplineLookup.TryGetValue(Grid.PosToCell(navigator), out entrance))
                {
                    elapsedTime = 0;
                    transition.isLooping = false;
                    transition.anim = null;//"fall_loop";
                    transition.isCompleteCB = () => IsTransitionComplete(navigator);

                    // todo: vertical only lines?
                    targetX = entrance.target.Position.x;
                    direction = entrance.Position.x < targetX ? Direction.Right : Direction.Left;

                    var targetPosition = navigator.NavGrid.teleportTransitions[Grid.PosToCell(navigator)];

                    //Grid.CellToXY(Grid.PosToCell(navigator), out var num2, out var num3);
                    //Grid.CellToXY(targetPosition, out var num5, out var num6);

                    // this can definitely be optimized later
                    durationSeconds = Vector3.Distance(entrance.transform.position, Grid.CellToPos(targetPosition)) / TILE_PER_SECOND;

                    isUsingZipline = true;
                    transition.speed = 1f;

                    var kbac = navigator.GetComponent<KBatchedAnimController>();
                    kbac.ClearQueue();
                    kbac.Play("fall_pre", KAnim.PlayMode.Once);
                    kbac.Queue("fall_loop", KAnim.PlayMode.Loop);
                }
            }
        }

        private bool IsTransitionComplete(Navigator navigator)
        {
            Log.Debuglog("CHECKING TRANSITION");

            var x = navigator.transform.position.x;
            Log.Debuglog($"DUPE: {x} TARGET: {targetX} DIRECTION: {direction}");
            return direction == Direction.Right ? targetX <= x : targetX >= x;
        }

        public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
        {
            base.UpdateTransition(navigator, transition);

            if(!isUsingZipline)
            {
                return;
            }

            elapsedTime += Time.deltaTime;

            var position = navigator.transform.GetPosition();
            var cell1 = Grid.PosToCell(position);

            var t = Mathf.Clamp01(elapsedTime / durationSeconds);
            //navigator.transform.position = entrance.GetPosition(t, navigator.transform.position.z);
            navigator.transform.SetPosition(entrance.GetPosition(t, position.z));

            if(cell1 != Grid.PosToCell(position))
            {
                navigator.Trigger((int)GameHashes.NavigationCellChanged);
            }
        }

        public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
        {
            base.EndTransition(navigator, transition);

            isUsingZipline = false;
            navigator.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
        }
    }
}
