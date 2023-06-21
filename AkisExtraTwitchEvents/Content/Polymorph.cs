using UnityEngine;

namespace Twitchery.Content
{
	public class Polymorph : Resource
	{
		public string navigationType;
		public string animFile;
		public Vector2 balloonOffset;
		public string hatTrackerSymbol;
		public Vector2 hatOffset;

		public Polymorph(string critterId, string name, string animFile, string navigationType, Vector2 balloonOffset, string hatTrackerSymbol, Vector2 hatOffset) : base(critterId, name)
		{
			this.animFile = animFile;
			this.navigationType = navigationType;
			this.balloonOffset = balloonOffset;
			this.hatTrackerSymbol = hatTrackerSymbol;
			this.hatOffset = hatOffset;
		}

		public static class NavigatorType
		{
			public const string
				FLOOR = "Floor",
				FLYER = "Flyer",
				IMMOBILE = "Immobile";
		}
	}
}
