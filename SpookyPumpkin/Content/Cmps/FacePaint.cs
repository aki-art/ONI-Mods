using FUtility;
using KSerialization;

namespace SpookyPumpkinSO.Content.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class FacePaint : KMonoBehaviour
    {
        [MyCmpGet]
        private Accessorizer accessorizer;

        [Serialize]
        private string currentFaceAccessory;

        [Serialize]
        private string originalFaceAccessory;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if(currentFaceAccessory != null)
            {
                Apply(currentFaceAccessory);
            }
        }

        public void Apply(string accessory)
        {
            if (accessorizer != null)
            {
                var mouthSlot = Db.Get().AccessorySlots.Mouth;

                var original = accessorizer.GetAccessory(mouthSlot);
                var newMouth = mouthSlot.Lookup(accessory);

                accessorizer.RemoveAccessory(original);
                accessorizer.AddAccessory(newMouth);

                accessorizer.ApplyAccessories();

                originalFaceAccessory = original.Id;
                currentFaceAccessory = newMouth.Id;
            }
        }

        public void Restore()
        {
            if (accessorizer != null)
            {
                var mouthSlot = Db.Get().AccessorySlots.Mouth;
                var originalAccessory = mouthSlot.Lookup(originalFaceAccessory);

                if(originalAccessory == null)
                {
                    Log.Warning($"Could not restore accessory {originalFaceAccessory}, it was not found in the database.");
                    return;
                }

                accessorizer.RemoveAccessory(accessorizer.GetAccessory(mouthSlot));
                accessorizer.AddAccessory(originalAccessory);
                accessorizer.ApplyAccessories();

                currentFaceAccessory = null;
                originalFaceAccessory = null;
            }
        }
    }
}
