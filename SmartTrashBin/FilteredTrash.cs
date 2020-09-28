using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartTrashBin
{
    public class FilteredTrash
    {
        private KMonoBehaviour root;
        private FetchList2 fetchList;
        private TreeFilterable filterable;
        private Storage storage;
        public Color32 filterTint = FilteredStorage.FILTER_TINT;
        public Color32 noFilterTint = FilteredStorage.NO_FILTER_TINT;
        private Tag[] requiredTags;
        private Tag[] forbiddenTags;

        private static StatusItem noFilterStatusItem;
        private ChoreType choreType;

        public FilteredTrash(KMonoBehaviour root, Tag[] required_tags, Tag[] forbidden_tags, ChoreType fetch_chore_type)
        {
            this.root = root;
            requiredTags = required_tags;
            forbiddenTags = forbidden_tags;
            choreType = fetch_chore_type;

            root.Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
            root.Subscribe((int)GameHashes.UserSettingsChanged, FilterChanged);

            filterable = root.FindOrAdd<TreeFilterable>();
            filterable.OnFilterChanged += OnFilterChanged;

            storage = root.GetComponent<Storage>();
            storage.Subscribe((int)GameHashes.OnlyFetchMarkedItemsSettingChanged, FilterChanged);

            noFilterStatusItem = new StatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
        }

        public void CleanUp()
        {
            if (filterable != null)
            {
                TreeFilterable treeFilterable = filterable;
                treeFilterable.OnFilterChanged -= OnFilterChanged;
            }
            if (fetchList != null)
            {
                fetchList.Cancel("Parent destroyed");
            }
        }

        public void FilterChanged(object _ = null)
        {
            OnFilterChanged(filterable.GetTags());
        }

        private void OnStorageChanged(object _)
        {
            if (fetchList == null)
            {
                OnFilterChanged(filterable.GetTags());
            }
        }

        private void OnFetchComplete()
        {
            OnFilterChanged(filterable.GetTags());
        }

        public void UpdateTags(float treshold)
        {
            List<Tag> forbidden = new List<Tag>();
            Tag[] tags = filterable.GetTags();
            foreach (var tag in tags)
            {
                if (WorldInventory.Instance.GetTotalAmount(tag) <= treshold)
                {
                    forbidden.Add(tag);
                }
            }

            if (forbidden.Count == 0) return;
            List<Tag> oldForbidden = new List<Tag>(fetchList.FetchOrders[0].ForbiddenTags);
            if (oldForbidden.SequenceEqual(forbidden)) return;

            Debug.Log("change has occured");

            forbiddenTags = forbidden.ToArray();
            OnFilterChanged(tags);
        }

        private void OnFilterChanged(Tag[] tags)
        {
            KAnimControllerBase component = root.GetComponent<KBatchedAnimController>();
            bool hasTags = tags != null && tags.Length != 0;
            component.TintColour = (hasTags ? filterTint : noFilterTint);

            if (fetchList != null)
            {
                fetchList.Cancel("");
                fetchList = null;
            }

            float maxCapacityMinusStorageMargin = storage.capacityKg - storage.storageFullMargin;
            float spaceLeft = Mathf.Max(0f, maxCapacityMinusStorageMargin - storage.MassStored());
            if (spaceLeft > 0f && hasTags)
            {
                spaceLeft = Mathf.Max(0f, storage.capacityKg - storage.MassStored());
                fetchList = new FetchList2(storage, choreType)
                {
                    ShowStatusItem = true
                };

                fetchList.Add(tags, requiredTags, forbiddenTags, spaceLeft, FetchOrder2.OperationalRequirement.Functional);
                fetchList.Submit(OnFetchComplete, false);
            }

            root.GetComponent<KSelectable>().ToggleStatusItem(noFilterStatusItem, !hasTags, this);
        }
    }
}