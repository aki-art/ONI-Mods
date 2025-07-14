extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.CodexTemplates
{
	public class CodexEntryTemplate : BaseTemplate, IShadowTypeBase<CodexEntry>
	{
		public string Title { get; set; }
		public string[] DlcIds { get; set; }
		public string Icon { get; set; }
		public string LockIcon { get; set; }
		public List<ContainerTemplate> ContentContainers { get; set; }
		public List<CodexEntry_MadeAndUsed> ContentMadeAndUsed { get; set; }

		public CodexEntry Convert(Action<string> log = null)
		{
			var result = new CodexEntry
			{
				id = Id,
				name = Name,
				title = Title,
				iconLockID = LockIcon,
				iconPrefabID = Icon,
				contentContainers = ShadowTypeUtil.CopyList<ContentContainer, ContainerTemplate>(ContentContainers, log),
				contentMadeAndUsed = ContentMadeAndUsed
			};

			if (DlcIds != null)
				result.requiredDlcIds = DlcIds;

			return result;
		}
	}
}
