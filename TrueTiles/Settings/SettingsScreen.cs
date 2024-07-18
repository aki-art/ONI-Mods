using FUtility;
using FUtility.FUI;
using System.Collections.Generic;
using System.IO;
using TrueTiles.Cmps;
using TrueTiles.Settings.Unity_UI_Extensions.Scripts.Controls.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

namespace TrueTiles.Settings
{
	public class SettingsScreen : FScreen
	{
		private PackEntry entryPrefab;
		private Transform entryParent;
		private FToggle2 saveExternally;
		private List<PackEntry> entries;

		public override void SetObjects()
		{
			base.SetObjects();

			saveExternally = transform.Find("Buttons/ExternalSaveConfirm").FindOrAddComponent<FToggle2>();
			saveExternally.mark = saveExternally.transform.Find("Background/Checkmark").GetComponent<Image>();
			Helper.AddSimpleToolTip(saveExternally.gameObject, STRINGS.UI.SETTINGSDIALOG.BUTTONS.EXTERNALSAVECONFIRM.TOOLTIP);

			entryParent = transform.Find("ScrollView/Viewport/Content");

			entryPrefab = entryParent.Find("Entry").gameObject.AddComponent<PackEntry>();
			entryPrefab.gameObject.SetActive(false);

			var reorderable = entryParent.FindOrAddComponent<ReorderableList>();
			reorderable.ContentLayout = entryParent.FindComponent<VerticalLayoutGroup>();
			reorderable.IsDraggable = true;
			reorderable.IsDropable = true;

			transform.Find("VersionLabel").GetComponent<LocText>().text = "v" + Log.GetVersion();
		}

		public override void ShowDialog()
		{
			saveExternally.On = Mod.Settings.SaveExternally;

			base.ShowDialog();

			entries = new List<PackEntry>();

			foreach (var pack in TexturePacksManager.Instance.packs)
			{
				if (!pack.IsValid)
					continue;

				var row = Instantiate(entryPrefab, entryParent);
				row.gameObject.SetActive(true);
				row.SetTitle(pack.Name);
				row.SetIcon(pack.Icon);
				row.SetDescription(pack.Author, pack.TextureCount);
				row.SetEnabled(pack.Enabled);
				row.SetFolder(pack.Root);
				row.SetTooltip(pack.Description);

				row.FindOrAddComponent<ReorderableListElement>();

				row.Id = pack.Id;

				entries.Add(row);
			}
		}

		public override void OnClickApply()
		{
			if (!saveExternally.On && Directory.Exists(Mod.GetExternalSavePath()))
				Directory.Delete(Mod.GetExternalSavePath(), true);

			SaveAll();
		}

		private void SaveAll()
		{
			foreach (var pack in TexturePacksManager.Instance.packs)
			{
				var entry = entries.Find(e => e.Id == pack.Id);
				if (entry != null)
				{
					pack.Enabled = entry.IsEnabled();
					pack.Order = entry.GetComponent<Transform>().GetSiblingIndex();
				}
			}

			Mod.Settings.SaveExternally = saveExternally.On; // just to remember last setting
			Mod.SaveConfig();

			var savePath = saveExternally.On ? Mod.GetExternalSavePath() : Mod.GetLocalSavePath();

			TexturePacksManager.Instance.SavePacks(savePath);
			TileAssetLoader.Instance.ReloadAssets();

			Deactivate();
		}
	}
}
