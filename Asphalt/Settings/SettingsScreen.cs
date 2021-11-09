using FUtility.FUI;

namespace Asphalt.Settings
{
    public class SettingsScreen : FScreen
    {
        private PlaidSlider slider;
        private LocText versionLabel;

        public override void SetObjects()
        {
            base.SetObjects();
            slider = transform.Find("SliderPanel/Slider").gameObject.AddComponent<PlaidSlider>();
            versionLabel = transform.Find("VersionLabel").gameObject.GetComponent<LocText>();

            Restore();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            slider.UpdateRanges();
        }

        void Restore()
        {
            slider.Value = Mod.Settings.Speed;
            versionLabel.SetText("v2.0.0.0");
        }

        public override void OnClickApply()
        {
            if(Mod.Settings.Speed != slider.Value)
            {
                Mod.Settings.SpeedChanged = true;
            }

            Mod.Settings.Speed = slider.Value;
            Mod.config.Write();

            Deactivate();
        }
    }
}
