using KSerialization;
using UnityEngine;

namespace ConfigurableStreaks
{
	public class StreakToggler : KMonoBehaviour
	{
		[MyCmpReq] private Light2D light2D;
		[Serialize] public bool showStreaks;
		[Serialize] public bool initialized;

		protected override void OnSpawn()
		{
			if (!initialized)
			{
				showStreaks = light2D.drawOverlay;
				initialized = true;
			}

			RefreshStreaks();
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		private void RefreshStreaks() => light2D.drawOverlay = showStreaks;

		private void OnToggle()
		{
			showStreaks = !showStreaks;
			RefreshStreaks();
		}

		private void OnCopySettings(object obj)
		{
			if (obj is GameObject go && go.TryGetComponent(out StreakToggler toggler))
			{
				toggler.showStreaks = showStreaks;
				toggler.RefreshStreaks();
			}
		}

		private void OnRefreshUserMenu(object obj)
		{
			var text = showStreaks
				? "Hide Streaks"
				: "Show Streaks";

			var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggle);

			Game.Instance.userMenu.AddButton(gameObject, button);
		}
	}
}
