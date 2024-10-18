extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Templates.SubTemplates
{
	// silly workaround to the original YAML's inconsistent casing
	[Serializable]
	public class Vector2FC : Dictionary<string, FloatNumber>
	{
		public static readonly Vector2FC zero = new(0, 0);

		public const string X = "X";
		public const string Y = "Y";

		public Vector2FC() : base() { }

		public Vector2FC(float x, float y)
		{
			this[X] = x;
			this[Y] = y;
		}

		public DiscreteShadowCaster.Direction ToDirection()
		{
			var vector = ToVector2();

			return Mathf.Abs(vector.x) > Mathf.Abs(vector.y)
				? GetSnappedMgn(vector.x) == -1 ? DiscreteShadowCaster.Direction.West : DiscreteShadowCaster.Direction.East
				: GetSnappedMgn(vector.y) == -1 ? DiscreteShadowCaster.Direction.South : DiscreteShadowCaster.Direction.North;
		}

		private static int GetSnappedMgn(float number) => (int)(number / Mathf.Abs(number));

		public float Get(string keyUpperCase)
		{
			if (TryGetValue(keyUpperCase, out FloatNumber number))
				return number.CalculateOrDefault(0);

			if (TryGetValue(keyUpperCase.ToLowerInvariant(), out number))
				return number.CalculateOrDefault(0);

			return 0f;
		}

		public Vector2f ToVector2f() => new(Get(X), Get(Y));
		public Vector2 ToVector2() => new(Get(X), Get(Y));

		public static implicit operator Vector2FC(Vector2f vector) => new()
		{
			[X] = vector.X,
			[Y] = vector.Y
		};

		public override string ToString() => $"Vector2FC(X: {Get(X)} Y:{Get(Y)})";
	}
}
