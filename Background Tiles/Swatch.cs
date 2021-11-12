using FUtility;
using UnityEngine;
using UnityEngine.UI;

namespace BackgroundTiles
{
    public class Swatch : KMonoBehaviour
    {
        // using transform hierarchy to get a skewed look, as if this was rendered with 3d perspective
        private RectTransform middleTransform;
        private Sprite sprite;
        private Image image;

        public void SetSprite(BuildingDef buildingDef)
        {
            sprite = BackgroundTilesManager.GetSprite(buildingDef);

            if (middleTransform is null) InitTransforms();

            middleTransform.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }

        private void InitTransforms()
        {
            var go1 = new GameObject("UI_SwatchSkewTransform");
            middleTransform = go1.AddOrGet<RectTransform>();
           // middleTransform.localScale.Set(1, 0.37f, 1);
            //middleTransform.position.Set(0, 0, 0);
            middleTransform.SetParent(transform);
            middleTransform.gameObject.SetActive(true);
            //Image img = middleTransform.gameObject.AddComponent<Image>();
            //img.color = Color.red;

            var go2 = new GameObject("UI_Swatch");
            var lastTransform = go2.AddOrGet<RectTransform>();
            //lastTransform.Rotate(0, -72.4f, -109.9f);
            lastTransform.SetParent(middleTransform);

            image = go2.AddComponent<Image>();
        }
    }
}
