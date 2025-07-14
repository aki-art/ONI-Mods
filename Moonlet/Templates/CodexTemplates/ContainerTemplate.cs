extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.Templates.CodexTemplates
{
	public class ContainerTemplate : BaseTemplate, IShadowTypeBase<ContentContainer>
	{
		public List<BaseWidgetTemplate> Content { get; set; }

		public string LockID { get; set; }

		public string ContentLayout { get; set; }

		public bool ShowBeforeGeneratedContent { get; set; }

		public ContentContainer Convert(Action<string> log = null)
		{
			Content ??= [];

			var result = new ContentContainer
			{
				content = Content.Select(c => c.Convert(log)).ToList(),
				showBeforeGeneratedContent = ShowBeforeGeneratedContent,
				lockID = LockID,
				contentLayout = EnumUtils.ParseOrDefault(ContentLayout, ContentContainer.ContentLayout.Vertical)
			};

			return result;
		}
	}
}
