﻿using Moonlet.Loaders;
using System.Collections.Generic;

namespace Moonlet
{
	public class ModAPI
	{
		// returns the dictionary of unique template loaders actually active
		public static Dictionary<string, Dictionary<string, object>> GetTemplates(string contentType)
		{
			if(Mod.allLoaders == null)
			{
				Log.Warn("MoonletAPI: Moonlet is not initialized yet. Try afer OnAllModsLoaded.");
				return null;
			}

			// outer dictionary: all templates by that loader
			// inner dictionary: template data. has keys: sourceMod, path, id, template, loader, isActive, isValid

			// template holds the data loaded by a user, loader is the functional loading class that puts it into the game. you are probably looking for the template.

			// isActive is a bool, a template is inactive when stomped by another template with a higher priority, or has incompatible DLC-s or mods. there are plans in the future to reload all templates on cluster change, and this will update.

			foreach (var loader in Mod.allLoaders)
			{
				if(loader is TemplatesLoader templatesLoader && templatesLoader.path == contentType)
					return templatesLoader.GetTemplatesSerialized();
			}

			// example: ModAPI.GetTemplates("worldgen/clusters")["clusters/AstropelagosMoonlets"]["sourceMod"] should output "Beached"
			return null;
		}

		// return all loaders, even is stomped by another mod
		public static List<Dictionary<string, object>> GetAllTemplates(string contentType)
		{
			if (Mod.allLoaders == null)
			{
				Log.Warn("MoonletAPI: Moonlet is not initialized yet. Try afer OnAllModsLoaded.");
				return null;
			}

			foreach (var loader in Mod.allLoaders)
			{
				if (loader is TemplatesLoader templatesLoader && templatesLoader.path == contentType)
					return templatesLoader.GetAllTemplatesSerialized();
			}

			return null;
		}
	}
}
