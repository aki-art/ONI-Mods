using TMPro;
using UnityEngine;

namespace FUtility.FUI
{
    class TMPFixer : KMonoBehaviour
    {
        [SerializeField]
        public TextAlignmentOptions alignment;

        [MyCmpReq]
        private LocText text;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            text.alignment = alignment;
            Destroy(this);
        }
    }
}
