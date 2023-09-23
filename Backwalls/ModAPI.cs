using Backwalls.Cmps;
using FUtility;
using UnityEngine;

namespace Backwalls
{
	public class ModAPI
	{
		public static void RegisterMaterialOverride(string patternID, Material material)
		{
			if (material == null)
			{
				Log.Warning("Tried to register a material override, but material is null.");
				return;
			}

			if(BackwallRenderer.shaderOverrides.ContainsKey(patternID))
				Log.Warning($"Multiple mods are registering a material for {patternID}.");

			BackwallRenderer.shaderOverrides[patternID] = material;
		}
	}
}
