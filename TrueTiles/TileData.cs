using Newtonsoft.Json;

namespace TrueTiles
{
	public class TileData
	{
		[JsonIgnore]
		public string Root { get; set; }

		[JsonIgnore]
		public string AssetBundle { get; set; }

		public string MainTex { get; set; }

		public string TopTex { get; set; }

		public string MainSpecular { get; set; }

		public string TopSpecular { get; set; }

		public float[] MainSpecularColor { get; set; }

		public float[] TopSpecularColor { get; set; }

		public string NormalTex { get; set; }

		public float Frequency { get; set; }

		public bool Transparent { get; set; }

		public string AssetRoot { get; set; }
	}
}
