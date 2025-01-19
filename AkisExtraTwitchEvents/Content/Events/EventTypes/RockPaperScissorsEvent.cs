using ONITwitchLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class RockPaperScissorsEvent : TwitchEventBase
	{
		public const string ID = "RockPaperScissors";

		public static CellElementEvent spawnEvent = new("AETE_RockPaperScissorsEvent", "RockPaperScissors event spawn", false);

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		private List<Hand> rolledHand = [];

		public RockPaperScissorsEvent() : base(ID)
		{
			rolledHand = [
				new Hand(STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.ROCK, Rock),
				new Hand(STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.PAPER, Paper),
				new Hand(STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.SCISSORS, Scissor)
				];
		}

		private bool Rock()
		{
			var pos = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			var center = Grid.PosToCell(pos);
			var worldIdx = Grid.WorldIdx[center];

			var positions = ProcGen.Util.GetBlob(pos, 2, new KRandom());

			foreach (var position in positions)
			{
				var cell = Grid.PosToCell(position);

				if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != worldIdx)
					continue;

				SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.IgneousRock, spawnEvent, 800);
			}

			var fx = FXHelpers.CreateEffect("aete_puff_kanim", Grid.CellToPosCCC(center, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
			fx.TintColour = new Color(0.6f, 0.6f, 0.6f, 1f);
			fx.animScale *= 1.5f;
			fx.Play("idle");
			fx.destroyOnAnimComplete = true;

			AudioUtil.PlaySound(ModAssets.Sounds.ROCK, ModAssets.GetSFXVolume() * 0.35f);

			return true;
		}

		private bool Paper()
		{
			return false; // can't think of an idea yet
		}

		private static readonly HashSet<ObjectLayer> layers =
			[
				ObjectLayer.Wire,
				ObjectLayer.LiquidConduit,
				ObjectLayer.GasConduit,
				ObjectLayer.LogicWire,
				//ObjectLayer.SolidConduit
			];


		private bool Scissor()
		{
			var allBuildings = Components.BuildingCompletes.GetWorldItems(ClusterManager.Instance.activeWorldId);
			var maxTries = 500;
			var attempt = 0;

			while (attempt++ < maxTries)
			{
				var building = allBuildings.GetRandom();

				if (building.IsNullOrDestroyed())
					continue;

				if (building.TryGetComponent(out IHaveUtilityNetworkMgr networkManager))
				{
					var cell = Grid.PosToCell(building);
					if (!networkManager.IsNullOrDestroyed())
					{
						var connections = networkManager.GetNetworkManager().GetConnections(cell, false);

						if (connections == 0)
							continue;

						UtilityConnections[] directions = [
							UtilityConnections.Left,
							UtilityConnections.Right,
							UtilityConnections.Up,
							UtilityConnections.Down];

						directions.Shuffle();

						foreach (var direction in directions)
						{
							if (connections.HasFlag(direction))
							{
								var offset = CellOffset.none;
								var opposite = UtilityConnections.Left;

								switch (direction)
								{
									case UtilityConnections.Left:
										offset = CellOffset.left;
										opposite = UtilityConnections.Right;
										break;
									case UtilityConnections.Right:
										offset = CellOffset.right;
										opposite = UtilityConnections.Left;
										break;
									case UtilityConnections.Up:
										offset = CellOffset.up;
										opposite = UtilityConnections.Down;
										break;
									case UtilityConnections.Down:
										offset = CellOffset.down;
										opposite = UtilityConnections.Up;
										break;
								}

								var targetCell = Grid.OffsetCell(cell, offset);
								var objectLayer = building.Def.ObjectLayer;

								if (!Grid.IsValidCell(targetCell) || Grid.WorldIdx[targetCell] != building.GetMyWorldId())
									continue;

								var targetBuilding = Grid.ObjectLayers[(int)objectLayer][targetCell];

								if (targetBuilding != null && targetBuilding.TryGetComponent(out BuildingComplete buildingComplete2))
								{
									DisconnectCells(cell, building, networkManager, direction);
									DisconnectCells(targetCell, buildingComplete2, networkManager, opposite);
									AudioUtil.PlaySound(ModAssets.Sounds.SNIP, ModAssets.GetSFXVolume() * 0.9f);

									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		private void DisconnectCells(int cell, BuildingComplete building, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
		{
			if (building.TryGetComponent(out KAnimGraphTileVisualizer visualizer))
			{
				var newConnections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
				visualizer.UpdateConnections(newConnections);
				visualizer.Refresh();
			}

			TileVisualizer.RefreshCell(cell, building.Def.TileLayer, building.Def.ReplacementLayer);
			utilityComponent.GetNetworkManager().ForceRebuildNetworks();
		}

		public override void Run()
		{
			rolledHand.Shuffle();

			foreach (var hand in rolledHand)
			{
				if ((bool)(hand.runFn?.Invoke()))
				{
					ToastManager.InstantiateToast(
						STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.TOAST,
						hand.message);

					return;
				};
			}

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.TOAST,
				"You won this time >.> ");
		}

		public class Hand(string message, Func<bool> runFn)
		{
			public readonly string message = message;
			public Func<bool> runFn = runFn;
		}
	}
}
