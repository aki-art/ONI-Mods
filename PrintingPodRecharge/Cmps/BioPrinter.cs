using FUtility;
using KSerialization;
using PrintingPodRecharge.Items;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
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

        public bool CanStartPrint() => !isPrinterBusy && HasEnoughInk();

        public bool HasEnoughInk() => storage.GetMassAvailable(inkTag) >= 2f;


        protected override void OnSpawn()
        {
            base.OnSpawn();

            Subscribe((int)ModHashes.PrintEvent, OnPrintEvent);
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);

            OnStorageChange(null);

            if(inkTag != Tag.Invalid)
            {
                SetDelivery(inkTag);
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }

        private void OnStorageChange(object _)
        {
            if(HasEnoughInk())
            {
                kSelectable.AddStatusItem(ModAssets.StatusItems.printReady);
                delivery.Pause(true, "Enough Ink");
            }

            RefreshSideScreen();
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

        public void CancelDelivery()
        {
            inkTag = Tag.Invalid;
            isDeliveryActive = false;

            delivery.requestedItemTag = Tag.Invalid;
            delivery.Pause(true, "Cancelled");

            storage.DropAll();

            RefreshSideScreen();
        }

        public void SetDelivery(Tag tag)
        {
            if(tag == Tag.Invalid)
            {
                CancelDelivery();
                return;
            }

            inkTag = tag;
            lastInkTag = tag;

            isDeliveryActive = true;

            var tagToKeep = new TagBits(tag);
            storage.DropUnlessHasTags(tagToKeep, tagToKeep, default);

            delivery.requestedItemTag = tag; // settings this auto-aborts previous
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
