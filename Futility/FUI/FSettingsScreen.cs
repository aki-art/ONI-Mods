using UnityEngine;

namespace FUtility.FUI
{
	public class FSettingsScreen : FScreen
	{
		private Transform checkboxPanelPrefab;

		public FToggle2 AddCheckBox(string label)
		{
			var panel = Instantiate(checkboxPanelPrefab);
			var checkBox = panel.gameObject.AddComponent<FToggle2>();
			checkBox.SetCheckmark("Background/Checkmark");

			panel.Find("Label").GetComponent<LocText>().SetText(label);

			return checkBox;
		}

		public override void SetObjects()
		{
			checkboxPanelPrefab = transform.Find("Content/TogglePanel");
			base.SetObjects();
		}

		protected override void OnShow(bool show)
		{
			base.OnShow(show);
		}

	}
}
