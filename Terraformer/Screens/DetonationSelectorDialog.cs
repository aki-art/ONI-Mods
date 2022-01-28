using FUtility;
using FUtility.FUI;
using UnityEngine;
using UnityEngine.UI;

namespace Terraformer.Screens
{
    public class DetonationSelectorDialog : FScreen
    {
        [SerializeField]
        public string asteroidName;

        [SerializeField]
        public AsteroidGridEntity asteroid;

        private KBatchedAnimController kbac;

        private LocText selectLabel;
        private LocText inTitle;
        private Image inImage;
        private Image outImage;

        public override void SetObjects()
        {
            selectLabel = transform.Find("SelectLabel").GetComponent<LocText>();
            var resultPreview = transform.Find("ResultPreview");
            inTitle = resultPreview.Find("inTitle").GetComponent<LocText>();
            inImage = resultPreview.Find("inBg").GetComponent<Image>();
            outImage = resultPreview.Find("outBg").GetComponent<Image>();

            base.SetObjects();
        }

        public void SetAsteroid(AsteroidGridEntity asteroid)
        {
            this.asteroid = asteroid;
            asteroidName = asteroid.GetProperName();

            selectLabel.text = selectLabel.text.Replace("{Name}", asteroidName);
            inTitle.text = inTitle.text.Replace("{Name}", asteroidName);

            Log.Assert("asteroid.AnimConfigs[0].animFile", asteroid.AnimConfigs[0].animFile);
            Log.Assert("inImage", inImage);

            Log.Debuglog("playing" + asteroid.AnimConfigs[0].animFile.name);

            var pos = inImage.transform.position + new Vector3(10, 10, -5f);
            var parent = inImage.transform.parent;


            kbac = FXHelpers.CreateEffect(asteroid.AnimConfigs[0].animFile.name, inImage.transform.position, inImage.transform, layer: 0);
            kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
            //kbac.transform.localPosition.Set(0, 0, 0);
            kbac.animScale = 0.25f;
            kbac.setScaleFromAnim = false;
            kbac.isMovable = false;
            kbac.materialType = KAnimBatchGroup.MaterialType.UI;
            kbac.animOverrideSize = new Vector2(100, 100);
            kbac.usingNewSymbolOverrideSystem = true;

            /*
            var renderer = kbac.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
            renderer.batch = kbac.GetBatch();
            */

            inImage.enabled = false;
            outImage.enabled = false;
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            kbac.gameObject.SetActive(true);

            kbac.SetLayer(5);
            kbac.SetDirty();

            kbac.Play("idle_loop");
        }
    }
}
