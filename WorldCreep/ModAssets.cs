using UnityEngine;

namespace WorldCreep
{
    public class ModAssets
    {
        public static string ModPath;
        public class Prefabs
        {
            public static GameObject circleMarker;
        }

        public static void LateLoadAssets()
        {
            Prefabs.circleMarker = new GameObject();
            Prefabs.circleMarker.AddComponent<WorldEvents.SeismicEventVisualizer>();
            Prefabs.circleMarker.SetActive(false);
        }
    }
}
