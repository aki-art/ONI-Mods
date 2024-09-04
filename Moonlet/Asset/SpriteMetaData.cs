namespace Moonlet.Asset
{
	public class SpriteMetaData
	{
		public string ColorHex { get; set; } = "FFFFFF";
		public float PivotX { get; set; }
		public float PivotY { get; set; }

		public SpriteMetaData()
		{
			ColorHex = "FFFFFF";
		}
	}
}
