using System.Text;

namespace Moonlet.DocGen
{
	public class HTMLGenerator(StringBuilder stringBuilder)
	{
		private readonly StringBuilder stringBuilder = stringBuilder;

		public HTMLGenerator TableBegin(params string[] headers)
		{
			stringBuilder
				.AppendLine("<table>")
				.AppendLine("\t<tr>");

			foreach (string header in headers)
			{
				stringBuilder
					.Append("\t\t<th>")
					.Append(header)
					.AppendLine("</th>");
			}

			stringBuilder
				.AppendLine("\t</tr>");

			return this;
		}

		public HTMLGenerator AddTableRow(params string[] cells)
		{
			stringBuilder.AppendLine("\t<tr>");

			foreach (string cell in cells)
			{
				stringBuilder
					.Append("\t\t<td>")
					.Append(cell)
					.AppendLine("</td>");
			}

			stringBuilder.AppendLine("\t</tr>");

			return this;
		}

		public HTMLGenerator EndTable()
		{
			stringBuilder.AppendLine("</table>");
			return this;
		}
	}
}