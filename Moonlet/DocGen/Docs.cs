extern alias YamlDotNetButNew;

using HarmonyLib;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
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
		private HTMLGenerator generator;

		private Dictionary<Type, DocPage> pagesLookupByType;

		public void Generate(string outputPath)
		{
			pagesLookupByType = [];

			var assembly = Assembly.GetExecutingAssembly();
			var classes = assembly.GetTypes();

			CollectPageData(outputPath, classes);
			ConnectTypeLinks();
			WriteHTML(outputPath);
		}

		private void WriteHTML(string outputPath)
		{
			stringBuilder = new();
			generator = new(stringBuilder);

			foreach (var page in pagesLookupByType.Values)
			{
				stringBuilder.Clear();

				generator.TableBegin(
					"Type",
					"Property",
					"Description",
					"Default value");

				foreach (var entry in page.entries)
				{
					generator.AddTableRow(
						entry.typeTitle,
						entry.name,
						entry.description,
						"");
				}

				FileUtil.WriteFile(Path.Combine(outputPath, page.path), stringBuilder.ToString());
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
						Log.Debug("generics: " + generics.Join());
						foreach (var generic in generics)
							genericNames.Add(GetTypeTitle(generic));

						entry.typeTitle = $"{GetTypeTitle(entry.type)}<{generics.Join()}>";
						Log.Debug(entry.typeTitle);
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

				entries.Add(new DocEntry(name, description, "", property.PropertyType));
			}
		}
	}
}
