/*using Backwalls.Integration.Blueprints;
using FUtility;
using KSerialization;
using System.Collections.Generic;

namespace Backwalls.Cmps
{
    // stuff to be remembered on a save file
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BackwallStorage : KMonoBehaviour
    {
        [Serialize]
        // was meant as a helper for blueprints integration
        public Dictionary<int, AdditionalData.BackwallData> data;

        public static BackwallStorage Instance { get; private set; }

        public BackwallStorage()
        {
            data = new Dictionary<int, AdditionalData.BackwallData>();
        }

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

#if DEBUG
        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (data != null && data.Count > 0)
            {
                Log.Debuglog("restoring planned backwalls: ");
                foreach (var item in data)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    Log.Debuglog($"{item.Key}, {item.Value.ColorHex}, {item.Value.Pattern}");
                }

            }
        }
#endif
    }
}
*/