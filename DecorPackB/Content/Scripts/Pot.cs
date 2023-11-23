namespace DecorPackB.Content.Scripts
{
	public class Pot : Sculpture
	{
		[MyCmpReq] private Storage storage;
		[MyCmpReq] private PotStorageLocker storageLocker;
		public const string DEFAULT = "Default";

		public override void OnSpawn()
		{
			base.OnSpawn();
			UpdateStorage(CurrentStage);
		}

		private void UpdateStorage(string stage)
		{
			if (stage == DEFAULT)
			{
				storageLocker.Pause();
			}
			else
			{
				storageLocker.Resume();
			}

			// refreshes fetch chores
			//storage.Trigger((int)GameHashes.OnlyFetchMarkedItemsSettingChanged);
		}

		public override void SetStage(string stage_id, bool skip_effect)
		{
			if (stage_id != CurrentStage)
				UpdateStorage(stage_id);

			base.SetStage(stage_id, skip_effect);
		}

		public void SetRandomStage()
		{
			var potentialStages = Db.GetArtableStages().GetPrefabStages(this.PrefabID());

			potentialStages.RemoveAll(stage => stage.statusItem.StatusType != Database.ArtableStatuses.ArtableStatusType.LookingGreat);
			var selectedStage = potentialStages.GetRandom();

			SetStage(selectedStage.id, false);
		}
	}
}
