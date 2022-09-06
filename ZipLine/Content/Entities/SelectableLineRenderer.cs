using UnityEngine;

namespace ZipLine.Content.Entities
{
    // makes the rope highlight when hovered or selected
    public class SelectableLineRenderer : KMonoBehaviour
    {
        [MyCmpReq]
        private LineRenderer lineRenderer;

        [MyCmpReq]
        private KSelectable kSelectable;

        private static Color defaultColor = new Color(0.6f, 0.6f, 0.6f);
        private static Color selectColor = new Color(0.9f, 0.9f, 0.9f);
        private static Color hoverColor = new Color(0.75f, 0.75f, 0.75f);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            defaultColor = lineRenderer.material.GetColor("_TintColor");
            Subscribe((int)GameHashes.HighlightObject, OnHighlightChanged);
        }

        private void OnHighlightChanged(object data)
        {
            var highlighted = (bool)data;

            if (!highlighted)
            {
                SetColor(defaultColor);
                return;
            }

            SetColor(kSelectable.IsSelected ? selectColor : hoverColor);
        }

        private void SetColor(Color color)
        {
            lineRenderer.material.SetColor("_TintColor", color);
        }
    }
}
