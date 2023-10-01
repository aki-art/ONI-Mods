namespace SpookyPumpkinSO.Content
{
	public class ModDb
	{
		public static SPFriendlyPumpkinFaces pumpkinFaces;

		public static void OnDbInit()
		{
			pumpkinFaces = new SPFriendlyPumpkinFaces();
		}
	}
}
