using Database;
using STRINGS;

namespace Twitchery.Content
{
	public class TChoreTypes
	{
		public static ChoreType pipEat;

		public static void Register(ChoreTypes parent)
		{
			parent.Add(
				"AkisExtraTwitchEvents_PipEat",
				new string[] { },
				"Eat",
				new string[] { },
				"Munch (Pip)",
				DUPLICANTS.CHORES.EAT.STATUS,
				DUPLICANTS.CHORES.EAT.TOOLTIP,
				false);
		}
	}
}
