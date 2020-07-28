using Harmony;
using System.Collections.Generic;
using UnityEngine;
using static DetailsScreen;

namespace Utils.FUI
{
    public class SideScreen
    {
        public static void AddSideScreen<T>(string name, string original = "Single Button Side Screen")
        {
            if (!GetElements(out List<SideScreenRef> screens, out GameObject body)) return;

            var oldPrefab = FindOriginal(original, screens);
            var newPrefab = Copy<T>(oldPrefab, body, name);

            screens.Add(NewSideScreen(name, newPrefab));
        }

        private static bool GetElements(out List<SideScreenRef> screens, out GameObject body)
        {
            var ds = Traverse.Create(Instance);
            screens = ds.Field("sideScreens").GetValue<List<SideScreenRef>>();
            body = ds.Field("sideScreenContentBody").GetValue<GameObject>();

            return screens != null && body != null;
        }

        private static SideScreenContent FindOriginal(string original, List<SideScreenRef> screens)
        {
            var result = screens.Find(s => s.name == original).screenPrefab;

            if (result == null)
                Debug.LogWarning("Could not find a sidescreen with the name " + original);

            return result;
        }

        private static SideScreenContent Copy<T>(SideScreenContent orig, GameObject body, string name = null)
        {
            var screen = Util.KInstantiateUI<SideScreenContent>(orig.gameObject, body).gameObject;
            Object.Destroy(screen.GetComponent<DoorToggleSideScreen>());

            var prefab = screen.AddComponent(typeof(T)) as SideScreenContent;
            prefab.name = name.Trim();

            return prefab;
        }

        private static SideScreenRef NewSideScreen(string name, SideScreenContent prefab)
        {
            return new SideScreenRef
            {
                name = name,
                offset = Vector2.zero,
                screenPrefab = prefab
            };
        }
    }
}
