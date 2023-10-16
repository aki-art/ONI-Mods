extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.SubTemplates
{
	// silly workaround to the original YAML's inconsistent casing
	[Serializable]
	public class Vector2IC : Dictionary<string, IntNumber>
	{
		public const string X = "X";
		public const string Y = "Y";

		public int Get(string keyUpperCase)
		{
			if (TryGetValue(keyUpperCase, out IntNumber number))
				return number.CalculateOrDefault(0);

			if (TryGetValue(keyUpperCase.ToLowerInvariant(), out number))
				return number.CalculateOrDefault(0);

			return 0;
		}

		public Vector2I ToVector2I() => new(Get(X), Get(Y));

		public static implicit operator Vector2IC(Vector2I vector) => new()
		{
			[X] = vector.X,
			[Y] = vector.Y
		};

		public override string ToString() => $"Vector2IC(X: {Get(X)} Y:{Get(Y)})";
	}
}
