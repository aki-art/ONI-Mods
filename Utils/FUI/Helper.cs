using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace FUtility.FUI
{
    public class Helper
    {
        private static TMP_FontAsset NotoSans;
        private static TMP_FontAsset GrayStroke;
        public static TextStyleSetting defaultTextStyle;
        private static readonly Dictionary<TextAnchor, TextAlignmentOptions> alignments = new Dictionary<TextAnchor, TextAlignmentOptions>
        {
            { TextAnchor.UpperLeft, TextAlignmentOptions.TopLeft },
            { TextAnchor.UpperCenter, TextAlignmentOptions.Top },
            { TextAnchor.UpperRight, TextAlignmentOptions.TopRight },
            { TextAnchor.MiddleLeft, TextAlignmentOptions.MidlineLeft },
            { TextAnchor.MiddleCenter, TextAlignmentOptions.Midline },
            { TextAnchor.MiddleRight, TextAlignmentOptions.MidlineRight },
            { TextAnchor.LowerLeft, TextAlignmentOptions.TopLeft },
            { TextAnchor.LowerCenter, TextAlignmentOptions.Top },
            { TextAnchor.LowerRight, TextAlignmentOptions.TopRight }
        };


        public static void Initialize()
        {
            Debug.Log("Initialziing");
            foreach (var newFont in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
            {
                Debug.Log(newFont.name + " Fields and Properties ------------------------");
                ListFieldsAndProps(newFont, typeof(TMP_FontAsset));
                switch (newFont?.name)
                {
                    case "NotoSans-Regular":
                        NotoSans = newFont;
                        break;
                    case "GRAYSTROKE REGULAR SDF":
                        GrayStroke = newFont;
                        break;
                    default:
                        break;
                }
            }

            defaultTextStyle = ScriptableObject.CreateInstance<TextStyleSetting>();
            defaultTextStyle.enableWordWrapping = true;
            defaultTextStyle.fontSize = 14;
            defaultTextStyle.sdfFont = NotoSans;
            defaultTextStyle.style = FontStyles.Normal;
            defaultTextStyle.textColor = Color.white;

        }

    public static void ReplaceAllText(GameObject parent)
        {
            if (defaultTextStyle == null)
                Initialize();


            var textComponents = parent.GetComponentsInChildren(typeof(Text));
            foreach (GameObject obj in textComponents.Select(c => c.gameObject))
            {
                Text regularText = obj.GetComponent<Text>();
                Debug.Log(regularText.font.name);
                string content = regularText.text;
                UnityEngine.Object.DestroyImmediate(obj.GetComponent<Text>());

                var TMPText = obj.gameObject.AddComponent<TextMeshProUGUI>();
                TMPText.alignment = TextAlignmentOptions.Baseline;
                TMPText.autoSizeTextContainer = false;
                TMPText.enabled = true;
                TMPText.color = Color.cyan;
                TMPText.font = defaultTextStyle.sdfFont;
                TMPText.fontSize = defaultTextStyle.fontSize;
                TMPText.fontStyle = defaultTextStyle.style;
                TMPText.maxVisibleLines = 1;
                TMPText.text = content;
                //textDisplay.key = "TEST";

            }
        }

        class TextSettings
        {
            string text;
            Color color;
            TMP_FontAsset font;
            TextAlignmentOptions alignment;
            float fontSize;
            FontStyles style;

            public TextSettings(Text original)
            {
                text = original.text;
                color = original.color;
                alignment = alignments[original.alignment];
            }
        }


        public static void ListComponents(GameObject obj)
        {
            Debug.Log("Diagram object name: " + obj.name);
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

