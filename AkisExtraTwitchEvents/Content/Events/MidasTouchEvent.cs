using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class MidasTouchEvent : ITwitchEvent
    {
        public bool Condition(object data) => true;

        public string GetID() => "MidasTouch";

        public void Run(object data)
        {
            var go = new GameObject("MidasToucher");
            var midasToucher = go.AddComponent<MidasToucher>();
            midasToucher.lifeTime = 30f;
            midasToucher.radius = 2f;

            go.SetActive(true);
        }
    }
}
