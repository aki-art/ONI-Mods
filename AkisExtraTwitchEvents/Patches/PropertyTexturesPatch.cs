using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class PropertyTexturesPatch
	{
		[HarmonyPatch(typeof(PropertyTextures), "UpdateSolidLiquidGasMass", [
			typeof(TextureRegion),
			typeof(int),
			typeof(int),
			typeof(int),
			typeof(int),
			typeof(bool)])]
		public class PropertyTextures_UpdateSolidLiquidGasMass_Patch
		{
			public static bool Prefix(TextureRegion region, int x0, int y0, int x1, int y1)
			{
				if (AkisTwitchEvents.Instance == null || !AkisTwitchEvents.Instance.IsFakeFloodActive)
					return true;

				for (int y = y0; y <= y1; ++y)
				{
					for (int x = x0; x <= x1; ++x)
					{
						int cell = Grid.XYToCell(x, y);
						if (!Grid.IsActiveWorld(cell))
						{
							region.SetBytes(x, y, 0, 0, 0, 0);
						}
						else
						{
							var element = Grid.Element[cell];
							byte solidPresent = 0;
							byte anythingElse = 0;
							var mass = Grid.Mass[cell];
							var proportionalMass = 0f;

							if (element.IsSolid)
							{
								proportionalMass = Mathf.Min(1f, mass / 2000f);
								solidPresent = byte.MaxValue;
							}
							else if (element.IsLiquid || element.IsGas || element.IsVacuum)
							{
								proportionalMass = 1f;
								anythingElse = byte.MaxValue;
							}

							if (mass > 0)
								proportionalMass = Mathf.Max(0.003921569f, proportionalMass);

							region.SetBytes(x, y, solidPresent, anythingElse, 0, (byte)((double)proportionalMass * byte.MaxValue));
						}
					}
				}

				return false;
			}
		}
	}
}
