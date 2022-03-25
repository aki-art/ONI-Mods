using UnityEngine;

namespace AETNTweaks.Components
{
    // TRASH, get an fx hash going instead
    public class PulseVisualizer : KMonoBehaviour
    {
        [SerializeField]
        public string fxAnimName;

        [SerializeField]
        public Vector3 offset;

        private KBatchedAnimController fx;

        protected override void OnCmpEnable()
        {
            base.OnCmpEnable();
            ShowFx();
        }
        protected override void OnCmpDisable()
        {
            base.OnCmpDisable();
            HideFx();
        }

        protected override void OnCleanUp()
        {
            Destroy(fx.gameObject);
            base.OnCleanUp();
        }

        public void ShowFx()
        {
            if(fx == null)
            {
                fx = FXHelpers.CreateEffect(fxAnimName, transform.position + offset, transform, false, Grid.SceneLayer.Front, false);
                fx.destroyOnAnimComplete = false;
                fx.randomiseLoopedOffset = true;
                fx.animScale *= 3f;
                fx.TintColour = new Color(1f, 0.5f, 0.6f);
            }

            fx.gameObject.SetActive(true);
            fx.Play("snore", KAnim.PlayMode.Once);
        }

        public void HideFx()
        {
            if (fx == null) return;
            fx.Stop();
            fx.gameObject.SetActive(false);
        }
    }
}
