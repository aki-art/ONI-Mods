using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.DocGen
{
	public class DocPage
	{
		public List<DocEntry> entries;
		public string path;
		public string title;
		public string description;
		public Type type;

		public DocPage(string path, string title, Type type)
		{
			entries = [];
			this.path = path;
			this.title = title;
			this.type = type;

			var attributes = type.GetCustomAttributes(false);
			foreach (var attribute in attributes)
			{
				if (attribute is DocAttribute doc)
					description = doc.message;
			}
		}
	}
}
