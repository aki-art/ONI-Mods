using Moonlet.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Moonlet.Loaders
{
	public class TemplateCollection<TemplateType> : IYamlConvertible where TemplateType : class, ITemplate
	{
		public List<TemplateType> templates;

		public const string ADD = "Add";

		public readonly HashSet<string> IGNORED_FIELDS = new()
		{
			"Variables",
			"variables",
			"Remove"
		};

		public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			var values = (Dictionary<string, List<TemplateType>>)nestedObjectDeserializer(typeof(Dictionary<string, List<TemplateType>>));

			if (values == null)
				return;

			templates = new();

			foreach (var entry in values)
			{
				if (IGNORED_FIELDS.Contains(entry.Key))
					continue;

				templates.AddRange(entry.Value);
			}
		}

		public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			nestedObjectSerializer(new Dictionary<string, object>()
			{
				{
					ADD,
					templates
				}
			});
		}
	}
}
