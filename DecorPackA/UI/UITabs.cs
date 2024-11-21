using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
using FUtility.FUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.UI
{
	public class UITabs : KMonoBehaviour
	{
		private Transform tabPrefab;

		public List<GameObject> tabContentContainers;
		public List<Image> tabs;
		public int selectedIndex;
		private Color selectedColor = Util.ColorFromHex("3E4357");
		private Color normalColor = Util.ColorFromHex("2B2F3D");

		private void UpdateTabs(int newIndex, bool force)
		{
			if (newIndex == selectedIndex && !force)
				return;

			selectedIndex = newIndex;

			for (int i = 0; i < tabContentContainers.Count; i++)
				tabContentContainers[i].SetActive(i == newIndex);

			for (int i = 0; i < tabs.Count; i++)
				tabs[i].color = i == newIndex ? selectedColor : normalColor;
		}

		public void UpdateTabs() => UpdateTabs(selectedIndex, true);

		public void AddTab(string name, string prefabIcon, GameObject targetContentPanel)
		{
			if (tabPrefab == null)
				tabPrefab = transform.GetChild(0);

			tabs ??= [];
			tabContentContainers ??= [];

			var tab = Instantiate(tabPrefab, transform);

			var toggle = tab.gameObject.AddComponent<FButton>();
			toggle.normalColor = normalColor;
			toggle.hoverColor = Util.ColorFromHex("5C6585");
			tab.gameObject.SetActive(true);

			tab.GetChild(0).GetComponent<Image>().sprite = Def.GetUISprite(Assets.GetPrefab(prefabIcon)).first;

			Helper.AddSimpleToolTip(toggle.gameObject, name);
			var index = tabs.Count;

			toggle.OnClick += () => UpdateTabs(index, false);

			var image = tab.GetComponent<Image>();
			image.color = index == 0 ? selectedColor : normalColor;

			tabs.Add(image);
			tabContentContainers.Add(targetContentPanel);
		}

		public List<TabInfo> GetTabInfos(Transform contentPanel)
		{
			return
			[
				new TabInfo()
				{
					tabPath = "GlassSculptures",
					name = STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE.NAME,
					iconPrefabId = GlassSculptureConfig.ID,
					contentPanel = contentPanel.Find("GlassSculpturesContent").gameObject
				},
				new TabInfo()
				{
					tabPath = "GlassTiles",
					name = STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.NAME,
					iconPrefabId = DefaultStainedGlassTileConfig.DEFAULT_ID,
					contentPanel = contentPanel.Find("GlassTilesContent").gameObject
				},
				new TabInfo()
				{
					tabPath = "MoodLamps",
					name = STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP.NAME,
					iconPrefabId = MoodLampConfig.ID,
					contentPanel = contentPanel.Find("ModLampsContent").gameObject
				},
			];
		}

		public class TabInfo
		{
			public string tabPath;
			public string name;
			public string iconPrefabId;
			public GameObject contentPanel;
		}
	}
}
