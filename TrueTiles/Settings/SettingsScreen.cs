using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace TrueTiles.Settings
{
    public class SettingsScreen : FScreen
    {
        private PackEntry entryPrefab;
        private Transform entryParent;
        private FToggle2 saveExternally;

        List<PackEntry> entries;

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
        }

        public override void ShowDialog()
        {
            saveExternally.On = Mod.Settings.SaveExternally;

            base.ShowDialog();

            entries = new List<PackEntry>();

            foreach (var item in TexturePacksManager.Instance.packs)
            {
                var pack = item.Value;

                var row = Instantiate(entryPrefab, entryParent);
                row.gameObject.SetActive(true);
                row.SetTitle(pack.Name);
                row.SetIcon(pack.Icon);
                row.SetDescription(pack.Author, pack.TextureCount);
                row.SetEnabled(pack.Enabled);
                row.SetFolder(pack.DataPath);
                row.SetTooltip(pack.Description);

                row.FindOrAddComponent<ReorderableListElement>();

                row.Id = pack.Id;

                entries.Add(row);
            }
        }

        public override void OnClickApply()
        {
            foreach(var pack in TexturePacksLoader.Instance.packs)
            {
                var entry = entries.Find(e => e.Id == pack.Id);
                if(entry != null)
                {
                    pack.Enabled = entry.IsEnabled();
                    pack.Order = entry.GetComponent<Transform>().GetSiblingIndex();
                }
            }

            Mod.Settings.SaveExternally = saveExternally.On;
            Mod.SaveConfig();

            //TexturePacksLoader.Instance.Save(saveExternally.On);
            TileAssetLoader.Instance.ReloadAssets();

            Deactivate();
        }
    }
}
