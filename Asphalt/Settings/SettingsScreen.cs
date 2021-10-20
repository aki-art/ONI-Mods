using FUtility.FUI;

namespace Asphalt.Settings
{
    public class SettingsScreen : FScreen
    {
        private FPlaidSlider speedSlider;
        private FToggle externalFolder;
        private FToggle nukeToggle;
        private LocText nukeLabel;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            speedSlider = transform.Find("Content/SliderPanel/Slider").gameObject.AddComponent<FPlaidSlider>();
            externalFolder = transform.Find("Content/TogglePanel").gameObject.AddComponent<FToggle>();
            nukeToggle = transform.Find("Content/NukePanel").gameObject.AddComponent<FToggle>();
            nukeLabel = transform.Find("Content/NukePanel/NukeButton/Text").gameObject.GetComponent<LocText>();
            speedSlider.Initialize("Label", "SpeedLabel", 0, Tuning.SpeedRanges);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            SetKeys(nameof(Asphalt));
            nukeToggle.OnClick += ToggleNuke;
            externalFolder.toggle.isOn = ModSettings.Asphalt.UseSafeFolder;
            speedSlider.Value = ModSettings.Asphalt.SpeedMultiplier;
        }

        private void ToggleNuke()
        {
            nukeLabel.text = nukeToggle.toggle.isOn ? 
                STRINGS.UI.ASPHALTSETTINGSDIALOG.CONTENT.NUKEPANEL.NUKEBUTTON.CANCEL :
                STRINGS.UI.ASPHALTSETTINGSDIALOG.CONTENT.NUKEPANEL.NUKEBUTTON.TEXT;
        }

        public override void OnClickApply()
        {
            ModSettings.speedChanged = ModSettings.Asphalt.SpeedMultiplier != speedSlider.slider.value;
            ModSettings.Asphalt.SpeedMultiplier = speedSlider.slider.value;
            ModSettings.Asphalt.UseSafeFolder = externalFolder.toggle.isOn;
            ModSettings.Write();
        }

    }
}
