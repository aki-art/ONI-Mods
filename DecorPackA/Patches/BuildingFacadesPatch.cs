using Database;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class BuildingFacadesPatch
	{
		// manually patching, because referencing BuildingFacades class will load strings too early
		public static void Patch(Harmony harmony)
		{
			var targetType = AccessTools.TypeByName("Database.BuildingFacades");
			var target = AccessTools.Constructor(targetType, new[] { typeof(ResourceSet) });
			var postfix = AccessTools.Method(typeof(BuildingFacades_Ctor_Patch), "Postfix");

			harmony.Patch(target, null, new HarmonyMethod(postfix));
		}

		public class BuildingFacades_Ctor_Patch
		{
			public static void Postfix(object __instance)
			{
				DPFacades.Register((ResourceSet<BuildingFacadeResource>)__instance);
			}
		}
	}
}
