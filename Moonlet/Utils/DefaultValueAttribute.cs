using System;

namespace Moonlet.Utils
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DefaultValueAttribute(object defaultValue) : Attribute
	{
		public object defaultValue = defaultValue;
	}
}
