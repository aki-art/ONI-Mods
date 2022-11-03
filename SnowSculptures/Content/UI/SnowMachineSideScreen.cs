using FUtility;
using FUtility.FUI;
using SnowSculptures.Content.Buildings;
using UnityEngine;

namespace SnowSculptures.Content.UI
{
    public class SnowMachineSideScreen : SideScreenContent
    {
        private FSlider density;
        private FSlider speed;
        private FSlider lifeTime;
        private FSlider turbulence;

        SnowMachine target;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<SnowMachine>() != null;

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);

            this.target = target.GetComponent<SnowMachine>();
            RefreshUI();
        }

        private void RefreshUI()
        {
            if(target == null)
            {
                return;
            }

            density.Value = target.density;
            speed.Value = target.speed;
            turbulence.Value = target.turbulence;
            lifeTime.Value = target.lifeTime;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            density = transform.Find("Contents/Density/Slider").gameObject.AddComponent<FSlider>();
            speed = transform.Find("Contents/Speed/Slider").gameObject.AddComponent<FSlider>();
            lifeTime = transform.Find("Contents/Lifetime/Slider").gameObject.AddComponent<FSlider>();
            turbulence = transform.Find("Contents/Turbulence/Slider").gameObject.AddComponent<FSlider>();

            Helper.AddSimpleToolTip(density.gameObject, STRINGS.UI.SNOWMACHINESIDESCREEN.CONTENTS.DENSITY.TOOLTIP);
            Helper.AddSimpleToolTip(speed.gameObject, STRINGS.UI.SNOWMACHINESIDESCREEN.CONTENTS.SPEED.TOOLTIP);
            Helper.AddSimpleToolTip(lifeTime.gameObject, STRINGS.UI.SNOWMACHINESIDESCREEN.CONTENTS.LIFETIME.TOOLTIP);
            Helper.AddSimpleToolTip(turbulence.gameObject, STRINGS.UI.SNOWMACHINESIDESCREEN.CONTENTS.TURBULENCE.TOOLTIP);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            density.OnChange += OnSettingsChanged;
            speed.OnChange += OnSettingsChanged;
            lifeTime.OnChange += OnSettingsChanged;
            turbulence.OnChange += OnSettingsChanged;
        }

        private void OnSettingsChanged()
        {
            if(target != null)
            {
                target.speed = speed.Value;
                target.turbulence = turbulence.Value;
                target.lifeTime = lifeTime.Value;
                target.density = density.Value;

                target.UpdateValues();
            }
        }
    }
}
