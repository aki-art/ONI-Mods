extern alias YamlDotNetButNew;
using Moonlet.TemplateLoaders;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Loaders
{
	public class TemplatesLoader<TemplateLoaderType>(string path) : ContentLoader(path)
		where TemplateLoaderType : TemplateLoaderBase
	{
		protected List<TemplateLoaderType> loaders = [];
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

		public virtual IDeserializer CreateDeserializer() => null;

		public virtual void ResolveConflicts()
		{
			Log.Debug("resolving conflicts!!");

			loaders = [.. loaders.OrderBy(loader => loader.GetPriority(null))];
			var masters = new Dictionary<string, TemplateLoaderType>();

			foreach (var loader in loaders)
			{
				if (masters.TryGetValue(loader.id, out var master))
				{
					loader.isActive = false;
					if (typeof(IMergeable).IsAssignableFrom(loader.GetType()))
					{
						var templateType = loader.GetType().GenericTypeArguments[0];
						(loader as IMergeable).MergeInto(master as IMergeable);
					}
				}
			}
		}

		/*		public virtual void ResolveConflicts(List<string> clusterTags)
				{
					loaders = [.. loaders.OrderBy(loader => loader.GetPriority(clusterTags))];
					var loadedTemplateIds = new HashSet<string>();

					foreach (var loader in loaders)
					{
						if (loadedTemplateIds.Contains(loader.id))
						{
							loader.isActive = false;
						}
					}
				}*/

		protected static T ReadYamlWithPath<T>(string path, IDeserializer deserializer) where T : class
		{
			if (!File.Exists(path))
				return null;

			var entry = FileUtil.ReadYaml<T>(path, deserializer: deserializer);

			if (entry == null)
			{
				Log.Debug($"File {path} was found, but could not be parsed.");
				return null;
			}

			return entry;
		}

		public static List<(string path, T template)> ReadYamlsWithPath<T>(string path, IDeserializer deserializer) where T : class
		{
			var list = new List<(string, T)>();

			if (!Directory.Exists(path))
				return list;

			foreach (var file in Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories))
			{
				var entry = FileUtil.ReadYaml<T>(file, deserializer: deserializer);

				if (entry == null)
				{
					Log.Debug($"File {file} was found, but could not be parsed.");
					continue;
				}

				list.Add((file, entry));
			}

			return list;
		}

		protected virtual void LoadSingleYaml_Internal<TemplateType>(string path, string staticID, bool singleEntry) where TemplateType : class, ITemplate
		{
			Log.Debug($"LoadYaml_Internal {path}", staticID);
			if (!File.Exists(path))
				return;

			if (singleEntry)
			{
				var entry = ReadYamlWithPath<TemplateType>(path, CreateDeserializer());
				if (entry != null)
					CreateTemplate(entry, staticID, path, Path.GetDirectoryName(path));
			}
			else
			{
				var entry = ReadYamlWithPath<TemplateCollection2<TemplateType>>(path, CreateDeserializer());

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
				foreach ((string path, TemplateType template) entry in ReadYamlsWithPath<TemplateType>(path, CreateDeserializer()))
					CreateTemplate(entry.template, staticID, entry.path, path);
			}
			else
			{
				foreach (var entry in ReadYamlsWithPath<TemplateCollection2<TemplateType>>(path, CreateDeserializer()))
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
