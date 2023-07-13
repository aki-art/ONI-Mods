using FUtility;
using HarmonyLib;
using Moonlet.ZoneTypes;
using System;
using System.Linq;
using static GroundMasks;

namespace Moonlet.Patches
{
	internal class GroundRendererPatch
	{
		// sets the mask data. this is also responsible for selecting which border type will be rendered on ground tiles
		[HarmonyPatch(typeof(GroundRenderer), "OnPrefabInit")]
		public static class GroundRenderer_OnPrefabInit_Patch
		{
			public static void Postfix(ref GroundMasks.BiomeMaskData[] ___biomeMasks)
			{
				if (ZoneTypeUtil.zones == null || ___biomeMasks == null)
					return;

				Array.Resize(ref ___biomeMasks, ___biomeMasks.Length + ZoneTypeUtil.GetCount());

				foreach (var zone in ZoneTypeUtil.zones)
				{
					var index = (int)zone.type;

					BiomeMaskData reference = null;
					var id = zone.borderType.ToString().ToLowerInvariant();

					foreach (var data in ___biomeMasks)
						Log.Debuglog(data?.name);

					foreach (var data in ___biomeMasks)
					{
						if (data != null && data.name == id)
							reference = data;
					}

					if (reference == null)
					{
						Log.Warning("There is no border type with id " + id);
						return;
					}

					var newEntry = new BiomeMaskData(zone.id)
					{
						tiles = reference.tiles
					};

					___biomeMasks[index] = newEntry;
					___biomeMasks[index].GenerateRotations();

					// only runs once so whatever traverse go
					Traverse.Create(___biomeMasks[index]).Method("Regenerate").GetValue();
				}
			}
		}
	}
}
