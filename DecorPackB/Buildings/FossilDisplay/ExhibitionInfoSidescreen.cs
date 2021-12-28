using FUtility;
using UnityEngine;

namespace DecorPackB.Buildings.FossilDisplay
{
    public class ExhibitionInfoSidescreen : SideScreenContent
    {
        private LocText label;
        private GameObject button;
        private Assemblable target;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<Assemblable>() is object;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";

            var contents = transform.Find("Contents");
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
            Assemblable component = target.GetComponent<Assemblable>();
            if (component == null)
            {
                Log.Error("Target doesn't have a Assemblable associated with it.");
                return;
            }

            this.target = component;
            target.Subscribe((int)GameHashes.WorkableCompleteWork, OnWorkComplete);
            Refresh();
        }

        private void OnWorkComplete(object data)
        {
            Log.Debuglog("triggered");
            Refresh();
        }

        private void Refresh()
        {
            if(label is null || target is null) return;
            label.SetText(target.GetDescription());
        }
    }
}
