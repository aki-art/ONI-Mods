using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Moonlet.Patches
{
	public class PropertyTexturesPatch
	{

		// Makes the Salty Oxygen texture the lighter texture Oxygen uses
		[HarmonyPatch(typeof(PropertyTextures), nameof(PropertyTextures.UpdateDanger))]
		public static class PropertyTextures_UpdateDanger_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.LoadsConstant((int)SimHashes.Oxygen)); /// 34 ldc.i4 SimHashes.Oxygen

				if (index == -1)
					return codes;

				var m_GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), nameof(GetDangerForElement), [typeof(int), typeof(int)]);

				codes.InsertRange(index + 3,
				[
					// byte.maxValue is loaded to the stack
					new CodeInstruction(OpCodes.Ldloc_2), // load num to the stack
					new CodeInstruction(OpCodes.Call, m_GetDangerForElement)
				]);

				return codes;
			}

			private static byte GetDangerForElement(int existingValue, int cell)
			{
				return (Grid.Element[cell].HasTag(GameTags.Breathable))
					? (byte)0
					: (byte)existingValue;
			}
		}

		/*		[HarmonyPatch(typeof(PropertyTextures), "UpdateDanger")]
				public class PropertyTextures_UpdateDanger_Patch
				{
					public static bool Prefix(TextureRegion region, int x0, int y0, int x1, int y1)
					{
						for (int y2 = y0; y2 <= y1; y2++)
						{
							for (int x2 = x0; x2 <= x1; x2++)
							{
								var cell = Grid.XYToCell(x2, y2);
								var isActiveCell = !Grid.IsActiveWorld(cell);

								if (isActiveCell)
								{
									region.SetBytes(x2, y2, 0);
								}
								else
								{
									var element = Grid.Element[cell];
									region.SetBytes(x2, y2, element.HasTag(GameTags.Breathable) ? 0 : byte.MaxValue);
								}
							}
						}

						return false;
					}
				}*/
	}
}
