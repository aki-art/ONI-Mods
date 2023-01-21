using KSerialization;
using UnityEngine;

namespace Backwalls.Cmps
{
    public class ModStorage : KMonoBehaviour
    {
        public static ModStorage Instance;


        [Serialize]
        [SerializeField]
        public bool CopyColor;

        [Serialize]
        [SerializeField]
        public bool CopyPattern;

        [Serialize]
        [SerializeField]
        public bool ShowHSV;

        [Serialize]
        [SerializeField]
        public bool ShowSwatches;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
