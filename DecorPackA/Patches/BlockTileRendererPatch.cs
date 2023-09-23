using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using Rendering;
using System.Reflection;

namespace DecorPackA.Patches
{
	public class BlockTileRendererPatch
	{
		private const string NEUTRONIUM_ALLOY_TILEDEF = "DecorPackA_UnobtaniumAlloyStainedGlassTile";
		private const string CRUDEOIL_TILEDEF = "DecorPackA_CrudeOilStainedGlassTile";

		[HarmonyPatch]
		public class BlockTileRenderer_RenderInfo_Ctor_Patch
		{
			public static MethodBase TargetMethod()
			{
				var type = AccessTools.TypeByName("Rendering.BlockTileRenderer+RenderInfo");
				return AccessTools.Constructor(type, new[]
				{
					typeof(BlockTileRenderer),
					typeof(int),
					typeof(int),
					typeof(BuildingDef),
					typeof(SimHashes)
				});
			}

			public static void Postfix(BlockTileRenderer.RenderInfo __instance, int queryLayer, BuildingDef def, SimHashes element)
			{
				if (queryLayer != (int)def.TileLayer || element == SimHashes.Void)
					return;

				switch (def.PrefabID)
				{
					case NEUTRONIUM_ALLOY_TILEDEF:
						DecorPackAGlassShineColors.neutroniumAlloyMaterial = __instance.material;
						break;
					case CRUDEOIL_TILEDEF:
						DecorPackAGlassShineColors.oilMaterial = __instance.material;
						break;
				}
			}
		}
	}
}
