using FUtility;
using UnityEngine;

namespace DecorPackB.Buildings.FossilDisplay
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

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";

            Transform contents = transform.Find("Contents");
            label = contents.Find("Label")?.GetComponent<LocText>();
            button = contents.Find("Button")?.gameObject;

            label.alignment = TMPro.TextAlignmentOptions.TopLeft;

            button.SetActive(false);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Refresh();
        }

        public override void SetTarget(GameObject target)
        {
            IExhibition component = target.GetComponent<IExhibition>();
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
    }
}
