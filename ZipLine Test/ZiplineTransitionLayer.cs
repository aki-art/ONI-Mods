using HarmonyLib;
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
                    transition.isLooping = true;
                    transition.preAnim = "fall_pre";
                    transition.anim = "fall_loop";

                    var targetPosition = navigator.NavGrid.teleportTransitions[Grid.PosToCell(navigator)];

                    //Grid.CellToXY(Grid.PosToCell(navigator), out var num2, out var num3);
                    //Grid.CellToXY(targetPosition, out var num5, out var num6);

                    // this can definitely be optimized later
                    durationSeconds = Vector3.Distance(entrance.transform.position, Grid.CellToPos(targetPosition)) / TILE_PER_SECOND;

                    isUsingZipline = true;
                    transition.speed = 1f;
                }
            }
        }

        public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
        {
            base.UpdateTransition(navigator, transition);

            if(!isUsingZipline)
            {
                return;
            }

            elapsedTime += Time.deltaTime;

            var t = Mathf.Clamp01(elapsedTime / durationSeconds);
            //navigator.transform.position = entrance.GetPosition(t, navigator.transform.position.z);
            navigator.GetComponent<KBatchedAnimController>().Offset = entrance.GetPosition(t, navigator.transform.position.z) - navigator.transform.position;
        }

        public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
        {
            base.EndTransition(navigator, transition);

            isUsingZipline = false;
            navigator.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
        }
    }
}
