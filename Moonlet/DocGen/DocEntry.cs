using System;
using System.Collections.Generic;

namespace Moonlet.DocGen
{
	public class DocEntry(string name, string description, string defaultValue, Type type, List<string> acceptedValues)
	{
		public string name = name;
		public string description = description;
		public int numericRepresenation;
		public string defaultValue = defaultValue;
		public Type type = type;
		public Type sourceType;
		public string typeTitle;
		public List<string> acceptedValues = acceptedValues;
	}
}
