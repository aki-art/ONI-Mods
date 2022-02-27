using System.Diagnostics;
using UnityEngine;

namespace Beached.Components
{
    // Allows to dynamically set biome colors with a tiny UI, used for previewing
    public class ZoneTypeColorPreviewer : KMonoBehaviour
    {
        public static ZoneTypeColorPreviewer Instance;

        protected override void OnPrefabInit()
        {
            Instance = this;
            base.OnPrefabInit();
        }

        protected override void OnCleanUp()
        {
            Instance = null;
            base.OnCleanUp();
        }

        public string color = "FFFFFF";

        public void Recolor()
        {
            var col = Util.ColorFromHex(color);
            if(col != null)
            {
                World.Instance.zoneRenderData.zoneColours[(int)ModAssets.ZoneTypes.depths] = col;
                World.Instance.zoneRenderData.OnActiveWorldChanged();
            }
        }

#if DEBUG
#pragma warning disable IDE0051 // Remove unused private members
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 200, 200, 500));

            color = GUILayout.TextField(color, 25);
            if (GUILayout.Button("Recolor"))
            {
                Recolor();
            }

            GUILayout.EndArea();
        }
#pragma warning restore IDE0051 // Remove unused private members
#endif
    }
}
