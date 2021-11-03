using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FScreen : KScreen
    {
        public const float SCREEN_SORT_KEY = 300f;

        new bool ConsumeMouseScroll = true; // do not remove
        private bool shown = false;
        public bool pause = true;

        public FButton cancelButton;
        public FButton confirmButton;
        public FButton XButton;
        public FButton SteamButton;
        public FButton GithubButton;

        protected override void OnPrefabInit()
        {
            SetObjects();
            activateOnSpawn = true;
            gameObject.SetActive(true);
        }

        public void SetKeys(string prefix)
        {
            foreach (LocText text in gameObject.GetComponentsInChildren(typeof(LocText), true))
            {
                if (text.text.StartsWith("{\"Alignment\""))
                {
                    TextContent data = JsonConvert.DeserializeObject<TextContent>(text.text);
                    string key = prefix + "." + data.Key;
                    text.key = key;
                    text.SetText(Strings.Get(key));
                    text.alignment = data.Alignment;
                    text.KForceUpdateDirty();
                }
            }
        }

        public virtual void SetObjects()
        {
            Text refsData = gameObject.GetComponent<Text>();
            if(refsData != null && refsData.text.StartsWith("{\"Cancel\""))
            {
                var buttonRefs = JsonConvert.DeserializeObject<Dictionary<string, string>>(refsData.text);
                SetButton("Cancel", ref cancelButton, buttonRefs);
                SetButton("Apply", ref confirmButton, buttonRefs);
                SetButton("X", ref XButton, buttonRefs);
                SetButton("Steam", ref SteamButton, buttonRefs);
                SetButton("Github", ref GithubButton, buttonRefs);

                Destroy(refsData);
            }
        }

        private void SetButton(string key, ref FButton button, Dictionary<string, string> buttonRefs)
        {
            if (buttonRefs.TryGetValue(key, out string path))
                button = transform.Find(path).FindOrAddComponent<FButton>();
        }

        public virtual void ShowDialog()
        {
            if (transform.parent.GetComponent<Canvas>() == null && transform.parent.parent != null)
                transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();

            if(cancelButton != null)
                cancelButton.OnClick += OnClickCancel;
            if (XButton != null)
                XButton.OnClick += OnClickCancel;
            if (confirmButton != null)
                confirmButton.OnClick += OnClickApply;
            if (GithubButton != null)
                GithubButton.OnClick += OnClickGithub;
            if (SteamButton != null)
                SteamButton.OnClick += OnClickSteam;
        }

        public void OnClickGithub() => Application.OpenURL("https://github.com/aki-art/ONI-Mods");
        public void OnClickSteam() => Application.OpenURL("https://steamcommunity.com/id/akisnothere/myworkshopfiles/?appid=457140");

        public virtual void OnClickCancel()
        {
            Reset();
            Deactivate();
        }

        public virtual void Reset()
        {
        }

        public virtual void OnClickApply()
        {
        }

#region generic kscreen behaviour
        protected override void OnCmpEnable()
        {
            base.OnCmpEnable();
            if (CameraController.Instance != null)
            {
                CameraController.Instance.DisableUserCameraControl = true;
            }
        }

        protected override void OnCmpDisable()
        {
            base.OnCmpDisable();
            if (CameraController.Instance != null)
            {
                CameraController.Instance.DisableUserCameraControl = false;
            }
            Trigger((int)GameHashes.Close, null);
        }

        public override bool IsModal()
        {
            return true;
        }

        public override float GetSortKey()
        {
            return SCREEN_SORT_KEY;
        }

        protected override void OnActivate()
        {
            OnShow(true);
        }

        protected override void OnDeactivate()
        {
            OnShow(false);
        }

        protected override void OnShow(bool show)
        {
            base.OnShow(show);
            if (pause && SpeedControlScreen.Instance != null)
            {
                if (show && !shown)
                {
                    SpeedControlScreen.Instance.Pause(false);
                }
                else
                {
                    if (!show && shown)
                    {
                        SpeedControlScreen.Instance.Unpause(false);
                    }
                }
                shown = show;
            }
        }

        public override void OnKeyDown(KButtonEvent e)
        {
            if (e.TryConsume(Action.Escape))
            {
                OnClickCancel();
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        public override void OnKeyUp(KButtonEvent e)
        {
            if (!e.Consumed)
            {
                KScrollRect scroll_rect = GetComponentInChildren<KScrollRect>();
                if (scroll_rect != null)
                {
                    scroll_rect.OnKeyUp(e);
                }
            }
            e.Consumed = true;
        }
        #endregion

        [Serializable]
        public struct TextContent
        {
            [JsonProperty]
            public TextAlignmentOptions Alignment { get; set; }
            [JsonProperty]
            public string Key { get; set; }
        }
    }
}
