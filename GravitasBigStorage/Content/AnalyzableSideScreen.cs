using FUtility;
using System;
using UnityEngine;

namespace GravitasBigStorage.Content
{
    public class AnalyzableSideScreen : ButtonMenuSideScreen
    {
        public Analyzable analyzable;

        private ToolTip tooltip;
        private LocText locText;
        private KButton kButton;

        public override bool IsValidForTarget(GameObject target) => 
            !GravitasBigStorageUnlockManager.Instance.hasUnlockedTech &&
            target.TryGetComponent(out Analyzable analyzable) &&
            analyzable.storyTraitUnlocked;

        private void OnClick()
        {
            if (analyzable != null)
            {
                analyzable.OnSidescreenButtonPressed();
            }
        }

        private void Initialize()
        {
            if (buttonPrefab == null)
            {
                buttonContainer = transform.Find("Buttons").GetComponent<RectTransform>();
                buttonPrefab = buttonContainer.Find("Button").gameObject.GetComponent<KLayoutElement>();
            }

            var button = Util.KInstantiateUI(buttonPrefab.gameObject, buttonContainer.gameObject, true);

            kButton = button.GetComponentInChildren<KButton>();
            tooltip = button.GetComponentInChildren<ToolTip>();
            locText = button.GetComponentInChildren<LocText>();

            kButton.onClick += OnClick;
        }

        public override void SetTarget(GameObject target)
        {
            if (target == null)
            {
                Log.Warning("Invalid gameObject received on AnalyzableSideScreen");
            }

            if(target.TryGetComponent(out Analyzable analyzable))
            {
                this.analyzable = analyzable;
            }

            if(tooltip == null)
            {
                Initialize();
            }

            tooltip.SetSimpleTooltip(analyzable.SidescreenButtonTooltip);
            locText.SetText(analyzable.SidescreenButtonText);
        }
    }
}
