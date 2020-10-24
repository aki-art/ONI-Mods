using Harmony;
using KMod;
using System.Collections;
using UnityEngine;
using static FUtility.FUI.Helper;

namespace FUtility.FUI
{
    public class ModMenuButton
    {
        /// <summary> Clones the Subscription button to add a Settings button next to it. </summary>
        public static void AddModSettingsButton(object displayedMods, string modPath, System.Action OnClick, string label = "Settings")
        {
            foreach (var modEntry in (IEnumerable)displayedMods)
                TryAddButton(modEntry, modPath, OnClick, label);
        }

        private static void TryAddButton(object modEntry, string modPath, System.Action action, string label)
        {
            int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
            Mod mod = Global.Instance.modManager.mods[index];

            if (IsThisMyMod(index, mod, modPath))
                MakeButton(modEntry, action, label);
        }

        private static bool IsThisMyMod(int index, Mod mod, string path)
        {
            return index >= 0 && mod.file_source.GetRoot() == path;
        }

        private static void MakeButton(object modEntry, System.Action action, string label)
        {
            Transform transform = Traverse.Create(modEntry).Field("rect_transform").GetValue<RectTransform>();
            if (transform != null)
                CloneButton(transform.Find("ManageButton"), action, label);
        }

        private static void CloneButton(Transform prefab, System.Action action, string label)
        {
            if (prefab != null)
                MakeKButton(
                    new ButtonInfo(label, action, 14),
                    prefab.gameObject,
                    prefab.parent.gameObject,
                    prefab.GetSiblingIndex() - 1);
        }
    }
}
