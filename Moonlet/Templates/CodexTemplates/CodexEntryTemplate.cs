extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.CodexTemplates
{
	public class CodexEntryTemplate : ShadowTypeBase<CodexEntry>, ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string[] DlcIds { get; set; }
		public string Icon { get; set; }
		public string LockIcon { get; set; }
		public List<ContainerTemplate> ContentContainers { get; set; }

		[YamlIgnore]
		public string Priority { get; set; }
		[YamlIgnore]
		public ITemplate.MergeBehavior Command { get; set; }
		[YamlIgnore]
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public override CodexEntry Convert(Action<string> log = null)
		{
			var result = new CodexEntry
			{
				id = Id,
				name = Name,
				title = Title,
				iconLockID = LockIcon,
				iconPrefabID = Icon,
				contentContainers = ShadowTypeUtil.CopyList<ContentContainer, ContainerTemplate>(ContentContainers, log)
			};

			if (DlcIds != null)
				result.dlcIds = DlcIds;

			return result;
		}
	}
}
