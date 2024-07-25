using System;

namespace Moonlet.DocGen
{
	public class DocEntry(string name, string description, string defaultValue, Type type)
	{
		public string name = name;
		public string description = description;
		public string defaultValue = defaultValue;
		public Type type = type;
		public string typeTitle;
	}
}
