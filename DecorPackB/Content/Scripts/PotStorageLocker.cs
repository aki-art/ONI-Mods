namespace DecorPackB.Content.Scripts
{
	public class PotStorageLocker : StorageLocker
	{
		public PausableFilteredStorage pausableFilteredStorage;

		public void Pause() => pausableFilteredStorage.Pause();

		public void Resume() => pausableFilteredStorage.Resume();

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			log = new LoggerFS(nameof(PotStorageLocker));
			var choreType = Db.Get().ChoreTypes.Get(choreTypeID);
			pausableFilteredStorage = new PausableFilteredStorage(this, null, this, false, choreType);
			filteredStorage = pausableFilteredStorage;

			Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			Subscribe((int)GameHashes.OnStorageLockerSetupComplete, Refresh);
		}

		private void Refresh(object obj)
		{
			if (pausableFilteredStorage.isPaused)
				Pause();
			else
				Resume();
		}
	}
}
