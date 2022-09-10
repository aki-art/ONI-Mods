using Database;

namespace PrintingPodRecharge.Content
{
    public class PAssignableSlots
    {
        public static AssignableSlot shaker;

        public static void Register(AssignableSlots slots)
        {
            // OwnableSlot and EquippableSlot have no actual differences, so it doesnt matter which one is given here
            shaker = slots.Add(new OwnableSlot("ppr_ShakerSlot", "Shaker")); // TODO: LOC
        }
    }
}
