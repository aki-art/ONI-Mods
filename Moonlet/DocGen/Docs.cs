extern alias YamlDotNetButNew;

using HarmonyLib;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization.Utilities;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.DocGen
{
	public class Docs
	{
		private StringBuilder stringBuilder;
		private StringBuilder templateBuilder;
		private HTMLGenerator generator;

		private Dictionary<Type, DocPage> pagesLookupByType;

		public void Generate(string outputPath, string templatePath)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			pagesLookupByType = [];
			templateBuilder = new StringBuilder(File.ReadAllText(templatePath));

			var assembly = Assembly.GetExecutingAssembly();
			var classes = assembly.GetTypes();

			CollectPageData(outputPath, classes);
			ConnectTypeLinks();
			GenerateContentTable(outputPath);

			stopWatch.Stop();
			Log.Info($"Generated docs files in {stopWatch.ElapsedMilliseconds} ms");
		}

		private void GenerateContentTable(string outputPath)
		{
			stringBuilder = new();
			generator = new(stringBuilder);

			foreach (var page in pagesLookupByType.Values)
			{
				var finalFile = new StringBuilder(templateBuilder.ToString());

				stringBuilder.Clear();

				generator.TableBegin(
					"Type",
					"Property",
					"Description",
					"Default value");

				generator.stringBuilder.AppendLine("\t<tbody>");

				foreach (var entry in page.entries)
				{
					if (entry.acceptedValues != null)
					{
						entry.description += "</br><b>Accepted Values</b>";
						entry.description += generator.MakeList([.. entry.acceptedValues]);
					}

					generator.AddTableRow(
						entry.typeTitle,
						entry.name,
						entry.description,
						"");
				}

				generator.stringBuilder.AppendLine("\t</tbody>");

				generator.EndTable();

				finalFile.Replace("{{contents_table}}", stringBuilder.ToString());

				File.WriteAllText(Path.Combine(outputPath, page.path), finalFile.ToString());
			}

			Log.Info("Wrote docs to " + outputPath);
		}

		private string GetTypeTitle(Type type)
		{
			return pagesLookupByType.TryGetValue(type, out var referencedPage)
				? $"<a href={referencedPage.path}>{type.Name}</a>"
				: type.Name;
		}

		private void ConnectTypeLinks()
		{
			foreach (var page in pagesLookupByType.Values)
			{
				foreach (var entry in page.entries)
				{
					var generics = entry.type.GetGenericArguments();
					if (generics != null && generics.Length > 0)
					{
						var genericNames = new List<string>();
						foreach (var generic in generics)
							genericNames.Add(GetTypeTitle(generic));

						var entryType = GetTypeTitle(entry.type);
						int index = entryType.IndexOf("`");
						if (index >= 0)
							entryType = entryType.Substring(0, index);

						entry.typeTitle = $"{entryType}&lt;{genericNames.Join()}&gt;";
					}
					else
					{
						entry.typeTitle = GetTypeTitle(entry.type);
					}
				}
			}
		}

		private void CollectPageData(string outputPath, Type[] classes)
		{
			foreach (var type in classes)
			{
				if (IsDocumentedClass(type))
					AddPage(outputPath, type);
			}
		}

		private void AddPage(string outputPath, Type type)
		{
			string relativePath = type.Name.ToCamelCase() + ".html";
			var page = new DocPage(relativePath, type.Name, "", type);

			AddProperties(type, page.entries);
			pagesLookupByType.Add(type, page);
		}

		private static bool IsDocumentedClass(Type type)
		{
			// don't generate for the basic ITemplate
			if (type == typeof(ITemplate) || type == typeof(IDocumentation))
				return false;

			return typeof(ITemplate).IsAssignableFrom(type)
				|| typeof(IDocumentation).IsAssignableFrom(type);
		}

		private void AddProperties(Type type, List<DocEntry> entries)
		{
			var properties = type.GetProperties();
			List<string> acceptedValues = null;

			foreach (var property in properties)
			{
				var name = property.Name.ToCamelCase();
				var description = string.Empty;

				foreach (Attribute attr in Attribute.GetCustomAttributes(property))
				{
					var attributeType = attr.GetType();

					if (attr is YamlMemberAttribute yamlMember)
					{
						if (!yamlMember.Alias.IsNullOrWhiteSpace())
							name = yamlMember.Alias;
					}
					else if (attr is DocAttribute doc)
					{
						description = doc.message;
					}
				}

				if (property.PropertyType.IsEnum)
				{
					acceptedValues = new(Enum.GetNames(property.PropertyType)); // TODO: custom overrides
				}
				else
					acceptedValues = null;

				entries.Add(new DocEntry(name, description, "", property.PropertyType, acceptedValues));
			}
		}
	}
}
