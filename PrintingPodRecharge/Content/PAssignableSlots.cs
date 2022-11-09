using Database;

namespace PrintingPodRecharge.Content
{
    public class PAssignableSlots
    {
        public static AssignableSlot Book;

        public static void Register(AssignableSlots parent)
        {
            Book = parent.Add(new EquipmentSlot("PrintingPodRecharge_Book", "Book", true));
        }
    }
}
