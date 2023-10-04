using System;

namespace Moonlet.Utils
{
	/// <summary> Limits a template fields value to be within range.</summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RangeAttribute(float min, float max, bool enforce = true) : Attribute
	{
		/// <summary> If disabled, only a warning will be logged, the value won't be adjusted.</summary>
		public bool enforce = enforce;
		public float min = min, max = max;
	}
}
