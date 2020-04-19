using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    /* TODO
     * - Localization support  */

    class UIHelper
    {
        public static ToolTip AddSimpleToolTip(GameObject gameObject, string message, bool center = false, float wrapWidth = 0)
        {
            ToolTip toolTip = gameObject.AddComponent<ToolTip>();
            if (center)
            {
                toolTip.tooltipPivot = new Vector2(0.5f, 0f);
                toolTip.tooltipPositionOffset = new Vector2(0f, 20f);
                toolTip.parentPositionAnchor = new Vector2(0.5f, 0.5f);
            }
            else
            {
                toolTip.tooltipPivot = new Vector2(1f, 0f);
                toolTip.tooltipPositionOffset = new Vector2(0f, 20f);
                toolTip.parentPositionAnchor = new Vector2(0.5f, 0.5f);
            }

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
                    Object.DontDestroyOnLoad(parent);
                    Canvas canvas = parent.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                    canvas.sortingOrder = 3000;
                    parent.AddComponent<GraphicRaycaster>();
                }
            }

            return parent.GetComponent<Canvas>();
        }
    }
}
