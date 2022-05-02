using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
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
                row.SetFolder(pack.Root);
                row.SetTooltip(pack.Description);

                row.FindOrAddComponent<ReorderableListElement>();

                row.Id = pack.Id;

                entries.Add(row);
            }
        }

        public override void OnClickApply()
        {
            foreach(var pack in TexturePacksManager.Instance.packs)
            {
                var entry = entries.Find(e => e.Id == pack.Key);
                if(entry != null)
                {
                    pack.Value.Enabled = entry.IsEnabled();
                    pack.Value.Order = entry.GetComponent<Transform>().GetSiblingIndex();
                }
            }

            Mod.Settings.SaveExternally = saveExternally.On; // just to remember last setting
            Mod.SaveConfig();

            Log.Debuglog("SAVED CONFIG: " + saveExternally.On);

            var savePath = saveExternally.On ? Mod.GetExternalSavePath() : Mod.GetLocalSavePath();

            Log.Debuglog("path " + savePath);

            TexturePacksManager.Instance.SavePacks(savePath);
            TileAssetLoader.Instance.ReloadAssets();

            Deactivate();
        }
    }
}
