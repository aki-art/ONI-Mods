using KSerialization;
using PrintingPodRecharge.Cmps;
using UnityEngine;

namespace PrintingPodRecharge.Items
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BundleModifier : KMonoBehaviour
    {
        [SerializeField]
        public ImmigrationModifier.Bundle bundle;
    }
}
