using UnityEngine;

namespace ReviveADupe
{
	public class ReviveADupe_ReviveableCorpse : KMonoBehaviour, ISidescreenButtonControl
	{
		[MySmiReq] private DeathMonitor.Instance deathMonitor;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			GetComponent<DeathMonitor>();
		}

		public string SidescreenButtonText => "Revive";

		public string SidescreenButtonTooltip => "";

		public int ButtonSideScreenSortOrder() => 0;

		public int HorizontalGroupID() => -1;

		public void OnSidescreenButtonPressed()
		{
			var go = new GameObject("temporary carrier");
			var storage = go.AddComponent<MinionStorage>();
			var pos = transform.position;
			storage.SerializeMinion(gameObject);
			storage.DeserializeMinion(storage.GetStoredMinionInfo()[0].id, pos);

			Destroy(go);
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
		}

		public bool SidescreenButtonInteractable() => true;

		public bool SidescreenEnabled() => deathMonitor.IsDead();
	}
}
