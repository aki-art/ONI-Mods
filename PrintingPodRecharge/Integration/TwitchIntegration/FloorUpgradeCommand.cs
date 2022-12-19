#if TWITCH
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class FloorUpgradeCommand
    {
        public const string ID = "FloorUpgrade";

        public static bool Condition() => Mod.otherMods.IsDecorPackIHere;
        
        public static void Run(object data)
        {
            var go = new GameObject("Floor Upgrader");
            go.AddComponent<FloorUpgrader>();
            go.SetActive(true);
        }
    }
}
#endif
