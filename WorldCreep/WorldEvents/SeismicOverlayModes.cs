using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class SeismicOverlayMode
    {
        public class SeismicMode : OverlayModes.Mode
        {
            // TODO: make update automatically, not just on overlay enable
            public override HashedString ViewMode() => ID;
            protected UniformGrid<WorldEvent> partition;
            private HashSet<WorldEvent> layerTargets = new HashSet<WorldEvent>();
            protected ICollection<Tag> targetIDs;
            public static readonly HashedString ID = "SeismicOverlay";
            private int cameraLayerMask;
            private int selectionMask;
            protected int targetLayer;

            public override string GetSoundName() => "Decor";

            public SeismicMode()
            {
                targetIDs = new HashSet<Tag>()
                {
                    EarthQuakeConfig.ID
                };

                targetLayer = LayerMask.NameToLayer("MaskedOverlay");
                cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
                selectionMask = LayerMask.GetMask("MaskedOverlay");
            }


            public override void Update()
            {
                layerTargets.Clear();
                foreach (WorldEvent instance in partition.GetAllItems())
                {
                    layerTargets.Add(instance);
                    instance.GetComponent<SeismicEventVisualizer>().Show();
                }

                base.Update();
            }

            private void DisableCircles()
            {
                foreach (var target in layerTargets)
                    target.GetComponent<SeismicEventVisualizer>().Stop();
            }

            protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
            {
                if (!targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
                    return;

                WorldEvent we = item.GetComponent<WorldEvent>();
                SeismicEventVisualizer visualizer = item.GetComponent<SeismicEventVisualizer>();

                if (we == null || visualizer == null)
                    return;

                partition.Add(we);
            }

            protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
            {
                if (item == null || item.gameObject == null)
                    return;

                WorldEvent component = item.GetComponent<WorldEvent>();

                if (component == null)
                    return;

                if (layerTargets.Contains(component))
                    layerTargets.Remove(component);

                partition.Remove(component);
            }

            public override void Enable()
            {
                RegisterSaveLoadListeners();
                Camera.main.cullingMask |= cameraLayerMask;
                SelectTool.Instance.SetLayerMask(selectionMask);
                partition = PopulatePartition<WorldEvent>(targetIDs);
            }

            public override void Disable()
            {
                DisableCircles();
                UnregisterSaveLoadListeners();
                partition.Clear();
                Camera.main.cullingMask &= ~cameraLayerMask;
                layerTargets.Clear();
                SelectTool.Instance.ClearLayerMask();
            }
        }

        public class FaultChunksOverlayMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "SeismicChunksOverlay";
            public override string GetSoundName() => "Decor";
        }

        public class EarthQuakeOverlayMode : OverlayModes.Mode
        {
            public override HashedString ViewMode() => ID;
            public static readonly HashedString ID = "EarthquakeOverlay";
            public override string GetSoundName() => "Decor";
        }
    }
}
