using HarmonyLib;

namespace Moonlet.Patches
{
	public class PropertyTexturesPatch
	{
		[HarmonyPatch(typeof(PropertyTextures), "UpdateDanger")]
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
		}
	}
}
