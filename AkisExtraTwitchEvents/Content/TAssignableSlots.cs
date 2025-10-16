using Database;

namespace Twitchery.Content
{
	public class TAssignableSlots
	{
		//public static AssignableSlot Pet;
		//public const string PET = "AkisExtraTwitchEvents_Pet";
		public static AssignableSlot Cure;
		public const string CURE = "AkisExtraTwitchEvents_Cure";

		public static void Register(AssignableSlots parent)
		{
			//Pet = parent.Add(new AssignableSlot(PET, "Pet", true));
			Cure = parent.Add(new AssignableSlot(CURE, "Cure", true));
		}
	}
}
