using Database;

namespace Twitchery.Content
{
	public class TAssignableSlots
	{
		public static AssignableSlot Pet;
		public const string PET = "AkisExtraTwitchEvents_Pet";

		public static void Register(AssignableSlots parent)
		{
			Pet = parent.Add(new AssignableSlot(PET, "Pet", true));
		}
	}
}
