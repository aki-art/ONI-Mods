using FUtility;
using System;
using Terraformer.Entities;
using UnityEngine;

namespace Terraformer.Screens
{
    public class DestroyablePlanetSidescreen : SideScreenContent
    {
        private LocText label;
        private KButton button;
        private InstantDestroyablePlanet target;

        public override bool IsValidForTarget(GameObject target)
        {
            return target is object && target.GetComponent<InstantDestroyablePlanet>() is object;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";

            Transform contents = transform.Find("Contents");
            label = contents.Find("Label")?.GetComponent<LocText>();
            button = contents.Find("Button")?.GetComponent<KButton>();

            label.alignment = TMPro.TextAlignmentOptions.TopLeft;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            button.onClick += OnButtonClick;
            Refresh();
        }

        private void OnButtonClick()
        {
            WorldDestroyer destroyer = Utils.Spawn(WorldDestroyerConfig.ID, target.worldContainer.minimumBounds).GetComponent<WorldDestroyer>();
            destroyer.onWorldDestroyed += SpawnRandomPOI;
            /*
            destroyer.onWorldDestroyed += axiall =>
            {
                new GameObject().AddComponent<Terraformer>().CreateNewWorld(axiall);
            };*/

            destroyer.Detonate(true);
        }

        private void SpawnRandomPOI(AxialI loc)
        {
            string id = SpacePOIOptions.GetRandom();
            Log.Debuglog("chosen poi: {id}");

            var poi = Assets.GetPrefab(id);
            if(poi is null)
            {
                Log.Debuglog("but its null :(");
            }

            GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(id));
            gameObject.GetComponent<ClusterGridEntity>().Location = loc;
            gameObject.GetComponent<HarvestablePOIClusterGridEntity>().Init(loc);
            gameObject.SetActive(true);
        }

        public override void SetTarget(GameObject target)
        {
            InstantDestroyablePlanet component = target?.GetComponent<InstantDestroyablePlanet>();

            if (component is null)
            {
                Log.Error("Target doesn't have a ISidescreenButtonWithTextControl associated with it.");
                return;
            }

            this.target = component;
            //target.Subscribe((int)ModHashes.SidescreenRefresh, obj => Refresh());
            Refresh();
        }

        private void Refresh()
        {
            if (label is null || target is null)
            {
                return;
            }

            button.isInteractable = target.IsWorldTargetable();
            label.SetText(target.GetText());
        }
    }
}
