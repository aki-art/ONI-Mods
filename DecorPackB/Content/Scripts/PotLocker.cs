using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class PotLocker : StorageLocker
	{
		[MyCmpReq] private Pot pot;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Initialize(false);

			Unsubscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings2);
		}

		private void OnCopySettings2(object obj)
		{
			if (!pot.ShouldShowSettings)
				return;

			if (obj is GameObject gameObject && gameObject.TryGetComponent(out StorageLocker locker))
			{
				if (locker.TryGetComponent(out Pot pot2) && !pot2.ShouldShowSettings)
					return;

				UserMaxCapacity = locker.UserMaxCapacity;
			}
		}
	}
}
