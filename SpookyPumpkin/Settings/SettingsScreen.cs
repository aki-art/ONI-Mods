using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpookyPumpkin.Settings
{
    public class SettingsScreen : FScreen
    {
        public Dictionary<string, LabelInfo> labels;

        Toggle ghostPipToggle;
        Toggle rotToggle;
        Toggle lightToggle;
        FCycle timeCycle;
        FButton github;
        FButton steam;

        public class LabelInfo
        {
            public readonly TextMeshProUGUI label;
            public string Text
            {
                get => label.text;
                set => label.SetText(value);
            }

            public LabelInfo(string path, TextAlignmentOptions alignment, Component parent)
            {
                label = parent.transform.Find(path).GetComponent<TextMeshProUGUI>();
                label.alignment = alignment;
            }
        }

        public override void SetObjects()
        {
            #region Set Object paths
            cancelButton = transform.Find("Content/Content/Buttons/Cancel").gameObject.AddComponent<FButton>();
            confirmButton = transform.Find("Content/Content/Buttons/OK").gameObject.AddComponent<FButton>();
            github = transform.Find("Content/Content/Buttons/Github").gameObject.AddComponent<FButton>();
            steam = transform.Find("Content/Content/Buttons/Steam").gameObject.AddComponent<FButton>();
            XButton = transform.Find("TitleBar/XButton").gameObject.AddComponent<FButton>();

            timeCycle = transform.Find("Content/Content/Settings/Time").gameObject.AddComponent<FCycle>();
            lightToggle = transform.Find("Content/Content/Settings/LightToggle").gameObject.AddComponent<FToggle>().toggle;
            rotToggle = transform.Find("Content/Content/Settings/RotToggle").gameObject.AddComponent<FToggle>().toggle;
            ghostPipToggle = transform.Find("Content/Content/Settings/PipToggle").gameObject.AddComponent<FToggle>().toggle;

            labels = new Dictionary<string, LabelInfo>
            {
                { "title", new LabelInfo("TitleBar/Label", TextAlignmentOptions.Center, this) },
                { "cancel", new LabelInfo("Content/Content/Buttons/Cancel/Text", TextAlignmentOptions.Center, this) },
                { "apply", new LabelInfo("Content/Content/Buttons/OK/Text", TextAlignmentOptions.Center, this) },
                { "timeselect", new LabelInfo("Content/Content/Settings/Time/Cycle/Label", TextAlignmentOptions.MidlineLeft, this) },
                { "time", new LabelInfo("Content/Content/Settings/Time/Cycle", TextAlignmentOptions.Center, this) },
                { "piptoggle", new LabelInfo("Content/Content/Settings/PipToggle/Label", TextAlignmentOptions.MidlineLeft, this) },
                { "lighttoggle", new LabelInfo("Content/Content/Settings/LightToggle/Label", TextAlignmentOptions.MidlineLeft, this) },
                { "rottogle", new LabelInfo("Content/Content/Settings/RotToggle/Label", TextAlignmentOptions.MidlineLeft, this) },
                { "version", new LabelInfo("Content/Content/Buttons/Version", TextAlignmentOptions.MidlineLeft, this) }
            };
            #endregion

            timeCycle.Options = new List<string>
            {
                "October",
                "Always",
                "Never"
            };
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            github.OnClick += OnClickGithub;
            steam.OnClick += OnClickSteam;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            lightToggle.isOn = ModSettings.Settings.GhostPipEmitsLight;
            rotToggle.isOn = ModSettings.Settings.PumpkinRequiresRot;
            ghostPipToggle.isOn = ModSettings.Settings.SpawnGhostPip;
            timeCycle.Value = ModSettings.Settings.Spooks.ToString();
            labels["version"].Text = "v" + Log.GetVersion();
        }

        public override void OnClickApply()
        {
            bool lightChanged = ModSettings.Settings.GhostPipEmitsLight != lightToggle.isOn;
            bool rotChanged = ModSettings.Settings.PumpkinRequiresRot != rotToggle.isOn;
            bool pipChanged = ModSettings.Settings.GhostPipEmitsLight != ghostPipToggle.isOn;
            bool spookChanged = false;
            if (Enum.TryParse(timeCycle.Value, out UserSettings.SpooksSetting spook))
                spookChanged = ModSettings.Settings.Spooks != spook;

            bool restartneeded = lightChanged || rotChanged || pipChanged || spookChanged;

            if (restartneeded)
            {
                var restartDialog = ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject;
                var canvas = Global.Instance.globalCanvas;

                var screen = ((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(restartDialog, canvas));
                screen.PopupConfirmDialog(
                        title_text: "Restart needed",
                        text: "The game needs to be restarted for the changes to take effect.",
                        confirm_text: "Restart",
                        on_confirm: () => Apply(true),
                        cancel_text: "Cancel",
                        on_cancel: () => screen.Deactivate());
            }
            else Deactivate(); // the user didn't change anything
        }

        public void Apply(bool restart)
        {
            ModSettings.Settings.GhostPipEmitsLight = lightToggle.isOn;
            ModSettings.Settings.PumpkinRequiresRot = rotToggle.isOn;
            ModSettings.Settings.SpawnGhostPip = ghostPipToggle.isOn;

            if (Enum.TryParse(timeCycle.Value, out UserSettings.SpooksSetting spook))
                ModSettings.Settings.Spooks = spook;

            ModSettings.Save();

            if (restart)
                App.instance.Restart();
        }

        public void OnClickGithub() => Application.OpenURL("https://github.com/aki-art/ONI-Mods");
        public void OnClickSteam() => Application.OpenURL("https://steamcommunity.com/id/akisnothere/myworkshopfiles/?appid=457140");
    }
}
