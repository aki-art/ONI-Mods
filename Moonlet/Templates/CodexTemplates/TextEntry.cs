using Moonlet.Utils;
using System;

namespace Moonlet.Templates.CodexTemplates
{
	public class TextEntry : BaseWidgetTemplate
	{
		public string Text { get; set; }

		public string MessageID { get; set; }

		public string Style { get; set; }

		public string StringKey { get; set; }

		public override ICodexWidget Convert(Action<string> log = null)
		{
			var style = EnumUtils.ParseOrDefault(Style, CodexTextStyle.Body);

			if (Text.IsNullOrWhiteSpace() && StringKey.IsNullOrWhiteSpace())
				log("Codex Entry Text needs either Text or StringKey defined");

			var text = "MISSING...";

			if (!Text.IsNullOrWhiteSpace())
				text = Text;
			else if (!StringKey.IsNullOrWhiteSpace() && Strings.TryGet(StringKey, out var entry))
				text = entry.String;

			return new CodexText(text, style);
		}
	}
}
