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
		protected List<TemplateLoaderType> loaders = new();
		private bool pathId;

		public List<TemplateLoaderType> GetTemplates() => loaders;

		public bool TryGet(string id, out TemplateLoaderType templateLoader)
		{
			templateLoader = null;

			if (loaders == null)
				return false;

			foreach (var template in loaders)
			{
				if (template.isActive && template.id == id)
				{
					templateLoader = template;
					return true;
				}
			}

			return false;
		}

		public bool IsActive() => loaders.Count > 0;

		public TemplatesLoader<TemplateLoaderType> CachePaths()
		{
			pathId = true;
			return this;
		}

		public virtual void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry) where TemplateType : class, ITemplate
		{
			Log.Debug($"Loading yamls for {mod.staticID}/{this.path}", mod.staticID);

			var path = mod.GetDataPath(this.path);

			if (path.EndsWith(".yaml"))
			{
				if (!File.Exists(path))
					return;

				LoadSingleYaml_Internal<TemplateType>(path, mod.staticID, singleEntry);
				ResolveConflicts();
			}
			else
			{
				if (!Directory.Exists(path))
					return;

				LoadYamls_Internal<TemplateType>(path, mod.staticID, singleEntry);
				ResolveConflicts();
			}

			Log.Info($"Loaded {(loaders == null ? "N/A" : loaders.Count)} {this.path}", mod.staticID);
		}

		public virtual void ResolveConflicts()
		{
			// TODO
			foreach (var template in loaders)
				template.isActive = true;
		}

		public static T ReadYamlWithPath<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			if (!File.Exists(path))
				return null;

			var entry = FileUtil.ReadYaml<T>(path, mappings: mappings);

			if (entry == null)
			{
				Log.Debug($"File {path} was found, but could not be parsed.");
				return null;
			}

			return entry;
		}

		public static List<(string path, T template)> ReadYamlsWithPath<T>(string path, Dictionary<string, Type> mappings = null) where T : class
		{
			var list = new List<(string, T)>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
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

		protected virtual Dictionary<string, Type> GetMappings() => null;

		protected virtual void LoadSingleYaml_Internal<TemplateType>(string path, string staticID, bool singleEntry) where TemplateType : class, ITemplate
		{
			Log.Debug($"LoadYaml_Internal {path}", staticID);
			if (!File.Exists(path))
				return;

			if (singleEntry)
			{
				var entry = ReadYamlWithPath<TemplateType>(path, GetMappings());
				if (entry != null)
					CreateTemplate(entry, staticID, path, Path.GetDirectoryName(path));
			}
			else
			{
				var entry = ReadYamlWithPath<TemplateCollection2<TemplateType>>(path, GetMappings());

				if (entry != null)
				{
					var templates = entry.Templates();
					if (templates != null)

						foreach (var template in templates)
							CreateTemplate(template, staticID, path, Path.GetDirectoryName(path));
				}
			}
		}

		protected virtual void LoadYamls_Internal<TemplateType>(string path, string staticID, bool singleEntry) where TemplateType : class, ITemplate
		{
			Log.Debug($"LoadYamls_Internal {path}", staticID);
			if (!Directory.Exists(path))
				return;

			if (singleEntry)
			{
				foreach ((string path, TemplateType template) entry in ReadYamlsWithPath<TemplateType>(path))
					CreateTemplate(entry.template, staticID, entry.path, path);
			}
			else
			{
				foreach (var entry in ReadYamlsWithPath<TemplateCollection2<TemplateType>>(path))
				{
					var templates = entry.template?.Templates();

					if (templates == null) continue;

					foreach (var template in templates)
						CreateTemplate(template, staticID, entry.path, path);
				}
			}
		}

		private void CreateTemplate<TemplateType>(TemplateType template, string staticID, string templatePath, string path)
		{
			Log.Debug($"creating template: {templatePath}", staticID);

			if (template == null)
			{
				Log.Warn("null template " + path, staticID);
				return;
			}

			var loader = Activator.CreateInstance(typeof(TemplateLoaderType), template, staticID) as TemplateLoaderType;

			loader.path = templatePath;
			loader.relativePath = FileUtil.GetRelativePathKleiWay(templatePath, path);

			loader.Initialize();

			loaders.Add(loader);
		}

		public void ApplyToActiveTemplates(Action<TemplateLoaderType> fn)
		{
			Log.Debug($"Applying {path}");
			foreach (var template in loaders)
				if (template.isActive) fn(template);
			Log.Debug("== DONE ==");
		}
	}
}
