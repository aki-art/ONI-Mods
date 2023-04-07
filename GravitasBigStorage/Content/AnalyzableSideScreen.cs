using FUtility;
using FUtility.FUI;
using UnityEngine;

namespace GravitasBigStorage.Content
{
    public class AnalyzableSideScreen : ButtonMenuSideScreen
    {
        public Analyzable analyzable;

        private ToolTip tooltip;
        private LocText locText;
        private FButton button;

        public override bool IsValidForTarget(GameObject target) =>
            !GravitasBigStorageUnlockManager.Instance.hasUnlockedTech &&
            target.TryGetComponent(out Analyzable analyzable) &&
            analyzable.storyTraitUnlocked;

        private void Initialize()
        {
            Helper.ListChildren(transform);
            button = transform.Find("Contents/ButtonGroupPrefab/ButtonPrefab").gameObject.AddOrGet<FButton>();
            button.transform.parent.gameObject.SetActive(true);
            locText = button.GetComponentInChildren<LocText>();
            tooltip = Helper.AddSimpleToolTip(button.gameObject, "");

            button.OnClick += OnClick;
        }

        private void OnClick()
        {
            if (analyzable != null)
            {
                analyzable.OnSidescreenButtonPressed();
            }

            Refresh();
        }

        public override void SetTarget(GameObject target)
        {
            if (target == null)
            {
                Log.Warning("Invalid gameObject received on AnalyzableSideScreen");
            }

            if (target.TryGetComponent(out Analyzable analyzable))
            {
                this.analyzable = analyzable;
            }

            Refresh();
        }

        private void Refresh()
        {
            if (analyzable == null)
            {
                return;
            }

            if (locText == null)
            {
                Initialize();
            }

            tooltip.SetSimpleTooltip(analyzable.SidescreenButtonTooltip);
            locText.SetText(analyzable.SidescreenButtonText);
        }
    }
}
