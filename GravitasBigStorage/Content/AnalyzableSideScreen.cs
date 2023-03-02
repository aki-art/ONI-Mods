using FUtility;
using UnityEngine;

namespace GravitasBigStorage.Content
{
    public class AnalyzableSideScreen : ButtonMenuSideScreen
    {
        public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out Analyzable _);

        public Analyzable analyzable;

        public override void SetTarget(GameObject target)
        {
            if (target == null)
            {
                Log.Warning("Invalid gameObject received on AnalyzableSideScreen");
            }

            target.TryGetComponent(out Analyzable analyzable);
            Refresh();
        }

        private void Refresh()
        {
            var button = Util.KInstantiateUI(buttonPrefab, buttonContainer.gameObject, true);

            var kButton = button.GetComponentInChildren<KButton>();
            var tooltip = button.GetComponentInChildren<ToolTip>();
            var locText = button.GetComponentInChildren<LocText>();

            kButton.onClick += analyzable.OnSidescreenButtonPressed;
            tooltip.SetSimpleTooltip(analyzable.SidescreenButtonTooltip);
            locText.SetText(analyzable.SidescreenButtonText);
        }
    }
}
