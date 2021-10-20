

using TMPro;
using UnityEngine;

namespace PollingOfONIUITest
{
    public class SettingsDemoDialog : KScreen
    {
        // most of this is copypaste from the game-s KScreens, and covers basic functions for dialogs
        private bool shown = false;
        public bool pause = true;
        public const float SCREEN_SORT_KEY = 300f;
        new bool ConsumeMouseScroll = true;

        private FButton cancelButton;
        private FButton XButton;
        private FButton confirmButton;
        private FButton githubButton;
        private FButton steamButton;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            cancelButton = gameObject.transform.Find("Content/Buttons/Cancel").gameObject.AddComponent<FButton>();
            confirmButton = gameObject.transform.Find("Content/Buttons/OK").gameObject.AddComponent<FButton>();
            githubButton = gameObject.transform.Find("Content/Buttons/Github").gameObject.AddComponent<FButton>();
            steamButton = gameObject.transform.Find("Content/Buttons/Steam").gameObject.AddComponent<FButton>();
            XButton = gameObject.transform.Find("TitleBar/XButton").gameObject.AddComponent<FButton>();

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            cancelButton.OnClick += Deactivate;
            XButton.OnClick += Deactivate;
            confirmButton.OnClick += Deactivate;
            steamButton.OnClick += OnClickGithub;
            githubButton.OnClick += OnClickSteam;
        }

        public void OnClickGithub() => Application.OpenURL("https://www.youtube.com/watch?v=pDOp8yH041g");
        public void OnClickSteam() => Application.OpenURL("https://www.youtube.com/watch?v=h-0HRs0FY2U");
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

        public override bool IsModal() => true;
        public override float GetSortKey() => SCREEN_SORT_KEY;
        protected override void OnActivate() => OnShow(true);
        protected override void OnDeactivate() => OnShow(false);

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
                Deactivate();
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
    }
}