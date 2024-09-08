extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.CodexTemplates
{
	public class ContainerTemplate : ShadowTypeBase<ContentContainer>, ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public List<BaseWidgetTemplate> Content { get; set; }

		public string LockID { get; set; }

		public string ContentLayout { get; set; }

		public bool ShowBeforeGeneratedContent { get; set; }

		[YamlIgnore]
		public string Priority { get; set; }

		[YamlIgnore]
		public ITemplate.MergeBehavior Command { get; set; }

		[YamlIgnore]
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public override ContentContainer Convert(Action<string> log = null)
		{
			Content ??= [];

			var result = new ContentContainer
			{
				content = Content.Select(c => c.Convert()).ToList(),
				showBeforeGeneratedContent = ShowBeforeGeneratedContent,
				lockID = LockID,
				contentLayout = EnumUtils.ParseOrDefault(ContentLayout, ContentContainer.ContentLayout.Vertical)
			};

			return result;
		}
	}
}
