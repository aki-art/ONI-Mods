using FUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AETNTweaks.Buildings.PyrositePylon
{
    public class TetherPlaceVisualizer : KMonoBehaviour, ISim200ms
    {
        [SerializeField]
        public Vector3 attachAnchorOffset = new Vector3(0.5f, 3.5f);

        [SerializeField]
        public Vector3 centerOffset = new Vector3(0.5f, 2f);

        private Transform anchor;
        private GameObject closestAETN;

        private List<GameObject> possibleAETNs;

        private bool hasAETN;
        private float sqrMaxDistance;

        [MyCmpReq]
        private Tether tether;

        [MyCmpReq]
        private LineRenderer lineRenderer;

        private Extents extents;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            sqrMaxDistance = Mod.Settings.PyrositeAttachRadius * Mod.Settings.PyrositeAttachRadius;

            var go = new GameObject("AETN_Tether_Anchor");
            anchor = go.transform;

            go.SetActive(true);

            possibleAETNs = new List<GameObject>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            extents = new Extents(this.NaturalBuildingCell(), Mod.Settings.PyrositeAttachRadius);
            hasAETN = false;

            foreach (var controller in Mod.AETNs.GetWorldItems(this.GetMyWorldId()))
            {
                possibleAETNs.Add(controller.gameObject);
            }

            StartCoroutine(Render33ms());
        }

        private IEnumerator Render33ms()
        {
            while(true)
            {
                if (hasAETN)
                {
                    tether.Settle(Time.deltaTime);
                }

                yield return new WaitForSecondsRealtime(0.033f);
            }
        }

        void SetAETN(GameObject aetn)
        {
            Log.Debuglog("SET AETN");

            if(aetn == closestAETN)
            {
                return;
            }

            if(closestAETN != null)
            {
                RemoveAETN();
            }

            anchor.position = aetn.transform.position + centerOffset;

            tether.enabled = true;
            lineRenderer.enabled = true;

            tether.SetEnds(anchor, transform, false);

            closestAETN = aetn;
            hasAETN = true;
        }

        bool IsWithinRange(Transform target)
        {
            var dist = (transform.position - (target.position + centerOffset)).sqrMagnitude;
            return dist <= sqrMaxDistance;
        }

        void RemoveAETN()
        {
            Log.Debuglog("REMOVED AETN");
            hasAETN = false;
            closestAETN = null;

            tether.enabled = false;
            lineRenderer.enabled = false;
        }

        public void Sim200ms(float dt)
        {
            if(hasAETN)
            {
                if(!IsWithinRange(closestAETN.transform))
                {
                    RemoveAETN();
                }
            }

            if(!hasAETN)
            {
                var closestDistance = float.MaxValue;
                GameObject closestController = null;

                foreach(MassiveHeatSink controller in Mod.AETNs.GetWorldItems(this.GetMyWorldId()))
                {
                    if(controller == null) continue;

                    var dist = (transform.position - (controller.transform.position + centerOffset)).sqrMagnitude;
                    if(dist <= sqrMaxDistance && dist <= closestDistance)
                    {
                        closestDistance = dist;
                        closestController = controller.gameObject;
                    }
                }

                if(closestController != null)
                {
                    SetAETN(closestController);
                }
            }
        }

        protected override void OnCleanUp()
        {
            StopCoroutine(Render33ms());
            Destroy(anchor.gameObject);
            base.OnCleanUp();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(200, 450, 400, 500));


            if(hasAETN)
            {
                var dist = (transform.position - closestAETN.transform.position).sqrMagnitude;
                GUILayout.Label($"ATTACHED AETN: {dist}");
            }
            else
            {
                GUILayout.Label($"no aetn nearby");
            }

            GUILayout.Space(10);

            foreach (MassiveHeatSink aetn in Mod.AETNs)
            {
                var dist = (transform.position - aetn.transform.position).sqrMagnitude;
                GUILayout.Label($"{aetn.transform.position} \t\t {dist}");
            }

            GUILayout.EndArea();
        }
    }
}
