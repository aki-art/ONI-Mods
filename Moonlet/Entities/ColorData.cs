using UnityEngine;

namespace Moonlet.Entities
{
	public struct ColorData
	{
		public static readonly ColorData White = new(1f, 1f, 1f);

		public ColorData(float r, float g, float b, float a = 1f) : this()
		{
			R = r;
			G = g;
			B = b;
			A = a;
			Hex = new Color(r, g, b, a).ToHexString();
		}

		public string Hex { get; set; }

		public float R { get; set; }

		public float G { get; set; }

		public float B { get; set; }

		public float A { get; set; }

		public readonly Color Get() => Hex.IsNullOrWhiteSpace() ? new Color(R, G, B, A) : Util.ColorFromHex(Hex);
	}
}
