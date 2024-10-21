using FUtility;
using HarmonyLib;
using Moonlet.Loaders;

namespace Moonlet.Patches
{
	public class TemplateCachePatch
	{
		[HarmonyPatch(typeof(TemplateCache), nameof(TemplateCache.Init))]
		public class TemplateCache_Init_Patch
		{
			public static void Prefix(ref bool __state)
			{
				__state = TemplateCache.Initted;
			}

			public static void Postfix(ref bool __state)
			{
				//if (!__state)
				//Mod.templatesLoader.CacheLoaders();
			}
		}

		[HarmonyPatch(typeof(TemplateCache), "TemplateExists")]
		public class TemplateCache_TemplateExists_Patch
		{
			public static bool Prefix(string templatePath, ref bool __result)
			{
				if (Mod.templatesLoader.TryGet(templatePath, out _))
				{
					__result = true;
					return false;
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(TemplateCache), "GetTemplate")]
		[HarmonyPriority(FPriority.Early)]
		public class TemplateCache_GetTemplate_Patch
		{
			// prefix skipping because otherwise the game cries about unfound templates
			public static bool Prefix(ref string templatePath, ref TemplateContainer __result)
			{
				if (templatePath == null)
				{
					Log.Warn("Game is trying to load a pathless template");
					return true;
				}

				if (templatePath.StartsWith(MTemplatesLoader.MOONLET_TEMPLATES_PREFIX))
				{
					Log.Debug(templatePath);

					// remove global moonlet prefix
					templatePath = templatePath.Replace(MTemplatesLoader.MOONLET_TEMPLATES_PREFIX, "");
					// remove mod subfolder
					var index = templatePath.IndexOf('/');
					if (index != -1)
						templatePath = templatePath.Substring(index + 1);

					Log.Debug(templatePath);
				}

				if (Mod.templatesLoader == null)
				{
					Log.Warn("TemplatesLoader not initialized yet.");
					return true;
				}

				if (Mod.templatesLoader.TryGet(templatePath, out var template) && template != null)
				{
					__result = template.GetOrLoad();
					return false;
				}

				return true;
			}
		}
	}
}
