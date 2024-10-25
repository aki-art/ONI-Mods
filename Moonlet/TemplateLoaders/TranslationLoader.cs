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
		public Dictionary<string, object> translationKeys = [];
		public string staticID = staticID;

		private static readonly string[] commentSeparator = ["--"];

		public void Add(string key, string value, string link = null)
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
			{
				if(link != null)
					value = FUtility.Utils.FormatAsLink(value, link);

				translationKeys[key] = new TextInfo()
				{
					text = value,
					translatorsNote = null
				};
			}
			else
			{
				if (link != null)
					value = FUtility.Utils.FormatAsLink(parts[0], link);

				translationKeys[key] = new TextInfo()
				{
					text = value,
					translatorsNote = parts[1]
				};
			}
		}

		public void RegisterStrings()
		{
			var mod = MoonletMods.Instance.GetModData(staticID);

			var allStringEntries = new Dictionary<string, object>(translationKeys);

			if (mod.data.StringsOverloadTypes != null)
			{
				foreach (var type in mod.data.StringsOverloadTypes)
					CollectModKeys(allStringEntries, type);
			}

			foreach (var translation in translationKeys)
				Strings.Add(translation.Key, ((TextInfo)translation.Value).text);

			if (mod.data.GenerateTranslationsTemplate && translationKeys.Count > 0)
				CreateTranslationsTemplate(Path.Combine(Manager.GetDirectory(), "strings_templates"), allStringEntries);
		}

		private void CollectModKeys(Dictionary<string, object> dllLoadedStrings, string stringsOverloadType)
		{
			var type = Type.GetType(stringsOverloadType);
			if (type == null)
			{
				Log.Warn($"Issue with loading translations. type {stringsOverloadType} was specified as stringsOverloadType, but type cannot be found.");
				return;
			}

			var keys = Localization.MakeRuntimeLocStringTree(type);
			foreach (var item in keys)
			{
				dllLoadedStrings[$"{type.Name}.{item.Key}"] = item.Value;
			}

			LocString.CreateLocStringKeys(type, null);
		}

		public void CreateTranslationsTemplate(string path, Dictionary<string, object> allStringEntries)
		{
			if (!System.IO.Directory.Exists(path))
				System.IO.Directory.CreateDirectory(path);

			var outputFilename = FileSystem.Normalize(Path.Combine(path, $"{staticID.ToLower()}_template.pot"));

			using (StreamWriter writer = new(outputFilename, false, new UTF8Encoding(false)))
			{
				writer.WriteLine("msgid \"\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("\"Application: Oxygen Not Included\"");
				//writer.WriteLine("\"Generated with FUtility\"");
				writer.WriteLine("\"POT Version: 2.0\"");
				WriteStringsTemplate(staticID, writer, allStringEntries);
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
