using HarmonyLib;

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
		public class TemplateCache_GetTemplate_Patch
		{
			// prefix skipping because otherwise the game cries about unfound templates
			public static bool Prefix(string templatePath, ref TemplateContainer __result)
			{
				if (Mod.templatesLoader.TryGet(templatePath, out var template))
				{
					__result = template.GetOrLoad();
					return false;
				}

				return true;
			}
		}
	}
}
