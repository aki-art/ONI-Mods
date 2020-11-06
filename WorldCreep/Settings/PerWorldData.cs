using UnityEngine;
using KSerialization;

namespace WorldCreep.Settings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class PerWorldData : KMonoBehaviour
    {
        [SerializeField]
        public WorldEventSettings worldSettings;

        [SerializeField]
        public bool OverridesDefaults = false;

        protected override void OnSpawn()
        {
            if(OverridesDefaults)
            {
                ModSettings.WorldEvents = worldSettings;
            }
        }

        public void Reset()
        {
            worldSettings = null;
            OverridesDefaults = false;
            ModSettings.LoadGlobalEventSettings();
        }
    }
}
