using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PrintingPodRecharge.Settings.General;

namespace PrintingPodRecharge.UI
{
    public class ModSettingsScreen : FScreen
    {
        private FInputField2 refundKgInput;
        private FToggle2 refundActiveToggle;
        private FToggle2 debugToggle;
        private FToggle2 twitch;
        private FToggle2 coloredMeeps;
        private RainbowSlider randoChance;
        private FCycle randoCycler;
        private FCycle refundCycler;
        private LocText meepLabel;

        public override void SetObjects()
        {
            base.SetObjects();

            refundKgInput = transform.Find("Content/Refund/Input").FindOrAddComponent<FInputField2>();

            refundActiveToggle = transform.Find("Content/RefundActiveToggle").FindOrAddComponent<FToggle2>();
            refundActiveToggle.SetCheckmark("Background/Checkmark");

            debugToggle = transform.Find("Content/DebugModeToggle").FindOrAddComponent<FToggle2>();
            debugToggle.SetCheckmark("Background/Checkmark");

            coloredMeeps = transform.Find("Content/ColoredMeeps").FindOrAddComponent<FToggle2>();
            coloredMeeps.SetCheckmark("Background/Checkmark");

            twitch = transform.Find("Content/TwitchIntegration").FindOrAddComponent<FToggle2>();
            twitch.SetCheckmark("Background/Checkmark");

            randoChance = transform.Find("Content/SliderPanel/Slider").FindOrAddComponent<RainbowSlider>();

            refundCycler = transform.Find("Content/RefundCycle").FindOrAddComponent<FCycle>();
            refundCycler.Initialize(
                refundCycler.transform.Find("Left").FindOrAddComponent<FButton>(),
                refundCycler.transform.Find("Right").FindOrAddComponent<FButton>(),
                refundCycler.transform.Find("ChoiceLabel").FindOrAddComponent<LocText>(),
                refundCycler.transform.Find("ChoiceLabel/Description").FindOrAddComponent<LocText>());

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
            Helper.AddSimpleToolTip(refundCycler.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.TOOLTIP);
            Helper.AddSimpleToolTip(coloredMeeps.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.COLOREDMEEPS.TOOLTIP);

            meepLabel = coloredMeeps.transform.Find("Label").GetComponent<LocText>();

        }

        public override void ShowDialog()
        {
            base.ShowDialog();


            var top = Util.ColorFromHex("FF51C9FF");
            var bottom = Util.ColorFromHex("70CAFFFF");
            meepLabel.colorGradient = new TMPro.VertexGradient(top, top, bottom, bottom);
            meepLabel.enableVertexGradient = true;
            meepLabel.color = Color.white;

            randoCycler.Options = new List<FCycle.Option>()
            {
                new FCycle.Option(RandoDupeTier.Terrible.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.TERRIBLE),
                new FCycle.Option(RandoDupeTier.Vanillaish.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.VANILLAISH),
                new FCycle.Option(RandoDupeTier.Default.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.DEFAULT),
                new FCycle.Option(RandoDupeTier.Generous.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.GENEROUS),
                new FCycle.Option(RandoDupeTier.Wacky.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RANDODUPEPRESET.TIERS.WACKY)
            };

            randoCycler.Value = Mod.Settings.RandoDupePreset.ToString();

            refundCycler.Options = new List<FCycle.Option>()
            {
                new FCycle.Option("matching", STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.MATCHING, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.MATCHING_DESCRIPTION),
                new FCycle.Option("default", STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.DEFAULT, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.DEFAULT_DESCRIPTION),
                new FCycle.Option("none", STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.NONE, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.OPTIONS.NONE_DESCRIPTION),
            };

            refundCycler.Value = Mod.Settings.RefundeInk ? (Mod.Settings.RefundActiveInk ? "matching" : "default") : "none";

            refundKgInput.Text = string.Format(STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUND.QUANTITY, Mod.Settings.RefundBioInkKg);
            refundActiveToggle.On = Mod.Settings.RefundActiveInk;
            debugToggle.On = Mod.Settings.DebugTools;
            twitch.On = Mod.Settings.TwitchIntegration;
            randoChance.Value = Mod.Settings.RandomDupeReplaceChance;
            coloredMeeps.On = Mod.Settings.ColoredMeeps;
        }

        public override void OnClickApply()
        {
            Mod.Settings.DebugTools = debugToggle.On;
            Mod.Settings.RefundActiveInk = refundActiveToggle.On;
            Mod.Settings.TwitchIntegration = twitch.On;
            Mod.Settings.RefundBioInkKg = float.TryParse(refundKgInput.Text, out var kg) ? kg : 1f;
            Mod.Settings.RandomDupeReplaceChance = randoChance.GetRoundedValue();
            Mod.Settings.RandoDupePreset = Enum.TryParse<RandoDupeTier>(randoCycler.Value, out var result) ? result : RandoDupeTier.Default;

            Log.Debuglog("refund ink value: " + refundCycler.Value);

            Mod.Settings.RefundActiveInk = refundCycler.Value == "matching";
            Mod.Settings.RefundeInk = refundCycler.Value != "none";
            Mod.Settings.ColoredMeeps = coloredMeeps.On;

            Mod.SaveSettings();
            Deactivate();
        }
    }
}
