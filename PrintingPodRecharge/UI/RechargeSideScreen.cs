using FUtility;
using FUtility.FUI;
using PrintingPodRecharge.Cmps;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrintingPodRecharge.UI
{
    public class RechargeSideScreen : SideScreenContent
    {
        private TMP_Dropdown dropdown;
        private BioPrinter target;
        private LocText descriptionLabel;
        private LayoutElement descriptionBox;
        private FButton deliverButton;
        private FButton activateButton;
        private LocText deliverButtonLabel;

        public List<Option> options = new List<Option>();

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<BioPrinter>() != null;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            dropdown = transform.Find("Contents/BeakerSelector/Dropdown").GetComponent<TMP_Dropdown>();
            descriptionBox = transform.Find("Contents/Description").GetComponent<LayoutElement>();
            descriptionLabel = descriptionBox.transform.Find("Label").GetComponent<LocText>();
            deliverButton = transform.Find("Contents/Buttons/Deliver").FindOrAddComponent<FButton>();
            deliverButtonLabel = deliverButton.transform.Find("Text").FindOrAddComponent<LocText>();
            activateButton = transform.Find("Contents/Buttons/Activate").FindOrAddComponent<FButton>();

            var item = dropdown.transform.Find("Template/Viewport/Content/Item");

            // when converting to LocText these references were lost, so they need to be rebound
            dropdown.itemText = item.Find("Item Label").GetComponent<LocText>();
            dropdown.captionText = dropdown.transform.Find("Label").GetComponent<LocText>();
        }

        protected override void OnSpawn()
        {
            Log.Debuglog("ON SPAWN");

            base.OnSpawn();

            options.Clear();

            foreach (var ink in Assets.GetPrefabsWithTag(ModAssets.Tags.bioInk))
            {
                var option = new Option(options.Count, ink);
                options.Add(option);
            }

            RefreshOptions();

            dropdown.onValueChanged.AddListener(OnValueChanged);
            deliverButton.OnClick += OnDeliveryClicked;
            activateButton.OnClick += OnActivateClicked;

            if (GetSelection(dropdown.value) is Option selection)
            {
                RefreshDescription(selection);
            }

            Refresh();
        }

        public void Refresh()
        {
            var isReady = target.IsReady;

            activateButton.SetInteractable(isReady);
            deliverButton.SetInteractable(!isReady);

            if(target.IsDeliveryActive)
            {
                deliverButtonLabel.text = global::STRINGS.UI.CANCELPLACEINRECEPTACLE;
            }
            else
            {
                deliverButtonLabel.text = STRINGS.UI.BIOINKSIDESCREEN.CONTENTS.BUTTONS.DELIVER.TEXT;
            }
        }

        private void OnActivateClicked()
        {
            target.Print();
        }

        private Option GetSelection(int index) => options.Find(x => x.index == index);

        private void OnDeliveryClicked()
        {
            if(!target.IsDeliveryActive)
            {
                if (GetSelection(dropdown.value) is Option selection)
                {
                    target.DeliveryTag = selection.prefabID;
                }
            }
            else
            {
                target.DeliveryTag = Tag.Invalid;
            }

            Refresh();
        }

        private void OnValueChanged(int index)
        {
            if(GetSelection(index) is Option selection)
            {
                RefreshDescription(selection);
            }

            target.DeliveryTag = Tag.Invalid;
        }

        private void RefreshDescription(Option selection)
        {
            descriptionLabel.text = selection.description;
            descriptionBox.minHeight = descriptionLabel.renderedHeight + 10;
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);

            if (target == null)
            {
                Log.Warning("Statemachine targeting BioPrinter needs a BioPrinter components.");
                return;
            }

            this.target = target.GetComponent<BioPrinter>();

            RefreshOptions();
            Refresh();
        }

        public void RefreshOptions()
        {
            dropdown.options.Clear();

            var newOptions = new List<TMP_Dropdown.OptionData>();

            foreach (var option in options)
            {
                newOptions.Add(new TMP_Dropdown.OptionData(option.name, option.sprite));
            }

            dropdown.AddOptions(newOptions);
            dropdown.RefreshShownValue();
        }

        public class Option
        {
            public string name;
            public string description;
            public Sprite sprite;
            public int index;
            public Tag prefabID;
            public bool available;

            public Option(int index, GameObject go)
            {
                prefabID = go.PrefabID();
                name = go.GetProperName();
                description = go.GetComponent<InfoDescription>().description;
                sprite = Def.GetUISprite(go).first;
                this.index = index;
            }
        }
    }
}
