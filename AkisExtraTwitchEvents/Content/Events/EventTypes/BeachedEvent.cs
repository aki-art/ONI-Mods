/*using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using Twitchery.Content.Defs.Foods;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class BeachedEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Beached";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon - 2;

		private static readonly HashSet<Tag> replacableTiles =
		[
			TileConfig.ID,
			SnowTileConfig.ID
		];

		private static readonly List<Tag> prefabsToSpawn =
		[
			CrabConfig.ID,
			CrabConfig.ID,
			BabyCrabConfig.ID,
			BeachedAstrobarConfig.ID,
			BasicForagePlantPlantedConfig.ID
		];

		public override void Run()
		{
			var pos = PosUtil.ClampedMouseWorldPos();
			var cells = ProcGen.Util.GetFilledCircle(pos, 16);

			foreach (var offset in cells)
			{
				var cell = Grid.PosToCell(offset);

				var element = Grid.Element[cell];

				if (!Grid.IsValidCell(cell) || element.id == SimHashes.Sand)
					continue;

				float tempOverride = GameUtil.GetTemperatureConvertedToKelvin(29, GameUtil.TemperatureUnit.Celsius);

				if (GridUtil.IsCellEmpty(cell))
				{
					if (Grid.IsLiquid(cell) || Random.value < 0.05f)
						AGridUtil.ReplaceElement(cell, element, SimHashes.Water, tempOverride: tempOverride);
					else if (Grid.IsGas(cell))
					{
						AGridUtil.ReplaceElement(cell, element, SimHashes.Oxygen, tempOverride: tempOverride);
					}
				}
				else
				{
					var cellBelow = Grid.CellBelow(cell);
					var isSupportingCell = !Grid.IsValidCell(cellBelow) || GridUtil.IsCellEmpty(cellBelow);
					var sandVariant = isSupportingCell ? SimHashes.SandStone : SimHashes.Sand;

					if (GridUtil.IsCellFoundationEmpty(cell))
					{
						AGridUtil.ReplaceElement(cell, element, sandVariant, tempOverride: tempOverride);
					}

					if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go)
						&& go.TryGetComponent(out Deconstructable deconstructable)
						&& replacableTiles.Contains(go.PrefabID()))
					{
						GameScheduler.Instance.ScheduleNextFrame("spawn sand tile", _ => SimMessages.ReplaceAndDisplaceElement(cell, sandVariant, AGridUtil.cellEvent, 200f, tempOverride));
						deconstructable.ForceDestroyAndGetMaterials();
					}

					Log.Debug("foundation");

					var cellAbove = Grid.CellAbove(cell);
					if (GridUtil.IsCellEmpty(cellAbove))
					{
						Log.Debug("space above");
						if (Random.value < 0.2f)
						{

							Log.Debug("spawning prefab");
							FUtility.Utils.Spawn(prefabsToSpawn.GetRandom(), Grid.CellToPosCCC(cellAbove, Grid.SceneLayer.Creatures));
						}
						else if (Random.value < 0.1f)
						{
							var cellRight = Grid.CellRight(cellAbove);
							if (Grid.IsValidBuildingCell(cellRight) && GridUtil.IsCellEmpty(cellRight))
							{
								if (!Grid.ObjectLayers[(int)ObjectLayer.Building].ContainsKey(cellAbove)
									&& Grid.ObjectLayers[(int)ObjectLayer.Building].ContainsKey(cellRight))
								{
									var def = Assets.GetBuildingDef(BeachChairConfig.ID);
									def.Build(cellAbove, Orientation.Neutral, null, [SimHashes.SandStone.CreateTag()], tempOverride, false);
								}
							}
						}
					}
				}
			}
		}
	}
}
*/