using FUtility;
using KSerialization;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTPIP_SPAWNER;

// Adds a button to telepads to call a pip
namespace SpookyPumpkinSO.Content.GhostPip.Spawning
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class GhostPipSpawner : KMonoBehaviour, ISidescreenButtonControl
    {
        [Serialize]
        private bool spawnComplete = false;
        private static Notifier notifier;

        public void SetSpawnComplete(bool value)
        {
            spawnComplete = value;
        }

        public const float SPAWN_DELAY = 1f;

        public string SidescreenButtonText => spawnComplete ? TEXT_INACTIVE : TEXT_ACTIVE;

        public string SidescreenButtonTooltip => spawnComplete ? TOOLTIP_INACTIVE : TOOLTIP;

        public int ButtonSideScreenSortOrder()
        {
            return 4;
        }

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

            var telepad = GetComponent<Telepad>();
            telepad.smi.sm.openPortal.Trigger(telepad.smi);

            // delay spawning so the portal has time for opening
            GameScheduler.Instance.Schedule("GhostPipArrival", SPAWN_DELAY, o => SpawnPip(o as GhostPipSpawner), this);
            spawnComplete = true;
        }

        private void SpawnPip(GhostPipSpawner spawner)
        {
            var pip = Spawn(GhostSquirrelConfig.ID, spawner.gameObject.transform.position);
            Utils.YeetRandomly(pip, true, 3, 5, false);
        }
        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            var prefab = Assets.GetPrefab(tag);

            if (prefab == null)
            {
                return null;
            }

            var go = GameUtil.KInstantiate(Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);
            return go;
        }


        public bool SidescreenButtonInteractable()
        {
            return !spawnComplete;
        }

        public bool SidescreenEnabled()
        {
            return !spawnComplete;
        }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
        {
            throw new System.NotImplementedException();
        }
    }
}
