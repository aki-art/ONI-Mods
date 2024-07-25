using System;

namespace Moonlet.Utils
{
	public class DocAttribute(object message, Type enumTypeCheck = null) : Attribute
	{
		public readonly string message = $"{message}";
		public readonly Type enumTypeCheck = enumTypeCheck;
	}
}
