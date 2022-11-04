using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using static PrintingPodRecharge.Settings.General;

namespace PrintingPodRecharge.UI
{
    public class ModSettingsScreen : FScreen
    {
        private FInputField2 refundKgInput;
        private FToggle2 refundActiveToggle;
        private FToggle2 debugToggle;
        private FToggle2 twitch;
        private RainbowSlider randoChance;
        private FCycle randoCycler;

        public override void SetObjects()
        {
            base.SetObjects();
            refundKgInput = transform.Find("Content/Refund/Input").FindOrAddComponent<FInputField2>();

            refundActiveToggle = transform.Find("Content/RefundActiveToggle").FindOrAddComponent<FToggle2>();
            refundActiveToggle.SetCheckmark("Background/Checkmark");

            debugToggle = transform.Find("Content/DebugModeToggle").FindOrAddComponent<FToggle2>();
            debugToggle.SetCheckmark("Background/Checkmark");

            twitch = transform.Find("Content/TwitchIntegration").FindOrAddComponent<FToggle2>();
            twitch.SetCheckmark("Background/Checkmark");

            randoChance = transform.Find("Content/SliderPanel/Slider").FindOrAddComponent<RainbowSlider>();

            randoCycler = transform.Find("Content/RandoDupePreset").FindOrAddComponent<FCycle>();
            randoCycler.Initialize(
                randoCycler.transform.Find("Left").FindOrAddComponent<FButton>(),
                randoCycler.transform.Find("Right").FindOrAddComponent<FButton>(),
                randoCycler.transform.Find("ChoiceLabel").FindOrAddComponent<LocText>());

            var unitLabel = refundKgInput.transform.parent.Find("UnitLabel").FindOrAddComponent<LocText>();
            unitLabel.text = GameUtil.GetCurrentMassUnit();

            transform.Find("VersionLabel").GetComponent<LocText>().text = $"v{Log.GetVersion()}";

            Helper.AddSimpleToolTip(debugToggle.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.DEBUGMODETOGGLE.TOOLTIP);
            Helper.AddSimpleToolTip(refundActiveToggle.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDACTIVETOGGLE.TOOLTIP);
            Helper.AddSimpleToolTip(refundKgInput.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUND.TOOLTIP);
            Helper.AddSimpleToolTip(randoChance.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.SLIDERPANEL.SLIDER.TOOLTIP);
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            randoCycler.Options = new List<FCycle.Option>()
            {
                new FCycle.Option(RandoDupeTier.Terrible.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.TERRIBLE),
                new FCycle.Option(RandoDupeTier.Vanillaish.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.VANILLAISH),
                new FCycle.Option(RandoDupeTier.Default.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.DEFAULT),
                new FCycle.Option(RandoDupeTier.Generous.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.GENEROUS),
                new FCycle.Option(RandoDupeTier.Wacky.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.WACKY)
            };

            randoCycler.Value = Mod.Settings.RandoDupePreset.ToString();

            refundKgInput.Text = string.Format(STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUND.QUANTITY, Mod.Settings.RefundBioInkKg);
            refundActiveToggle.On = Mod.Settings.RefundActiveInk;
            debugToggle.On = Mod.Settings.DebugTools;
            twitch.On = Mod.Settings.TwitchIntegration;
            randoChance.Value = Mod.Settings.RandomDupeReplaceChance;
        }

        public override void OnClickApply()
        {
            Mod.Settings.DebugTools = debugToggle.On;
            Mod.Settings.RefundActiveInk = refundActiveToggle.On;
            Mod.Settings.TwitchIntegration = twitch.On;
            Mod.Settings.RefundBioInkKg = float.TryParse(refundKgInput.Text, out var kg) ? kg : 1f;
            Mod.Settings.RandomDupeReplaceChance = randoChance.GetRoundedValue();
            Mod.Settings.RandoDupePreset = Enum.TryParse<RandoDupeTier>(randoCycler.Value, out var result) ? result : RandoDupeTier.Default;

            Mod.SaveSettings();
            Deactivate();
        }
    }
}
