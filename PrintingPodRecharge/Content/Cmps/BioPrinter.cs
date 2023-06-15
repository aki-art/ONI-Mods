using FUtility;
using KSerialization;
using PrintingPodRecharge.Content.Items;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BioPrinter : KMonoBehaviour
    {
        [Serialize]
        public Tag inkTag;

        [Serialize]
        public Tag lastInkTag;

        [Serialize]
        public bool isPrinterBusy;

        [SerializeField]
        public Storage storage;

        [MyCmpReq]
        private KSelectable kSelectable;

        [MyCmpReq]
        private ManualDeliveryKG delivery;

        [Serialize]
        public bool isDeliveryActive;

        // mostly here for Survival Not Included compatibility
        private bool hadEnoughInk = false;

        public bool CanStartPrint()
        {
            return !isPrinterBusy && HasEnoughInk();
        }

        public bool HasEnoughInk()
        {
            return storage.GetMassAvailable(inkTag) >= 2f;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Subscribe((int)ModHashes.PrintEvent, OnPrintEvent);
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);

            OnStorageChange(null);

            if (inkTag != Tag.Invalid)
            {
                SetDelivery(inkTag);
            }
            else
            {
                CancelDelivery(false);
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }

        private void OnStorageChange(object _)
        {
            if (HasEnoughInk())
            {
                kSelectable.AddStatusItem(ModAssets.StatusItems.printReady);
                delivery.Pause(true, "Enough Ink");

                if (!hadEnoughInk)
                {
                    RefreshSideScreen();
                }

                hadEnoughInk = true;
            }
            else
            {
                hadEnoughInk = false;
            }
        }

        private void OnPrintEvent(object obj)
        {
            isPrinterBusy = (bool)obj;
            RefreshSideScreen();
        }

        public void RefreshSideScreen()
        {
            if (kSelectable.IsSelected)
            {
                DetailsScreen.Instance.Refresh(gameObject);
            }
        }

        public void CancelDelivery(bool dropStorage = true)
        {
            inkTag = Tag.Invalid;
            isDeliveryActive = false;

            delivery.RequestedItemTag = Tag.Invalid;
            delivery.Pause(true, "Cancelled");

            if (dropStorage)
            {
                storage.DropAll();
            }

            RefreshSideScreen();
        }

        public void SetDelivery(Tag tag)
        {
            if (tag == Tag.Invalid)
            {
                CancelDelivery();
                return;
            }

            inkTag = tag;
            lastInkTag = tag;

            isDeliveryActive = true;

            storage.DropUnlessHasTag(tag);

            delivery.RequestedItemTag = tag; // settings this auto-aborts previous
            delivery.Pause(false, "New Ink Selected");
            delivery.RequestDelivery();

            RefreshSideScreen();
        }

        public void StartBonusPrint()
        {
            if (storage.FindFirst(inkTag) is GameObject ink)
            {
                if (ink.GetComponent<PrimaryElement>().Mass < 2f)
                {
                    Log.Warning("Tried to bonus print, but there is not enough Ink in the Printer.");
                    return;
                }

                ImmigrationModifier.Instance.IsOverrideActive = true;
                Immigration.Instance.timeBeforeSpawn = 0;

                if (ink.TryGetComponent(out BundleModifier modifier))
                {
                    ImmigrationModifier.Instance.SetModifier(modifier.bundle);
                    storage.ConsumeIgnoringDisease(ink);
                    kSelectable.RemoveStatusItem(ModAssets.StatusItems.printReady);
                    CancelDelivery();
                }
            }

            RefreshSideScreen();
        }
    }
}
