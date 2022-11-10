using Database;

namespace PrintingPodRecharge.Content
{
    public class PAssignableSlots
    {
        public static AssignableSlot Book;
        public const string BOOK_ID = "PrintingPodRecharge_Book";

        public static void Register(AssignableSlots parent)
        {
            Book = parent.Add(new AssignableSlot(BOOK_ID, "Book", true));
        }
    }
}
