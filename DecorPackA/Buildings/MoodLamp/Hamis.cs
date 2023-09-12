using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.USERMENUACTIONS.HAMIS_MAID;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Hamis : KMonoBehaviour
	{
		[Serialize] public bool isMaid;
		[MyCmpReq] private MoodLamp moodLamp;

		public static readonly string HAMIS_ID = "hamis";

		private static readonly HashSet<KAnimHashedString> SYMBOLS = new()
		{
			"maid_outfit_on",
			"maid_outfit_off"
		};

		public override void OnSpawn()
		{
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			Subscribe(ModEvents.OnMoodlampChanged, OnLampChanged);

			RefreshSymbols();
		}

		private void OnLampChanged(object data)
		{
			isMaid = LampVariant.TryGetData<string>(data, "LampId", out var id) && id == HAMIS_ID;
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
			moodLamp.lampKbac.BatchSetSymbolsVisiblity(SYMBOLS, isMaid);
		}
	}
}
