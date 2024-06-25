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
				if (!__state)
					Mod.templatesLoader.LoadContent();
			}
		}
	}
}
