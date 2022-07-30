using Backwalls.Integration.Blueprints;
using FUtility;
using KSerialization;
using System.Collections.Generic;

namespace Backwalls.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BackwallStorage : KMonoBehaviour
    {
        [Serialize]
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

            Log.Debuglog("restoring planned backwalls: ");
            foreach(var item in data)
            {
                Log.Debuglog(item.Key, item.Value.ColorHex, item.Value.Pattern);
            }
        }
#endif
    }
}
