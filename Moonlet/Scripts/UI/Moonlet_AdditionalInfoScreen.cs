using UnityEngine;

namespace Moonlet.Scripts.UI
{
	public class Moonlet_AdditionalInfoScreen : KMonoBehaviour
	{
		private CollapsibleDetailContentPanel panel;
		private DetailsPanelDrawer traitsDrawer;
		private GameObject attributesLabelTemplate;
		private GameObject target;

		private bool initialized;

		public void Initialize(AdditionalDetailsPanel instance)
		{
			if (!initialized)
			{
				attributesLabelTemplate = instance.attributesLabelTemplate;
				panel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, instance.gameObject);
				traitsDrawer = new DetailsPanelDrawer(attributesLabelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
				panel.HeaderLabel.text = global::STRINGS.UI.CHARACTERCONTAINER_TRAITS_TITLE;

				initialized = true;
			}
		}

		public void SetTarget(GameObject target)
		{
			this.target = target;
			Refresh();
		}

		public void Hide()
		{
			if (panel != null)
				panel.gameObject.SetActive(false);
		}

		private void Refresh()
		{
			if (gameObject.activeSelf && target != null)
				RefreshLabels();
		}

		private void RefreshLabels()
		{
			if (target.TryGetComponent(out KPrefabID kPrefabId))
			{
				panel.gameObject.SetActive(true);
				panel.HeaderLabel.text = "Tags";

				traitsDrawer.BeginDrawing();

				traitsDrawer.NewLabel($"Prefab ID: <b>{kPrefabId.PrefabID()}</b>").Tooltip("Internal ID"); // TODO: string entry
				traitsDrawer.NewLabel($"Tags:");

				foreach (var tag in kPrefabId.Tags)
					traitsDrawer.NewLabel($"- {tag.Name}").Tooltip(tag.ProperNameStripLink());

				traitsDrawer.EndDrawing();
			}
			else
			{
				panel.gameObject.SetActive(false);
			}
		}
	}
}
