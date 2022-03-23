using FUtility.FUI;
using MoreMarbleSculptures.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMarbleSculptures.UI
{
    public class PaintableSidescreen : SideScreenContent
    {
        Paintable paintable;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<Paintable>() is Paintable paintable && paintable.enabled;

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            if(target != null)
            {
                paintable = target.GetComponent<Paintable>();
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            var swatchContainer = transform.Find("Contents/ColorSwatches");

            foreach(Transform swatch in swatchContainer)
            {
                if(swatch.TryGetComponent(out Image image))
                {
                    var color = image.color;
                    swatch.gameObject.AddComponent<FButton>().OnClick += () =>
                    {
                        if (paintable != null)
                        {
                            //paintable.SetColor(color);
                            paintable.SetSecondaryColor(color);
                        }
                    };
                }
            }

            
        }
    }
}
