extern alias YamlDotNetButNew;

using System;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.CodexTemplates
{
	public abstract class BaseWidgetTemplate : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public abstract ICodexWidget Convert(Action<string> log = null);

		[YamlIgnore]
		public string Priority { get; set; }
		[YamlIgnore]
		public ITemplate.MergeBehavior Command { get; set; }
		[YamlIgnore]
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }
	}
}
