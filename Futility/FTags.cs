namespace FUtility
{
	public class FTags
	{
		// TagManager.Create creates or fetches the already registered tag, so double "creating" for each futility is not a big deal

		public static Tag 
			noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
			stainedGlass = TagManager.Create("DecorPackA_StainedGlass"), // MaterialColor mod uses this
			noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this
	}
}
