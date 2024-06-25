using System;

namespace Moonlet.Utils
{
	public class EnumValidatorAttribute(Type type) : Attribute
	{
		public readonly Type type = type;
	}
}
