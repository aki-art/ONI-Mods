using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldTraitsPlus
{
    public class WorldEventManager2 : KMonoBehaviour
    {
        public static WorldEventManager2 Instance { get; private set; }

        protected override void OnPrefabInit()
        {
            Instance = this;
        }

        public static void DestroyInstance() => Instance = null;
    }
}
