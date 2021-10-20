using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Adds a button to telepads to call a pip
namespace SpookyPumpkinSO.GhostPip.Spawning
{
    class GhostPipSpawner : KMonoBehaviour, ISidescreenButtonControl
    {
        [Serialize]
        bool spawnComplete = false;
        private static Notifier notifier;

        public const float SPAWN_DELAY = 1f;

        public string SidescreenButtonText => spawnComplete ? "(Completed)" : "Answer mysterious call";

        public string SidescreenButtonTooltip => "Spooky Squeaks are whispered through the receiver... Wait, since when does this thing have a speaker??!";

        public int ButtonSideScreenSortOrder() => 4;

        public void OnSidescreenButtonPressed()
        {
            // visually open portal for a split second
            var kbac = GetComponent<KBatchedAnimController>();

            if (kbac.currentAnim == "idle")
            {
                kbac.Play("working_pre");
                kbac.Queue("working_loop");
                kbac.Queue("working_pst");
            }

            Telepad telepad = GetComponent<Telepad>();
            telepad.smi.sm.openPortal.Trigger(telepad.smi);

            // delay spawning so the portal has time for opening
            GameScheduler.Instance.Schedule("GhostPipArrival", SPAWN_DELAY, o => SpawnPip(o as GhostPipSpawner), this);
            spawnComplete = true;
        }

        private void SpawnPip(GhostPipSpawner spawner)
        {
            GameObject pip = Spawn(GhostSquirrelConfig.ID, spawner.gameObject.transform.position);
            Utils.YeetRandomly(pip, true, 3, 7, false);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if(!spawnComplete)
            {
                var notification = new Notification(
                    title: "You got a call!",
                    type: NotificationType.Good,
                    tooltip: null,
                    tooltip_data: null,
                    expires: true,
                    delay: 0f,
                    custom_click_callback: null,
                    custom_click_data: null,
                    click_focus: gameObject.transform);

                notifier = gameObject.AddComponent<Notifier>();
                notifier.Add(notification);
            }
        }

        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            GameObject prefab = Assets.GetPrefab(tag);

            if (prefab == null) return null;

            GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);
            return go;
        }


        public bool SidescreenButtonInteractable() => !spawnComplete;

        public bool SidescreenEnabled() => !spawnComplete;
    }
}
