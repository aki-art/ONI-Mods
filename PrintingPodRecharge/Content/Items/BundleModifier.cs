using KSerialization;
using PrintingPodRecharge.Content.Cmps;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BundleModifier : KMonoBehaviour
    {
        [SerializeField]
        public Bundle bundle;
    }
}
