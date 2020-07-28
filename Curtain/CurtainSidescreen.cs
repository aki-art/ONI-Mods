using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace Curtain
{
    class CurtainSideScreen : SideScreenContent
    {
        private Curtain target;
        private KToggle openButton;
        private KToggle autoButton;
        private KToggle lockButton;
        private LocText description;
        private List<DoorButtonInfo> buttonList = new List<DoorButtonInfo>();

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<Curtain>() != null;
        }
        protected override void OnPrefabInit()
        {
            this.name = "CurtainToggleSidescreen";
            Debug.Log(gameObject.name);
            titleKey = "STRINGS.UI.UISIDESCREENS.CURTAIN_SIDE_SCREEN.TITLE";
            gameObject.SetActive(true);


            InitUIElements();
            InitButtons();

            gameObject.SetActive(false);
        }

        private void InitUIElements()
        {
            openButton = transform.Find("Contents/Buttons/Openbutton").GetComponent<KToggle>();
            autoButton = transform.Find("Contents/Buttons/AutoButton").GetComponent<KToggle>();
            lockButton = transform.Find("Contents/Buttons/CloseButton").GetComponent<KToggle>();
            description = transform.Find("Contents/Description").GetComponent<LocText>();
        }

        private void Refresh(Curtain.ControlState state)
        {
            target.QueueStateChange(state);
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            string text = null;

            foreach (var btn in buttonList)
            {
                bool requested = target.RequestedState == btn.state;
                bool current = target.CurrentState == btn.state;

                if (current && requested)
                {
                    text = btn.currentString;
                    RefreshButton(btn, true, false);
                }
                else if (requested)
                {
                    text = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, text, btn.pendingString);
                    RefreshButton(btn, true, true);
                }
                else
                    RefreshButton(btn, false, false);
            }

            description.text = text;

            /*foreach (DoorButtonInfo info in buttonList)
            {
                bool active = info.state != target.CurrentState;
                info.button.gameObject.SetActive(active);
            }*/
        }

        private static void RefreshButton(DoorButtonInfo btn, bool on, bool throb)
        {
            btn.button.isOn = on;

            foreach (ImageToggleState imageToggleState in btn.button.GetComponentsInChildren<ImageToggleState>())
                if (on)
                    imageToggleState.SetActive();
                else
                    imageToggleState.SetInactive();

            btn.button.GetComponent<ImageToggleStateThrobber>().enabled = throb;
        }

        public override void SetTarget(GameObject target)
        {
            if (target != null) ClearTarget();
            base.SetTarget(target);

            this.target = target.GetComponent<Curtain>();

            gameObject.SetActive(true);
            RefreshButtons();

            target.Subscribe((int)GameHashes.DoorStateChanged, OnDoorStateChanged);
        }

        private void OnDoorStateChanged(object data)
        {
            RefreshButtons();
        }

        private void InitButtons()
        {
            buttonList.Add(new DoorButtonInfo
            {
                button = openButton,
                state = Curtain.ControlState.Open,
                currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
                pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
            });

            buttonList.Add(new DoorButtonInfo
            {
                button = autoButton,
                state = Curtain.ControlState.Auto,
                currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
                pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
            });

            buttonList.Add(new DoorButtonInfo
            {
                button = lockButton,
                state = Curtain.ControlState.Locked,
                currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
                pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
            });

            foreach (var info in buttonList)
                info.button.onClick += delegate () { Refresh(info.state); };
        }

        private struct DoorButtonInfo
        {
            public KToggle button;
            public Curtain.ControlState state;
            public string currentString;
            public string pendingString;
        }
    }
}
