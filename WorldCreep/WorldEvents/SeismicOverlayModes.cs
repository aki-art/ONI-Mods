using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class SeismicOverlayMode
    {
        public static StatusItem.StatusItemOverlays seismicOverlayItem;
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
                foreach (WorldEvent target in layerTargets)
                    target.GetComponent<SeismicEventVisualizer>().Stop();
            }

            protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
            {
                if (IsTarget(item) && item.TryGetComponent(out WorldEvent worldEvent) && HasVisualizer(item))
                    partition.Add(worldEvent);
            }

            private bool IsTarget(SaveLoadRoot item) => targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag());
            private static bool HasVisualizer(SaveLoadRoot item) => item.GetComponent<SeismicEventVisualizer>() != null;

            protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
            {
                if (item?.gameObject != null && item.TryGetComponent(out WorldEvent worldEvent))
                {
                    if (layerTargets.Contains(worldEvent))
                        layerTargets.Remove(worldEvent);

                    partition.Remove(worldEvent);
                }
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
