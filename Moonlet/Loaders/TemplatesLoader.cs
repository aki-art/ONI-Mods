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

		protected virtual void LoadYamls_Internal<TemplateType>(string path, string staticID, bool singleEntry) where TemplateType : class, ITemplate
		{
			if (!Directory.Exists(path))
				return;

			if(singleEntry)
			{
				foreach (var entry in FileUtil.ReadYamls<TemplateType>(path))
				{
					if (entry == null)
					{
						Log.Warn("null template " + path);
						continue;
					}

					var loader = Activator.CreateInstance(typeof(TemplateLoaderType), entry) as TemplateLoaderType;
					loader.sourceMod = staticID;

					templates.Add(loader);
				}
			}
			else
				foreach (var entries in FileUtil.ReadYamls<TemplateCollection<TemplateType>>(path))
				{
					if (entries == null) continue;

					foreach (var entry in entries.templates)
					{
						if (entry == null)
						{
							Log.Warn("null template " + path);
							continue;
						}

						var loader = Activator.CreateInstance(typeof(TemplateLoaderType), entry) as TemplateLoaderType;
						loader.sourceMod = staticID;

						templates.Add(loader);
					}
				}
		}

		public void ApplyToActiveTemplates(Action<TemplateLoaderType> fn)
		{
			foreach (var template in templates)
				if (template.isActive) fn(template);
		}
	}
}
