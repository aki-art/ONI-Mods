/*using UnityEngine;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class AquariumSideScreen : SideScreenContent
    {
        private FishTank target;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<FishTank>() != null;

        protected override void OnPrefabInit()
        {
            titleKey = "InteriorDecorationVolI.STRINGS.UI.UISIDESCREENS.AQUARIUM_SIDE_SCREEN.TITLE";
            FindElements();
        }

        public override void SetTarget(GameObject newTarget)
        {
            ClearTarget();
            base.SetTarget(newTarget);

            target = newTarget.GetComponent<FishTank>();
            gameObject.SetActive(true);

            //newTarget.Subscribe((int)GameHashes.DoorStateChanged, OnDoorStateChanged);
        }

        public override void ClearTarget()
        {
            target = null;
        }

        private void FindElements()
        {
        }
    }
}
*/