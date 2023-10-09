using Moonlet.TemplateLoaders;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Moonlet.Loaders
{
	public class TemplatesLoader<TemplateLoaderType>(string path) : ContentLoader(path)
		where TemplateLoaderType : TemplateLoaderBase
	{
		protected List<TemplateLoaderType> templates = new();
		private bool pathId;

		public bool IsActive() => templates.Count > 0;

		public TemplatesLoader<TemplateLoaderType> CachePaths()
		{
			pathId = true;
			return this;
		}

		public virtual void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry) where TemplateType : class, ITemplate
		{
			Log.Debug($"Loading yamls for {mod.staticID}/{this.path}", mod.staticID);

			var path = mod.GetDataPath(this.path);

			if (!Directory.Exists(path))
				return;

			LoadYamls_Internal<TemplateType>(path, mod.staticID, singleEntry);
			ResolveConflicts();

			Log.Info($"Loaded {(templates == null ? 0 : templates.Count)} {this.path}", mod.staticID);
		}

		public virtual void ResolveConflicts()
		{
			// TODO
			foreach (var template in templates)
				template.isActive = true;
		}

		public static List<(string path, T template)> ReadYamlsWithPath<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			Log.Debug("GetFiles " + path);

			var list = new List<(string, T)>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				Log.Debug("\t" + file);
				var entry = FileUtil.ReadYaml<T>(file, mappings: mappings);

				if (entry == null)
				{
					Log.Debug($"File {file} was found, but could not be parsed.");
					continue;
				}

				list.Add((file, entry));
			}

			return list;
		}

		protected virtual void LoadYamls_Internal<TemplateType>(string path, string staticID, bool singleEntry) where TemplateType : class, ITemplate
		{
			if (!Directory.Exists(path))
				return;

			if (singleEntry)
			{
				foreach ((string path, TemplateType template) entry in ReadYamlsWithPath<TemplateType>(path))
				{
					CreateTemplate(entry.template, staticID, entry.path, path);
				}
			}
			else
				foreach (var entry in ReadYamlsWithPath<TemplateCollection<TemplateType>>(path))
				{
					if (entry.template == null) continue;

					foreach (var template in entry.template.templates)
					{
						CreateTemplate(template, staticID, entry.path, path);
					}
				}
		}

		private void CreateTemplate<TemplateType>(TemplateType template, string staticID, string templatePath, string path)
		{
			Log.Debug($"creatig template: {staticID}/{templatePath}");

			if (template == null)
			{
				Log.Warn("null template " + path);
				return;
			}

			var loader = Activator.CreateInstance(typeof(TemplateLoaderType), template, staticID) as TemplateLoaderType;

			loader.path = templatePath;
			loader.relativePath = FileUtil.GetRelativePathKleiWay(templatePath, path);

			loader.Initialize();

			templates.Add(loader);
		}

		public void ApplyToActiveTemplates(Action<TemplateLoaderType> fn)
		{
			foreach (var template in templates)
				if (template.isActive) fn(template);
		}
	}
}
