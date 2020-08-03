using System.Collections.Generic;
using UnityEngine;
using static Curtain.Curtain;
using static STRINGS.UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN;

namespace Curtain
{
    class CurtainSideScreen : SideScreenContent
    {
        private Curtain target;
        private KToggle openButton;
        private KToggle autoButton;
        private KToggle lockButton;
        private LocText description;
        private List<CurtainButtonInfo> buttonList = new List<CurtainButtonInfo>();

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<Curtain>() != null;

        protected override void OnPrefabInit()
        {
            titleKey = "STRINGS.UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.TITLE";
            FindElements();
            InitButtons();
        }

        public override void SetTarget(GameObject newTarget)
        {
            ClearTarget();
            base.SetTarget(newTarget);

            target = newTarget.GetComponent<Curtain>();
            gameObject.SetActive(true);
            RefreshButtons();

            newTarget.Subscribe((int)GameHashes.DoorStateChanged, OnDoorStateChanged);
        }

        public override void ClearTarget()
        {
            if (target != null)
                target.Unsubscribe((int)GameHashes.DoorStateChanged, OnDoorStateChanged);

            target = null;
        }

        private void FindElements()
        {
            openButton = transform.Find("Contents/Buttons/Openbutton").GetComponent<KToggle>();
            autoButton = transform.Find("Contents/Buttons/AutoButton").GetComponent<KToggle>();
            lockButton = transform.Find("Contents/Buttons/CloseButton").GetComponent<KToggle>();
            description = transform.Find("Contents/Description").GetComponent<LocText>();
        }

        private void Refresh(ControlState state)
        {
            target.QueueStateChange(state);
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            string text = null;
            foreach (var button in buttonList)
            {
                bool requested = target.RequestedState == button.state;
                bool current = target.CurrentState == button.state;

                if (requested)
                    text = current ?
                        button.currentString : 
                        string.Format(PENDING_FORMAT, text, button.pendingString);

                SetButton(button, requested, !current && requested);
            }

            description.text = text;
        }

        private static void SetButton(CurtainButtonInfo btn, bool on, bool throb)
        {
            foreach (var imageToggle in btn.button.GetComponentsInChildren<ImageToggleState>())
                if (on)
                    imageToggle.SetActive();
                else
                    imageToggle.SetInactive();

            btn.button.isOn = on;
            btn.button.GetComponent<ImageToggleStateThrobber>().enabled = throb;
        }

        private void OnDoorStateChanged(object data) => RefreshButtons();

        private void InitButtons()
        {
            buttonList = new List<CurtainButtonInfo>()
            {
                new CurtainButtonInfo(openButton, ControlState.Open, OPEN, OPEN_PENDING),
                new CurtainButtonInfo(autoButton, ControlState.Auto, AUTO, AUTO_PENDING),
                new CurtainButtonInfo(lockButton, ControlState.Locked, CLOSE, CLOSE_PENDING)
            };

            foreach (var info in buttonList)
                info.button.onClick += delegate () { Refresh(info.state); };
        }

        private struct CurtainButtonInfo
        {
            public KToggle button;
            public ControlState state;
            public string currentString;
            public string pendingString;

            public CurtainButtonInfo(KToggle button, ControlState state, string currentString, string pendingString)
            {
                this.button = button;
                this.state = state;
                this.currentString = currentString;
                this.pendingString = pendingString;
            }
        }
    }
}
