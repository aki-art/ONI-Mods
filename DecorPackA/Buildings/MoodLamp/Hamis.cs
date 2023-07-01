using KSerialization;
using UnityEngine;
using static DecorPackA.STRINGS.UI.USERMENUACTIONS.HAMIS_MAID;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	internal class Hamis : KMonoBehaviour
	{
		[Serialize] public bool isMaid;
		[MyCmpReq] private DecorPackA.Buildings.MoodLamp.MoodLamp moodLamp;
		[MyCmpReq] private KBatchedAnimController kbac;

		public static readonly HashedString HAMIS_ID = "hamis";

		private static readonly string[] SYMBOLS = new[]
		{
			"maid_on",
			"maid_off"
		};

		public override void OnSpawn()
		{
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);

			RefreshSymbols();
		}

		private void OnMoodlampChanged(object data)
		{
			isMaid = data is HashedString moodLampId && moodLampId == HAMIS_ID;
			RefreshSymbols();
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out Hamis hamis))
			{
				isMaid = hamis.isMaid;
				moodLamp.RefreshAnimation();
			}
		}

		private void OnRefreshUserMenu(object obj)
		{
			if (moodLamp.currentVariantID == HAMIS_ID)
			{
				var text = isMaid ? DISABLED.NAME : ENABLED.NAME;
				var toolTip = isMaid ? DISABLED.TOOLTIP : ENABLED.TOOLTIP;

				var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggleMaid, tooltipText: toolTip);

				Game.Instance.userMenu.AddButton(gameObject, button);
			}
		}

		private void OnToggleMaid()
		{
			isMaid = !isMaid;
			RefreshSymbols();
		}

		public void RefreshSymbols()
		{
			foreach (var symbol in SYMBOLS)
				kbac.SetSymbolVisiblity(symbol, isMaid);
		}
	}
}
