using FUtility;
using KSerialization;
using static SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTPIP_SPAWNER;

namespace SpookyPumpkinSO.Content.GhostPip.Spawning
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GhostPipSpawner : KMonoBehaviour, ISidescreenButtonControl
	{
		public const float SPAWN_DELAY = 1f;

		[Serialize] private bool spawnComplete = false;

		public void SetSpawnComplete(bool value) => spawnComplete = value;

		public string SidescreenButtonText => spawnComplete ? TEXT_INACTIVE : TEXT_ACTIVE;

		public string SidescreenButtonTooltip => spawnComplete ? TOOLTIP_INACTIVE : TOOLTIP;

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

			var telepad = GetComponent<Telepad>();
			telepad.smi.sm.openPortal.Trigger(telepad.smi);

			// delay spawning so the portal has time for opening
			GameScheduler.Instance.Schedule("GhostPipArrival", SPAWN_DELAY, o => SpawnPip(o as GhostPipSpawner), this);
			spawnComplete = true;
		}

		private void SpawnPip(GhostPipSpawner spawner)
		{
			var pip = Utils.Spawn(GhostSquirrelConfig.ID, spawner.gameObject.transform.position);
			Utils.YeetRandomly(pip, true, 3, 5, false);
		}

		public bool SidescreenButtonInteractable() => !spawnComplete;

		public bool SidescreenEnabled() => !spawnComplete;

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) => throw new System.NotImplementedException();

		public int HorizontalGroupID() => -1;
	}
}
