using UnityEngine;

namespace Dowsing
{
    class DowsingSidescreen2 : SideScreenContent
    {
        private DowsingRod target;
        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<DowsingRod>() != null;
        }
        public override void SetTarget(GameObject target)
        {
            DowsingRod component = target.GetComponent<DowsingRod>();
            Initialize(component);
        }

        private void Initialize(DowsingRod target)
        {
            this.target = target;
            gameObject.SetActive(true);
            //FUtility.FUI.Helper.ReplaceAllText(gameObject);
        }
    }
}
