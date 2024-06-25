using HarmonyLib;
using Moonlet.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using static Klei.WorldDetailSave;

namespace Moonlet.Patches.ZoneTypes
{
	public class SubworldZoneRenderDataPatches
	{
		[HarmonyPatch(typeof(SubworldZoneRenderData), nameof(SubworldZoneRenderData.GenerateTexture))]
		public static class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Prefix(SubworldZoneRenderData __instance)
			{
				var textureIndex = ZoneTypesLoader.LAST_INDEX;

				var count = Mod.zoneTypesLoader.GetCount();
				Array.Resize(ref __instance.zoneColours, __instance.zoneColours.Length + count);
				Array.Resize(ref __instance.zoneTextureArrayIndices, __instance.zoneTextureArrayIndices.Length + count);

				foreach (var zone in Mod.zoneTypesLoader.GetTemplates())
				{
					__instance.zoneColours[(int)zone.type] = zone.color32;
					__instance.zoneTextureArrayIndices[(int)zone.type] = textureIndex++;
				}
			}
		}

		[HarmonyPatch(typeof(SubworldZoneRenderData), nameof(SubworldZoneRenderData.OnActiveWorldChanged))]
		public class SubworldZoneRenderData_OnActiveWorldChanged_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var m_Contains = AccessTools.Method(typeof(Delaunay.Geo.Polygon), "Contains", new[] { typeof(Vector2) });

				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.Calls(m_Contains));

				if (index == -1)
				{
					Log.Warn("Zonetypes: could not patch SubworldZoneRenderData.OnActiveWorldChanged");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(SubworldZoneRenderData_OnActiveWorldChanged_Patch), nameof(InjectedMethod));

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
				// bool is on stack
				new CodeInstruction(OpCodes.Ldloc_3), // Vector2 zero
				new CodeInstruction(OpCodes.Ldloc_0), // byte[] rawTextureData1
				new CodeInstruction(OpCodes.Ldloc_1), // byte[] rawTextureData2
				new CodeInstruction(OpCodes.Ldloc_S, 5), // WorldDetailSave.OverworldCell overworldCell
				new CodeInstruction(OpCodes.Ldarg_0), // this
				new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static bool InjectedMethod(bool originalValue, Vector2 pos, byte[] rawTextureData1, byte[] rawTextureData2, OverworldCell overworldCell, SubworldZoneRenderData instance)
			{
				/*				var cell = Grid.PosToCell(pos);
								if (Moonlet_Mod.Instance.zoneTypeOverrides.TryGetValue(cell, out var zoneType))
								{
									if (Grid.IsValidCell(cell))
									{
										if (Grid.IsActiveWorld(cell))
										{
											rawTextureData2[cell] = zoneType == SubWorld.ZoneType.Space ? byte.MaxValue : (byte)instance.zoneTextureArrayIndices[(int)zoneType];
											var zoneColour = instance.zoneColours[(int)zoneType];
											rawTextureData1[cell * 3] = zoneColour.r;
											rawTextureData1[cell * 3 + 1] = zoneColour.g;
											rawTextureData1[cell * 3 + 2] = zoneColour.b;
										}
										else
										{
											rawTextureData2[cell] = byte.MaxValue;
											var zoneColour = instance.zoneColours[(int)SubWorld.ZoneType.Space];
											rawTextureData1[cell * 3] = zoneColour.r;
											rawTextureData1[cell * 3 + 1] = zoneColour.g;
											rawTextureData1[cell * 3 + 2] = zoneColour.b;
										}
									}

									return false;
								}
				*/
				return originalValue;
			}
		}
	}
}
