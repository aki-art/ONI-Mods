namespace Twitchery.Content
{
	public class Polymorph : Resource
	{
		public int navigationType;
		public string animFile;

		public Polymorph(string critterId, string name, string animFile, int navigationType) : base(critterId, name)
		{
			this.animFile = animFile;
			this.navigationType = navigationType;
		}

		public static class NavigatorType
		{
			public const int
				FLOOR = 0,
				FLYER = 1;
		}
	}
}
