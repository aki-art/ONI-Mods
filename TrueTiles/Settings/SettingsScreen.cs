using FUtility;
using FUtility.FUI;
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

        List<PackEntry> entries;

        public override void SetObjects()
        {
            base.SetObjects();

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
            if (TexturePacksLoader.Instance == null)
            {
                Log.Warning("TEXTURE PACK LOADER WAS NULL");
                var loader = Global.Instance.FindOrAddComponent<TexturePacksLoader>();
                loader.LoadPacks(Utils.ModPath);
            }

            base.ShowDialog();

            entries = new List<PackEntry>();

            foreach (var pack in TexturePacksLoader.Instance.packs)
            {
                var row = Instantiate(entryPrefab, entryParent);
                row.gameObject.SetActive(true);
                row.SetTitle(pack.Name);
                row.SetIcon(pack.Icon);
                row.SetDescription(pack.Author, pack.TextureCount);
                row.SetEnabled(pack.Enabled);
                row.SetFolder(pack.Path);
                row.SetTooltip(pack.Description);

                row.FindOrAddComponent<ReorderableListElement>();
                row.Id = pack.Id;

                entries.Add(row);
            }
        }

        public override void OnClickApply()
        {
            // save enabled / disabled
            // save ordering info

            foreach(var pack in TexturePacksLoader.Instance.packs)
            {
                var entry = entries.Find(e => e.Id == pack.Id);
                if(entry != null)
                {
                    pack.Enabled = entry.IsEnabled();
                    pack.Order = entry.GetComponent<Transform>().GetSiblingIndex();
                }
            }

            TexturePacksLoader.Instance.Save();
            TileAssetLoader.Instance.ReloadAssets();

            Deactivate();
        }
    }
}
