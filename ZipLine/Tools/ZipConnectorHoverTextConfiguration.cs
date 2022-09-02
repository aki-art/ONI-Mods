using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace ZipLine.Tools
{
    public class ZipConnectorHoverTextConfiguration : HoverTextConfiguration
    {
        public string errorMessage;
        public float distance;

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

            if (errorMessage != null && !errorMessage.IsNullOrWhiteSpace())
            {
                var first = true;
                var texts = errorMessage.Split(new char[]
                {
                    '\n'
                }, System.StringSplitOptions.RemoveEmptyEntries);

                if(texts != null && texts.Length > 0)
                {
                    foreach (string text in texts)
                    {
                        if (!first)
                        {
                            hoverTextDrawer.NewLine();
                        }

                        if(!text.IsNullOrWhiteSpace())
                        {
                            hoverTextDrawer.DrawText(text, HoverTextStyleSettings[first ? 0 : 1]);
                        }

                        first = false;
                    }
                }
            }

            hoverTextDrawer.NewLine();

            hoverTextDrawer.DrawText($"Length: {GameUtil.GetFormattedDistance(distance)}", HoverTextStyleSettings[1]);

            hoverTextDrawer.NewLine();

            if (KInputManager.currentControllerIsGamepad)
            {
                hoverTextDrawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(Action.MouseRight), 20);
            }
            else
            {
                hoverTextDrawer.DrawIcon(hoverTextScreen.GetSprite("icon_mouse_right"), 20);
            }

            hoverTextDrawer.DrawText(backStr, Styles_Instruction.Standard);
            hoverTextDrawer.EndShadowBar();
            hoverTextDrawer.EndDrawing();
        }
    }
}
