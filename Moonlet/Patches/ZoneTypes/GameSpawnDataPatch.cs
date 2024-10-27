using HarmonyLib;
using Moonlet.Scripts;
using ProcGenGame;
using System.Collections.Generic;

namespace Moonlet.Patches.ZoneTypes
{
	public class GameSpawnDataPatch
	{
		[HarmonyPatch(typeof(GameSpawnData), nameof(GameSpawnData.AddTemplate))]
		public class GameSpawnData_AddTemplate_Patch
		{
			public static void Prefix(TemplateContainer template, Vector2I position, ref Dictionary<int, int> claimedCells)
			{
				//Moonlet_Mod.Instance.ApplyZoneTypeOverrides(template, position);
				//Moonlet_ZoneTypeTracker.worldgenZoneTypeOverrides ??= [];
				//Moonlet_ZoneTypeTracker.worldgenZoneTypeOverrides[position] = template;
				Moonlet_Mod.worldgenZoneTypeOverrides ??= [];
				Moonlet_Mod.worldgenZoneTypeOverrides[position] = template;
			}
		}
	}
}
