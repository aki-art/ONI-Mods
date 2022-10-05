using FUtility;
using FUtility.FUI;

namespace PrintingPodRecharge.UI
{
    public class ModSettingsScreen : FScreen
    {
        private FInputField2 refundKgInput;
        private FToggle2 refundActiveToggle;
        private FToggle2 debugToggle;
        private RainbowSlider randoChance;

        public override void SetObjects()
        {
            base.SetObjects();
            refundKgInput = transform.Find("Content/Refund/Input").FindOrAddComponent<FInputField2>();

            refundActiveToggle = transform.Find("Content/RefundActiveToggle").FindOrAddComponent<FToggle2>();
            refundActiveToggle.SetCheckmark("Background/Checkmark");

            debugToggle = transform.Find("Content/DebugModeToggle").FindOrAddComponent<FToggle2>();
            debugToggle.SetCheckmark("Background/Checkmark");

            randoChance = transform.Find("Content/SliderPanel/Slider").FindOrAddComponent<RainbowSlider>();

            var unitLabel = refundKgInput.transform.parent.Find("UnitLabel").FindOrAddComponent<LocText>();
            unitLabel.text = GameUtil.GetCurrentMassUnit();

            transform.Find("VersionLabel").GetComponent<LocText>().text = $"v{Log.GetVersion()}";
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            refundKgInput.Text = string.Format(STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUND.QUANTITY, Mod.Settings.RefundBioInkKg);
            refundActiveToggle.On = Mod.Settings.RefundActiveInk;
            debugToggle.On = Mod.Settings.DebugTools;
            randoChance.Value = Mod.Settings.RandomDupeReplaceChance;
        }

        public override void OnClickApply()
        {
            Mod.Settings.DebugTools = debugToggle.On;
            Mod.Settings.RefundActiveInk = refundActiveToggle.On;
            Mod.Settings.RefundBioInkKg = float.TryParse(refundKgInput.Text, out var kg) ? kg : 1f;
            Mod.Settings.RandomDupeReplaceChance = randoChance.GetRoundedValue();

            Mod.SaveSettings();
            Deactivate();
        }
    }
}
