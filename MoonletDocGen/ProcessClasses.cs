using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MoonletDocGen
{
	internal partial class ProcessClasses
	{
		private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
		{
			string text = CasingTransformRegex()
				.Replace(str, (Match match) => match.Groups["char"].Value.ToUpperInvariant());

			return firstLetterTransform(text[0]) + text[1..];
		}

		public static void Process()
		{
			Assembly.Load("Moonlet");
			var assembly = AppDomain.CurrentDomain.GetAssemblies().
				SingleOrDefault(assembly => assembly.GetName().Name == "Moonlet");

			if (assembly == null)
			{
				Console.WriteLine("ASSEMBLY NOT FOUND");
				foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
					Console.WriteLine(ass.GetName().Name);

				return;
			}
			var classes = assembly.GetTypes();
			var stringBuilder = new StringBuilder();

			foreach (var type in classes)
			{
				//if (type.IsAssignableFrom(typeof(ITemplate)))
				//{
				stringBuilder.Clear();
				HTMLGenerator.TableBegin(stringBuilder, "type", "property", "default value", "description", "required");

				var properties = type.GetProperties();
				foreach (var property in properties)
				{
					foreach (Attribute attr in Attribute.GetCustomAttributes(property))
					{
						var attributeType = attr.GetType();
					}


					HTMLGenerator.AddRow(
						stringBuilder,
						property.GetType().ToString(),
						ToCamelOrPascalCase(property.Name, char.ToLowerInvariant),
						"",
						"",
						false.ToString()
						);
				}

				Console.WriteLine(type.Name);
				//File.WriteAllText($"C:/Users/Aki/Desktop/temp/New folder (15)/{type.Name}.md", stringBuilder.ToString());
				//}
			}
		}

		[GeneratedRegex("([_\\-])(?<char>[a-z])", RegexOptions.IgnoreCase, "en-US")]
		private static partial Regex CasingTransformRegex();
	}
}
