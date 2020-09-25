/*using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace WorldTraitsPlus.Traits
{
    class WorldEventClock : KScreen, ISim33ms
    {
        public static WorldEventClock Instance;
        GameObject progressOverlay;
        private bool shown = false;
        public bool pause = true;
        public const float SCREEN_SORT_KEY = 300f;

        new bool ConsumeMouseScroll = true;
        public static void DestroyInstance()
        {
            Instance = null;
        }

        // Token: 0x06003AA6 RID: 15014 RVA: 0x0014B42F File Offset: 0x0014962F
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        public void Sim33ms(float dt)
        {
            throw new System.NotImplementedException();
        }
    }
}
*/