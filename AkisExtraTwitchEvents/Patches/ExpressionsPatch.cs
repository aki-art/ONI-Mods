using Database;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	internal class ExpressionsPatch
	{

        [HarmonyPatch(typeof(Expressions), MethodType.Constructor, new[] {typeof(ResourceSet) })]
        public class TargetType_Ctor_Patch
        {
            public static void Postfix(Expressions __instance)
            {
				TExpressions.Register(__instance);
			}
        }
	}
}
