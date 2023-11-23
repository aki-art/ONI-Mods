/*using FUtility;
using UnityEngine;

namespace DecorPackB.Content.Buildings.FossilDisplays
{
    public class ExhibitionInfoSidescreen : SideScreenContent
    {
        private LocText label;
        private GameObject button;
        private IExhibition target;

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<IExhibition>() is object;
        }

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";

            var contents = transform.Find("Contents");
            label = contents.Find("Label")?.GetComponent<LocText>();
            button = contents.Find("Button")?.gameObject;

            label.alignment = TMPro.TextAlignmentOptions.TopLeft;

            button.SetActive(false);
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            Refresh();
        }

        public override void SetTarget(GameObject target)
        {
            var component = target.GetComponent<IExhibition>();
            if (component == null)
            {
                Log.Error("Target doesn't have a Assemblable associated with it.");
                return;
            }

            this.target = component;
            target.Subscribe((int)ModHashes.FossilStageSet, OnSetStage);
            Refresh();
        }

        private void OnSetStage(object data)
        {
            Log.Debuglog("triggered");
            Refresh();
        }

        private void Refresh()
        {
            if (label is null || target is null)
            {
                return;
            }

            label.SetText(target.GetDescription());
        }

        public override void OnActivate()
        {
            base.OnActivate();
            Refresh();
        }

    }
}
*/