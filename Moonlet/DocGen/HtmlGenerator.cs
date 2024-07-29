using System.Text;

namespace Moonlet.DocGen
{
	public class HTMLGenerator(StringBuilder stringBuilder)
	{
		public readonly StringBuilder stringBuilder = stringBuilder;
		private readonly StringBuilder stringBuilderTemp = new();

		public HTMLGenerator TableBegin(params string[] headers)
		{
			stringBuilder
				.AppendLine("<table id=\"data-table\" class=\"table data-table table-sm\">")
				.AppendLine("\t<thead>")
				.AppendLine("\t\t<tr>");

			foreach (string header in headers)
			{
				stringBuilder
					.Append("\t\t\t<th>")
					.Append(header)
					.AppendLine("</th>");
			}

			stringBuilder
				.AppendLine("\t\t</tr>")
				.AppendLine("\t</thead>");

			return this;
		}

		public string MakeList(params string[] items)
		{
			stringBuilderTemp.Clear();

			stringBuilderTemp
				.Append("<ul>");

			foreach (string item in items)
				stringBuilderTemp.Append($"<li>{item}</li>");

			stringBuilderTemp.Append("</ul>");

			return stringBuilderTemp.ToString();
		}

		public HTMLGenerator AddTableRow(params string[] cells)
		{
			stringBuilder.AppendLine("\t\t<tr>");

			foreach (string cell in cells)
			{
				stringBuilder
					.Append("\t\t\t<td>")
					.Append(cell)
					.AppendLine("</td>");
			}

			stringBuilder.AppendLine("\t\t</tr>");

			return this;
		}

		public HTMLGenerator EndTable()
		{
			stringBuilder.AppendLine("</table>");
			return this;
		}
	}
}