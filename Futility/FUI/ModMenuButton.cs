using Harmony;
using Klei;
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
            {
                if (TryAddButton(modEntry, modPath, OnClick, label))
                {
                    Debug.Log("found my mod");
                    return;
                }
            }
        }

        public static void AddCustomModSettingsButton(object displayedMods, string modPath, System.Action OnClick, GameObject prefab)
        {
            foreach (var modEntry in (IEnumerable)displayedMods)
            {
                int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
                Mod mod = Global.Instance.modManager.mods[index];

                if (IsThisMyMod(index, mod, modPath))
                {
                    Transform transform = Traverse.Create(modEntry).Field("rect_transform").GetValue<RectTransform>();
                    if (transform != null)
                    {
                        KButton kbutton = Util.KInstantiateUI<KButton>(prefab, transform.Find("ManageButton").parent.gameObject, true);
                        kbutton.transform.position = Vector3.zero;
                        kbutton.onClick += OnClick;
                        kbutton.transform.SetSiblingIndex(index);
                    }
                }
            }
        }

        private static bool TryAddButton(object modEntry, string modPath, System.Action action, string label)
        {
            int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
            Mod mod = Global.Instance.modManager.mods[index];

            if (IsThisMyMod(index, mod, modPath))
            {
                MakeButton(modEntry, action, label);
                return true;
            }

            return false;
        }

        private static bool IsThisMyMod(int index, Mod mod, string path)
        {
            return index >= 0 && FileSystem.Normalize(mod.file_source.GetRoot()) == FileSystem.Normalize(path);
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
