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

        private bool init;

        private void Init()
        {
            refundKgInput = transform.Find("Content/Refund/Input").gameObject.AddOrGet<FInputField2>();

            refundActiveToggle = transform.Find("Content/RefundActiveToggle").gameObject.AddOrGet<FToggle2>();
            refundActiveToggle.SetCheckmark("Background/Checkmark");

            debugToggle = transform.Find("Content/DebugModeToggle").gameObject.AddOrGet<FToggle2>();
            debugToggle.SetCheckmark("Background/Checkmark");

            coloredMeeps = transform.Find("Content/ColoredMeeps").gameObject.AddOrGet<FToggle2>();
            coloredMeeps.SetCheckmark("Background/Checkmark");

            transform.Find("Content/TwitchIntegration").gameObject.SetActive(false);

            randoChance = transform.Find("Content/SliderPanel/Slider").gameObject.AddOrGet<RainbowSlider>();

            refundCycler = transform.Find("Content/RefundCycle").gameObject.AddOrGet<FCycle>();
            refundCycler.Initialize(
                refundCycler.transform.Find("Left").gameObject.AddOrGet<FButton>(),
                refundCycler.transform.Find("Right").gameObject.AddOrGet<FButton>(),
                refundCycler.transform.Find("ChoiceLabel").gameObject.AddOrGet<LocText>(),
                refundCycler.transform.Find("ChoiceLabel/Description").gameObject.AddOrGet<LocText>());

            randoCycler = transform.Find("Content/RandoDupePreset").gameObject.AddOrGet<FCycle>();
            randoCycler.Initialize(
                randoCycler.transform.Find("Left").gameObject.AddOrGet<FButton>(),
                randoCycler.transform.Find("Right").gameObject.AddOrGet<FButton>(),
                randoCycler.transform.Find("ChoiceLabel").gameObject.AddOrGet<LocText>());


            var unitLabel = refundKgInput.transform.parent.Find("UnitLabel").gameObject.AddOrGet<LocText>();
            unitLabel.text = GameUtil.GetCurrentMassUnit();

            transform.Find("VersionLabel").GetComponent<LocText>().text = $"v{Log.GetVersion()}";

            Helper.AddSimpleToolTip(debugToggle.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.DEBUGMODETOGGLE.TOOLTIP);
            Helper.AddSimpleToolTip(refundActiveToggle.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDACTIVETOGGLE.TOOLTIP);
            Helper.AddSimpleToolTip(refundKgInput.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUND.TOOLTIP);
            Helper.AddSimpleToolTip(randoChance.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.SLIDERPANEL.SLIDER.TOOLTIP);
            Helper.AddSimpleToolTip(refundCycler.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.REFUNDCYCLE.TOOLTIP);
            Helper.AddSimpleToolTip(coloredMeeps.gameObject, STRINGS.UI.SETTINGSDIALOG.CONTENT.COLOREDMEEPS.TOOLTIP);

            meepLabel = coloredMeeps.transform.Find("Label").GetComponent<LocText>();

            init = true;
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            if (!init)
            {
                Init();
            }

            var top = Util.ColorFromHex("FF51C9FF");
            var bottom = Util.ColorFromHex("70CAFFFF");
            meepLabel.colorGradient = new TMPro.VertexGradient(top, top, bottom, bottom);
            meepLabel.enableVertexGradient = true;
            meepLabel.color = Color.white;

            if(Mod.otherMods.IsMeepHere)
            {
                meepLabel.SetText(string.Format(meepLabel.text, ""));
            }
            else
            {
                meepLabel.SetText(string.Format(meepLabel.text, STRINGS.UI.SETTINGSDIALOG.CONTENT.COLOREDMEEPS.MEEP_MISSING));
            }

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
            randoChance.Value = Mod.Settings.RandomDupeReplaceChance;
            coloredMeeps.On = Mod.Settings.ColoredMeeps;
        }

        public override void OnClickApply()
        {
            Mod.Settings.DebugTools = debugToggle.On;
            Mod.Settings.RefundActiveInk = refundActiveToggle.On;
            Mod.Settings.RefundBioInkKg = float.TryParse(refundKgInput.Text, out var kg) ? kg : 1f;
            Mod.Settings.RandomDupeReplaceChance = randoChance.GetRoundedValue();
            Mod.Settings.RandoDupePreset = Enum.TryParse<RandoDupeTier>(randoCycler.Value, out var result) ? result : RandoDupeTier.Default;
            Mod.Settings.RefundActiveInk = refundCycler.Value == "matching";
            Mod.Settings.RefundeInk = refundCycler.Value != "none";
            Mod.Settings.ColoredMeeps = coloredMeeps.On;

            Mod.SaveSettings();
            Deactivate();
        }
    }
}
