using CrittersDropBones.Integration.TwitchIntegration.Commands;
using HarmonyLib;
using UnityEngine;

namespace CrittersDropBones.Integration.TwitchIntegration
{
    public class TITester : KMonoBehaviour
    {
        [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
        public static class BuildingConfigManager_OnPrefabInit_Patch
        {
            // Happens once per game launch, persistent between world loads
            public static void Prefix(BuildingConfigManager __instance)
            {
                __instance.gameObject.AddComponent<TITester>();
            }
        }

        private MessyMessHallCommand messHallSpawner;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            messHallSpawner = new MessyMessHallCommand();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 200, 200, 500));

            GUILayout.Box("Event Tests");
            if (GUILayout.Button("Spawn muckroots"))
            {
                messHallSpawner.Run();
            }

            GUILayout.EndArea();
        }
    }
}
