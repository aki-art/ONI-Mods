using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace WorldTraitsPlus.WorldEvents
{
    public class EarthQuakeVignette : KMonoBehaviour
    {
        Image image;
        Color color;

        public void Initialize()
        {
            Image vignetteImage = Traverse.Create(Vignette.Instance).Field<Image>("image").Value;
            image = Instantiate(vignetteImage, vignetteImage.transform.parent);

            Material newMat = Instantiate(vignetteImage.material);

            image.material = newMat;
            image.material.SetTexture("_MainTex", ModAssets.QuakeVignette);
            image.gameObject.SetActive(false);
            color = Color.white;
        }

        public void Show(bool show)
        {
            image.gameObject.SetActive(show);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void SetIntensity(float val)
        {
            color.a = val;
        }
    }
}
