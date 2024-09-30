using System;

namespace Moonlet.Templates.CodexTemplates
{
	public class DividerLineEntry : BaseWidgetTemplate
	{
		public int PreferredWidth { get; set; }

		public int PreferredHeight { get; set; }

		public DividerLineEntry()
		{
			PreferredHeight = -1;
			PreferredWidth = -1;
		}

		public override ICodexWidget Convert(Action<string> log = null)
		{
			return new CodexDividerLine()
			{
				preferredHeight = PreferredHeight,
				preferredWidth = PreferredHeight,
			};
		}
	}
}
