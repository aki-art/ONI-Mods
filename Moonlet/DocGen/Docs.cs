extern alias YamlDotNetButNew;

using HarmonyLib;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		private StringBuilder navigation;

		private Dictionary<Type, DocPage> pagesLookupByType;
		private HashSet<Type> referencedEnums;

		public void Generate(string outputPath, string templatePath)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			pagesLookupByType = [];
			referencedEnums = [];
			templateBuilder = new StringBuilder(File.ReadAllText(templatePath));

			var assembly = Assembly.GetExecutingAssembly();
			try
			{
				var classes = assembly.GetTypes();


				CollectPageData(outputPath, classes);
				GeneratEnumPages(outputPath);

				navigation = new StringBuilder();
				AddLinksToNav(pagesLookupByType.Keys.Where(key => key.IsEnum), "Enums");
				AddLinksToNav(pagesLookupByType.Keys.Where(key => typeof(BaseTemplate).IsAssignableFrom(key)), "Templates");
				AddLinksToNav(pagesLookupByType.Keys.Where(key => !typeof(BaseTemplate).IsAssignableFrom(key)), "Types");

				templateBuilder = templateBuilder.Replace("{{Navigation}}", navigation.ToString());

				ConnectTypeLinks();
				GenerateContentTable(outputPath);

			}
			catch (Exception e)
			{
				Log.Warn(e.Message);
			}
			stopWatch.Stop();
			Log.Info($"Generated docs files in {stopWatch.ElapsedMilliseconds} ms");
		}

		private void GeneratEnumPages(string outputPath)
		{
			foreach (var type in referencedEnums)
			{
				AddEnumPage(outputPath, type);
			}
		}

		private void AddEnumPage(string outputPath, Type type)
		{
			string relativePath = $"enums/{type.Name}.html";
			var page = new DocPage(relativePath, type.Name, type);
			AddProperties(type, page.entries);

			// todo: extended enums
			foreach (var enumName in Enum.GetValues(type))
			{
				DocEntry item = new(enumName.ToString(), "todo", "", type, null);
				item.numericRepresenation = (int)Convert.ChangeType(enumName, typeof(int));
				page.entries.Add(item);
			}
			pagesLookupByType.Add(type, page);
		}

		private void GenerateContentTable(string outputPath)
		{
			stringBuilder = new();
			generator = new(stringBuilder);

			foreach (var page in pagesLookupByType.Values)
			{
				var finalFile = new StringBuilder(templateBuilder.ToString());

				if (page.type.IsEnum)
					CreateEnumTable(page);
				else
					CreateTable(page);

				finalFile.Replace("{{title}}", page.title);
				finalFile.Replace("{{description}}", page.description);
				finalFile.Replace("{{contents_table}}", stringBuilder.ToString());

				File.WriteAllText(Path.Combine(outputPath, page.path), finalFile.ToString());
			}

			Log.Info("Wrote docs to " + outputPath);
		}

		private void CreateEnumTable(DocPage page)
		{
			stringBuilder.Clear();

			if (page.entries == null || page.entries.Count == 0)
				return;

			generator.TableBegin("Value", "Name", "Description");
			generator.stringBuilder.AppendLine("\t<tbody>");

			foreach (DocEntry entry in page.entries)
				generator.AddTableRow(entry.numericRepresenation.ToString(), entry.name, entry.description);

			generator.stringBuilder.AppendLine("\t</tbody>");

			generator.EndTable();
		}

		private void CreateTable(DocPage page)
		{
			stringBuilder.Clear();

			if (page.entries == null || page.entries.Count == 0)
				return;

			page.entries.OrderBy(e => e.sourceType);

			generator.TableBegin("Type", "Property", "Description", "Default value");
			generator.stringBuilder.AppendLine("\t<tbody>");

			for (int i = 0; i < page.entries.Count; i++)
			{
				DocEntry entry = page.entries[i];

				// This would make categories for parent inherited properties
				/*				if (i < page.entries.Count - 1)
								{
									var nextEntry = page.entries[i + 1];
									var isNewCategory = entry.sourceType != nextEntry.sourceType;

									if (isNewCategory)
									{
										generator.stringBuilder.AppendLine("\t</tbody>");
										generator.EndTable();

										generator.stringBuilder.AppendLine($"<h2>Properties inherited from {nextEntry.sourceType.Name}</h2>");

										generator.TableBegin("Type", "Property", "Description", "Default value");
										generator.stringBuilder.AppendLine("\t<tbody>");
									}
								}*/

				AddTableRow(entry);
			}

			generator.stringBuilder.AppendLine("\t</tbody>");

			generator.EndTable();
		}

		private void AddNavHeader(string label, StringBuilder builder)
		{
			var before = "" +
				"<h6 class=\"sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-body-secondary text-uppercase\">\r\n" +
				"                            <span>{{Title}}</span> </h6>\r\n";

			builder.Append(before);
			builder.Replace("{{Title}}", label);
		}

		private void AddLinksToNav(IEnumerable<Type> types, string label)
		{
			var builder = new StringBuilder();

			AddNavHeader(label, builder);
			AddLinksToNav(types, builder);

			navigation.Append(builder.ToString());
		}

		private void AddLinksToNav(IEnumerable<Type> types, StringBuilder builder)
		{
			builder.Clear();
			builder.Append("<ul class=\"nav flex-column mb-auto\">");

			navigation.Append(builder.ToString());

			var str0 = "" +
				"                            <li class=\"nav-item\">\r\n" +
				"                                <a class=\"nav-link d-flex align-items-center gap-2\" href=\"";
			var str1 = "\">\r" +
				"\n                                    <svg class=\"bi\"><use\r" +
				"\n                                                xlink:href=\"\" /></svg> ";

			var str2 = "\r" +
				"\n                                </a>\r" +
				"\n                            </li>";

			foreach (var page in types)
			{
				var displayName = page.Name;
				if (page.Name.EndsWith("Template"))
					displayName = displayName.Substring(0, displayName.Length - 8);

				builder.Append(str0);
				builder.Append($"{page.Name}.html");
				builder.Append(str1);
				builder.Append($"{displayName}");
				builder.Append(str2);
				builder.AppendLine();
			}

			builder.Append("</ul>");
		}

		private void AddTableRow(DocEntry entry)
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
			string relativePath = type.Name + ".html";
			var page = new DocPage(relativePath, type.Name, type);

			if (type is IDocumentation)
			{
				var instance = Activator.CreateInstance(type) as IDocumentation;
				instance.ModifyDocs(page);
			}

			AddProperties(type, page.entries);

			pagesLookupByType.Add(type, page);
		}

		private static bool IsDocumentedClass(Type type)
		{
			// don't generate for the basic ITemplate
			if (type == typeof(BaseTemplate) || type == typeof(IDocumentation))
				return false;

			return typeof(BaseTemplate).IsAssignableFrom(type)
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
					referencedEnums.Add(property.PropertyType);
				}
				else
					acceptedValues = null;

				DocEntry item = new(name, description, "", property.PropertyType, acceptedValues);

				if (property.DeclaringType != type)
					item.sourceType = property.DeclaringType;

				entries.Add(item);
			}
		}
	}
}
