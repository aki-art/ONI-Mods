using HarmonyLib;
using LibNoiseDotNet.Graphics.Tools.Noise;
using Moonlet.LibNoiseExtension;
using ProcGen.Noise;

namespace Moonlet.Patches
{
	public class PrimitivePatch
	{
		[HarmonyPatch(typeof(Primitive), "CreateModule")]
		public class Primitive_CreateModule_Patch
		{
			public static bool Prefix(Primitive __instance, int globalSeed, ref IModule3D __result)
			{
				return !PrimitiveMod.OnCreatePrimitives(__instance, globalSeed, __instance.quality, globalSeed + __instance.seed, ref __result);
			}
		}
	}
}
