using System.Collections.Generic;
using UnityEngine;

namespace ZipLine.Content.Tools
{
    public class ZipConnectorHoverTextConfiguration : HoverTextConfiguration
    {
        public List<string> errors = new List<string>();
        public float distance;
        public float angle;

        public TextStyleSetting title;
        public TextStyleSetting normal;
        public TextStyleSetting warning;

        public override void UpdateHoverElements(List<KSelectable> hover_objects)
        {
            var hoverTextScreen = HoverTextScreen.Instance;

            var hoverTextDrawer = hoverTextScreen.BeginDrawing();

            var num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));

            if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
            {
                hoverTextDrawer.EndDrawing();
                return;
            }

            hoverTextDrawer.BeginShadowBar(false);

            DrawLength(hoverTextDrawer);
#if DEBUG
            DrawAngle(hoverTextDrawer);
#endif
            DrawFiberCost(hoverTextDrawer);
            DrawErrors(hoverTextDrawer);
            DrawCancel(hoverTextScreen, hoverTextDrawer);

            hoverTextDrawer.EndShadowBar();

            hoverTextDrawer.EndDrawing();
        }

        private void DrawCancel(HoverTextScreen hoverTextScreen, HoverTextDrawer hoverTextDrawer)
        {
            if (KInputManager.currentControllerIsGamepad)
            {
                hoverTextDrawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(Action.MouseRight), 20);
            }
            else
            {
                hoverTextDrawer.DrawIcon(hoverTextScreen.GetSprite("icon_mouse_right"), 20);
            }

            hoverTextDrawer.DrawText(backStr, Styles_Instruction.Standard);
        }

        private void DrawErrors(HoverTextDrawer hoverTextDrawer)
        {
            var first = true;

            if (errors != null && errors.Count > 0)
            {
                foreach (var text in errors)
                {
                    if (!first)
                    {
                        hoverTextDrawer.NewLine();
                    }

                    if (!text.IsNullOrWhiteSpace())
                    {
                        hoverTextDrawer.DrawText(text, warning);
                    }

                    first = false;
                }
            }

            hoverTextDrawer.NewLine();
        }

        private void DrawLength(HoverTextDrawer hoverTextDrawer)
        {
            hoverTextDrawer.DrawText($"Length: {GameUtil.GetFormattedDistance(distance)}", ToolTitleTextStyle);
            hoverTextDrawer.NewLine();
        }

        private void DrawAngle(HoverTextDrawer hoverTextDrawer)
        {
            hoverTextDrawer.DrawText($"Angle: {angle}", ToolTitleTextStyle);
            hoverTextDrawer.NewLine();
        }

        private void DrawFiberCost(HoverTextDrawer hoverTextDrawer)
        {
            var fiberName = global::STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME;
            hoverTextDrawer.DrawText($"Cost: {Mathf.CeilToInt(distance * Mod.Settings.FiberPerMeterCost)} {fiberName}", ToolTitleTextStyle);
            hoverTextDrawer.NewLine();
        }
    }
}
