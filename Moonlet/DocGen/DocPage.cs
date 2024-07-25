using System;
using System.Collections.Generic;

namespace Moonlet.DocGen
{
	public class DocPage(string path, string title, string description, Type type)
	{
		public List<DocEntry> entries = [];
		public string path = path;
		public string title = title;
		public string description = description;
		public Type type = type;
	}
}
