/*using Database;
using STRINGS;
using UnityEngine;

namespace Slag.Buildings
{
    public class Unravelable : KMonoBehaviour
    {
        [SerializeField]
        public bool isMarkedForUnravel;
        public static StatusItem MarkedForUnravel;
        public static StatusItem MarkedForUnravelInStorage;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            isMarkedForUnravel = GetComponent<KPrefabID>().HasTag(ModAssets.unravelable);
            if (isMarkedForUnravel)
                MarkForUnravel(true);

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
            Subscribe((int)GameHashes.OnStore, OnStoreDelegate);
        }

        private void MarkForUnravel(bool unravel)
        {
            isMarkedForUnravel = unravel;
            RefreshStatusItem();

            if (unravel)
            {
                Storage storage = GetComponent<Pickupable>().storage;
                if (storage != null)
                    storage.Drop(gameObject, true);
            }
        }

        private void OnToggleUnravel()
        {
            MarkForUnravel(!isMarkedForUnravel);

            if (!isMarkedForUnravel)
            {
                Pickupable pickupable = GetComponent<Pickupable>();
                if (pickupable.storage != null)
                    pickupable.storage.Drop(gameObject, true);
            }
            SelectTool.Instance.SelectNextFrame(GetComponent<KSelectable>(), true);
        }

        private void RefreshStatusItem()
        {
            KSelectable selectable = GetComponent<KSelectable>();

            if (selectable.HasStatusItem(MarkedForUnravel))
                selectable.RemoveStatusItem(MarkedForUnravel, false);
            //if (selectable.HasStatusItem(MarkedForUnravelInStorage))
            //    selectable.RemoveStatusItem(MarkedForUnravelInStorage, false);

            if (isMarkedForUnravel)
            {
                if (GetComponent<Pickupable>() != null)
                {
                    selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, MarkedForUnravel, null);
                    *//* else
                         selectable.AddStatusItem(ModAssets.statusItems.MarkedForUnravelInStorage, null);*//*
                }
            }
        }

        private void OnStore(object _)
        {
            RefreshStatusItem();
        }

        private void OnRefreshUserMenu(object _)
        {
            KIconButtonMenu.ButtonInfo buttonInfo;

            if (!isMarkedForUnravel)
            {
                buttonInfo = new KIconButtonMenu.ButtonInfo(
                    iconName: "action_compost",
                    text: "Unravel",
                    on_click: new System.Action(OnToggleUnravel));
            }
            else
            {
                buttonInfo = new KIconButtonMenu.ButtonInfo(
                    iconName: "action_compost",
                    text: "Cancel unravel",
                    on_click: new System.Action(OnToggleUnravel));
            }

            Game.Instance.userMenu.AddButton(gameObject, buttonInfo, 1f);
        }

        private static readonly EventSystem.IntraObjectHandler<Unravelable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Unravelable>(delegate (Unravelable component, object data)
        {
            component.OnRefreshUserMenu(data);
        });

        private static readonly EventSystem.IntraObjectHandler<Unravelable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Unravelable>(delegate (Unravelable component, object data)
        {
            component.OnStore(data);
        });
    }

}
*/