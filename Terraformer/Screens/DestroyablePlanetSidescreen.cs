using FUtility;
using FUtility.FUI;
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
            Helper.OpenFDialog<DetonationDialog>(ModAssets.Prefabs.detonationSelectorScreen, "DetonationSelectorScreen");
            //InitiateDetonation();
        }

        private void InitiateDetonation()
        {
            WorldDestroyer destroyer = Utils.Spawn(WorldDestroyerConfig.ID, target.worldContainer.minimumBounds).GetComponent<WorldDestroyer>();
            //destroyer.onWorldDestroyed += SpawnRandomPOI;
            destroyer.onWorldDestroyed += axial => Terraform(destroyer.GetMyWorld());
            destroyer.onWorldCleared += Terraform;
            /*
            destroyer.onWorldDestroyed += axiall =>
            {
                Log.Debuglog("before", Grid.WidthInCells, Grid.HeightInCells);
                //new GameObject().AddComponent<Terraformer>().CreateNewWorld(axiall);

                Grid.GetFreeGridSpace(new Vector2I(320, 32 * 8), out Vector2I offset);

                int y = Math.Max(Grid.HeightInCells, offset.y + 32 * 8);
                GridSettings.Reset(320, 32 * 8);

                //SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
                //Game.Instance.roomProber.Refresh();

                Log.Debuglog("after", Grid.WidthInCells, Grid.HeightInCells, offset);
            };*/

            destroyer.Detonate(false);
        }

        private void Terraform(WorldContainer world)
        {
            Log.Debuglog("terraforming");
            var terraformer = new GameObject();
            terraformer.transform.position = transform.position;
            terraformer.SetActive(true);
            terraformer.AddComponent<Terraformer>().Generate(world, world.worldName);

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
