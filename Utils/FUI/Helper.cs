﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUtility.FUI
{
    public class Helper
    {
        public static void ListComponents(GameObject obj)
        {
            Debug.Log("object name: " + obj.name);
            Debug.Log("Components:");
            foreach (var comp in obj.GetComponents<Component>())
            {
                Debug.Log(comp.GetType());
                ListFieldsAndProps(comp, comp.GetType());
            }
        }

        private static void ListFieldsAndProps(object obj, Type T)
        {
            foreach (FieldInfo fi in T.GetFields())
            {
                Debug.Log("Field[" + fi.Name + "] " + fi.GetValue(obj));
            }
            foreach (PropertyInfo fi in T.GetProperties())
            {
                Debug.Log("Property[" + fi.Name + "] " + fi.GetValue(obj, null));
            }
        }

        public static void ListChildren(Transform parent, int level = 0, int maxDepth = 10)
        {
            if (level >= maxDepth) return;

            foreach (Transform child in parent)
            {
                Console.WriteLine(string.Concat(Enumerable.Repeat('-', level)) + child.name);
                ListChildren(child, level + 1);
            }
        }

        public static ToolTip AddSimpleToolTip(GameObject gameObject, string message, bool alignCenter = false, float wrapWidth = 0)
        {
            ToolTip toolTip = gameObject.AddComponent<ToolTip>();
            toolTip.tooltipPivot = alignCenter ? new Vector2(0.5f, 0f) : new Vector2(1f, 0f);
            toolTip.tooltipPositionOffset = new Vector2(0f, 20f);
            toolTip.parentPositionAnchor = new Vector2(0.5f, 0.5f);

            if (wrapWidth > 0)
            {
                toolTip.WrapWidth = wrapWidth;
                toolTip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
            }
            ToolTipScreen.Instance.SetToolTip(toolTip);
            toolTip.SetSimpleTooltip(message);
            return toolTip;
        }

        public static KButton MakeKButton(ButtonInfo info, GameObject buttonPrefab, GameObject parent, int index = 0)
        {
            KButton kbutton = Util.KInstantiateUI<KButton>(buttonPrefab, parent, true);
            kbutton.onClick += info.action;
            kbutton.transform.SetSiblingIndex(index);
            LocText componentInChildren = kbutton.GetComponentInChildren<LocText>();
            componentInChildren.text = info.text;
            componentInChildren.fontSize = info.fontSize;
            return kbutton;
        }

        public static FDialog OpenFDialog<T>(GameObject prefab, string name = null)
        {
            if (prefab == null)
            {
                Log.Warning($"Could not display UI ({name}): screen prefab is null.");
                return null;
            }
            if (name == null)
                name = prefab.name;

            Transform parent = GetACanvas(name).transform;
            GameObject settingsScreen = UnityEngine.Object.Instantiate(prefab, parent);
            FDialog settingsScreenComponent = settingsScreen.AddComponent(typeof(T)) as FDialog;
            settingsScreenComponent.ShowDialog();

            return settingsScreenComponent;
        }

        public struct ButtonInfo
        {
            public LocString text;
            public System.Action action;
            public int fontSize;

            public ButtonInfo(LocString text, System.Action action, int font_size)
            {
                this.text = text;
                this.action = action;
                fontSize = font_size;
            }
        }

        public static Canvas GetACanvas(string name)
        {
            GameObject parent;
            if (FrontEndManager.Instance != null)
            {
                parent = FrontEndManager.Instance.gameObject;
            }
            else
            {
                if (GameScreenManager.Instance != null && GameScreenManager.Instance.ssOverlayCanvas != null)
                {
                    parent = GameScreenManager.Instance.ssOverlayCanvas;
                }
                else
                {
                    parent = new GameObject
                    {
                        name = name + "Canvas"
                    };
                    UnityEngine.Object.DontDestroyOnLoad(parent);
                    Canvas canvas = parent.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                    canvas.sortingOrder = 3000;
                    //parent.AddComponent<GraphicRaycaster>();
                }
            }

            return parent.GetComponent<Canvas>();
        }
    }
}

