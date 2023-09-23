using FUtility;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FUtility.FLocalization
{
	public class TemplateGenerator
	{
		/// <summary>
		/// Generates a .pot file
		/// Very similar to the game's already provided method, but this one supports translation notes, 
		/// provided to LocStrings with the <see cref="NoteAttribute"/>
		/// </summary>
		public static void GenerateStringsTemplate(Type locStringTreeRoot, string outputFolder)
		{
			outputFolder = FileSystem.Normalize(outputFolder);

			if (!FileUtil.CreateDirectory(outputFolder, 5))
				return;

			GenerateStringsTemplate(
				locStringTreeRoot.Namespace,
				Assembly.GetAssembly(locStringTreeRoot),
				FileSystem.Normalize(Path.Combine(outputFolder, $"{locStringTreeRoot.Namespace.ToLower()}_template.pot")),
				null);
		}

		private static void GenerateStringsTemplate(string locStringsNamespace, Assembly assembly, string outputFilename, Dictionary<string, object> runtimeForest)
		{
			var dictionary1 = new Dictionary<string, object>();
			var m_CollectLocStringTreeRoots = typeof(Localization).GetMethod("CollectLocStringTreeRoots", BindingFlags.Static | BindingFlags.NonPublic);

			if (m_CollectLocStringTreeRoots == null)
			{
				Log.Warning("Could not find method Localization.CollectLocStringTreeRoots.");
				return;
			}

			var treeRoots = m_CollectLocStringTreeRoots.Invoke(null, new object[] { locStringsNamespace, assembly });

			foreach (var locStringTreeRoot in treeRoots as IEnumerable<Type>)
			{
				var dictionary2 = MakeRuntimeLocStringTree(locStringTreeRoot);
				if (dictionary2.Count > 0)
				{
					dictionary1[locStringTreeRoot.Name] = dictionary2;
				}
			}

			if (runtimeForest != null)
			{
				dictionary1.Concat(runtimeForest);
			}

			using (StreamWriter writer = new StreamWriter(outputFilename, false, new UTF8Encoding(false)))
			{
				writer.WriteLine("msgid \"\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("\"Application: Oxygen Not Included\"");
				writer.WriteLine("\"Generated with FUtility\"");
				writer.WriteLine("\"POT Version: 2.0\"");
				WriteStringsTemplate(locStringsNamespace, writer, dictionary1);
			}

			Log.Info("Generated " + outputFilename);
		}

		private class TextInfo
		{
			public string text;
			public string translatorsNote;
		}

		private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locStringTreeRoot)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			var fields = locStringTreeRoot.GetFields();

			foreach (var fieldInfo in fields)
			{
				if (fieldInfo.FieldType != typeof(LocString))
					continue;

				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
					continue;
				}

				var locString = (LocString)fieldInfo.GetValue(null);
				if (locString == null)
				{
					Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
				}
				else
				{
					dictionary[fieldInfo.Name] = new TextInfo()
					{
						text = locString.text,
						translatorsNote = GetNote(fieldInfo)
					};
				}
			}

			var nestedTypes = locStringTreeRoot.GetNestedTypes();

			foreach (Type type in nestedTypes)
			{
				var dictionary2 = MakeRuntimeLocStringTree(type);
				if (dictionary2.Count > 0)
				{
					dictionary[type.Name] = dictionary2;
				}
			}

			return dictionary;
		}

		private static string GetNote(FieldInfo fieldInfo)
		{
			return fieldInfo.GetCustomAttribute<NoteAttribute>()?.message;
		}

		private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtimeTree)
		{
			if (writer == null) Log.Warning("writer is null");
			if (runtimeTree == null) Log.Warning("runtimeTree is null");
			var stringList = new List<string>(runtimeTree.Keys);
			stringList.Sort();

			foreach (string key in stringList)
			{
				var path1 = path + "." + key;
				var tree = runtimeTree[key];

				if (tree == null) Log.Warning("tree is null");
				var type = tree.GetType();

				if (type != typeof(string) && type != typeof(TextInfo))
				{
					WriteStringsTemplate(path1, writer, tree as Dictionary<string, object>);
				}
				else
				{
					var info = tree as TextInfo;
					if (info == null) Log.Warning("info is null");
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
	}
}
