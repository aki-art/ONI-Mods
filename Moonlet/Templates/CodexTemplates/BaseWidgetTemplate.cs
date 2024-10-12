extern alias YamlDotNetButNew;

using System;

namespace Moonlet.Templates.CodexTemplates
{
	public abstract class BaseWidgetTemplate : BaseTemplate
	{
		public abstract ICodexWidget Convert(Action<string> log = null);
	}
}
