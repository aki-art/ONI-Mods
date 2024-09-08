using Moonlet.Utils;
using System;

namespace Moonlet.Templates.CodexTemplates
{
	public class TextEntry : BaseWidgetTemplate
	{
		public string Text { get; set; }

		public string MessageID { get; set; }

		public string Style { get; set; }

		public string SringKey { get; set; }

		public override ICodexWidget Convert(Action<string> log = null)
		{
			var style = EnumUtils.ParseOrDefault(Style, CodexTextStyle.Body);
			return new CodexText(Text, style);
		}
	}
}
