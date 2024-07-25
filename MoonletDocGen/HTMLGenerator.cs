using System.Text;

namespace MoonletDocGen
{
	public class HTMLGenerator
	{
		public static void TableBegin(StringBuilder stringBuilder, params string[] headers)
		{
			AddRow(stringBuilder, headers);

			for (int i = 0; i < headers.Length; i++)
			{
				stringBuilder.Append(" - ");

				if (i < headers.Length - 1)
					stringBuilder.Append(" | ");
			}

			stringBuilder.AppendLine();
		}

		public static void AddRow(StringBuilder stringBuilder, params string[] cells)
		{
			stringBuilder.Append(" | ");

			for (int i = 0; i < cells.Length; i++)
			{
				string header = cells[i];
				stringBuilder.Append(header);

				if (i < cells.Length - 1)
					stringBuilder.Append(" | ");
			}

			stringBuilder.AppendLine();
		}
	}
}