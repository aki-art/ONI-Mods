using Klei;
using KMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moonlet.TemplateLoaders
{
	public class TranslationLoader(string staticID)
	{
		public Dictionary<string, object> translationKeys = new();
		public string staticID = staticID;

		private static readonly string[] commentSeparator = new string[] { "--" };

		public void Add(string key, string value)
		{
			if (key == null)
			{
				Log.Warn("Cannot register null translation key.", staticID);
				return;
			}

			if (value == null)
			{
				translationKeys[key] = new TextInfo()
				{
					text = "",
					translatorsNote = null
				};

				return;
			}

			var parts = value.IsNullOrWhiteSpace()
				? null
				: value.Split(commentSeparator, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts == null || parts.Length == 1)
				translationKeys[key] = new TextInfo()
				{
					text = value,
					translatorsNote = null
				};
			else
			{
				translationKeys[key] = new TextInfo()
				{
					text = parts[0],
					translatorsNote = parts[1]
				};
			}
		}

		public void RegisterStrings()
		{
			Log.Debug($"Loading translations for {staticID} ({translationKeys.Count} entries).");

			foreach (var translation in translationKeys)
				Strings.Add(translation.Key, ((TextInfo)translation.Value).text);

			if (translationKeys.Count > 0)
				CreateTranslationsTemplate(Path.Combine(Manager.GetDirectory(), "strings_templates"));
		}

		public void CreateTranslationsTemplate(string path)
		{
			Log.Debug($"Loading template file for {staticID}");
			var outputFilename = FileSystem.Normalize(Path.Combine(path, $"{staticID.ToLower()}_template.pot"));

			using (StreamWriter writer = new(outputFilename, false, new UTF8Encoding(false)))
			{
				writer.WriteLine("msgid \"\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("\"Application: Oxygen Not Included\"");
				writer.WriteLine("\"Generated with FUtility\"");
				writer.WriteLine("\"POT Version: 2.0\"");
				WriteStringsTemplate(staticID, writer, translationKeys);
			}

			Log.Info($"Generated translations template for {staticID} at {outputFilename}");
		}

		private void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtimeTree)
		{
			if (writer == null || runtimeTree == null)
				return;

			var stringList = new List<string>(runtimeTree.Keys);
			stringList.Sort();

			foreach (string key in stringList)
			{
				var path1 = path + "." + key;
				var tree = runtimeTree[key];

				if (tree == null) Log.Warn("tree is null");
				var type = tree.GetType();

				if (type != typeof(string) && type != typeof(TextInfo))
				{
					WriteStringsTemplate(path1, writer, tree as Dictionary<string, object>);
				}
				else
				{
					var info = tree as TextInfo;
					if (info == null) Log.Warn("info is null", staticID);
					var str = info == null ? tree as string : info.text;

					str = str
						.Replace("\\", "\\\\")
						.Replace("\"", "\\\"")
						.Replace("\n", "\\n")
						.Replace("’", "'")
						.Replace("“", "\\\"")
						.Replace("”", "\\\"")
						.Replace("…", "...");

					var desc = info == null ? "#. " + path1 : $"# {info.translatorsNote}";

					writer.WriteLine(desc);
					writer.WriteLine("msgctxt \"{0}\"", path1);
					writer.WriteLine("msgid \"" + str + "\"");
					writer.WriteLine("msgstr \"\"");
					writer.WriteLine("");
				}
			}
		}

		public void OverloadTemplate(Type root)
		{

		}
		private class TextInfo
		{
			public string text;
			public string translatorsNote;
		}

	}
}
