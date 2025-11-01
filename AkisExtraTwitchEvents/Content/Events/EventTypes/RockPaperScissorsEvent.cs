using Klei.AI;
using ONITwitchLib;
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

		private readonly List<Hand> rolledHand = [];

		public RockPaperScissorsEvent() : base(ID)
		{
			rolledHand = [
				new Rock(),
				new Paper(),
				new Scissors()
				];
		}

		public Hand PlayerHand(int cell, Element element, out string thing)
		{
			thing = global::STRINGS.UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.NOTHING;

			foreach (var hand in rolledHand)
			{
				if (hand.IsPicked(cell, element, out thing))
					return hand;
			}

			return null;
		}


		public override void Run()
		{
			rolledHand.Shuffle();

			var cursorPos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			var cursorCell = Grid.PosToCell(cursorPos);
			var element = Grid.Element[cursorCell];

			//var playerHand = PlayerHand(cursorCell, element, out var thing);
			//var id = playerHand == null ? HashedString.Invalid : playerHand.id;

			//thing = $"{Util.StripTextFormatting(element.name)} ({thing})";

			//Hand modHand = null;

			foreach (var hand in rolledHand)
			{
				if (hand.Run())
				{
					ToastManager.InstantiateToast(
						STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.TOAST,
						hand.Message(null, null));

					return;
				}

				// modHand = hand;
			}


			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.TOAST,
					"Something went wrong. You won this time >.> ");

		}

		public abstract class Hand(HashedString id)
		{
			public readonly HashedString id = id;

			public virtual bool CanBePlayed() => true;

			public virtual bool Run()
			{
				//Play(null);
				return true;
			}

			public abstract string Name();

			public abstract string LostMessage();

			public enum Result
			{
				Win,
				Lose,
				Tie
			}

			protected Result lastResult;

			public void Play(Hand otherHand)
			{
				if (otherHand.id == id)
				{
					lastResult = Result.Tie;
				}
				else if (BeatenBy() == otherHand.id)
				{
					lastResult = Result.Win;
				}
				else
				{
					lastResult = Result.Lose;
				}
			}

			public virtual string Message(Hand otherHand, string hoveredThing)
			{
				return $"{Name()}!";
				/*
								var msg = STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.MESSAGE
									.Replace("{Pick}", Name())
									.Replace("{PlayerPick}", otherHand.Name())
									.Replace("{Thing}", $"<b>{hoveredThing.ToUpperInvariant()}</b>");

								msg += "\n\n";

								msg += lastResult switch
								{
									Result.Win => (string)STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.WON,
									Result.Lose => (string)STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.LOST,
									_ => (string)STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.TIE,
								};

								return msg;*/
			}

			public abstract bool IsPicked(int cell, Element element, out string thing);


			public abstract HashedString BeatenBy();
		}

		public class Paper() : Hand("paper")
		{
			public override HashedString BeatenBy() => "scissors";

			public override string LostMessage() => STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.PAPER_LOST;

			public override string Message(Hand otherHand, string hoveredThing)
			{
				return $"{Name()}!\n{STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.PAPER_LOST}";
			}

			public override bool IsPicked(int cell, Element element, out string thing)
			{
				thing = string.Empty;

				if (element == null)
					return false;

				if (element.HasTag(GameTags.Organics))
				{
					thing = global::STRINGS.MISC.TAGS.ORGANICS;
					return true;
				}

				return false;
			}

			public override string Name() => STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.PAPER;

			public override bool Run()
			{
				foreach (var minion in Components.MinionIdentities.Items)
				{
					var effects = minion.GetComponent<Effects>();

					effects.Add(TEffects.TOILER_PAPER_STUCK, true);
				}

				return true;
			}
		}

		public class Scissors() : Hand("scissors")
		{
			private static readonly HashSet<ObjectLayer> layers =
				[
					ObjectLayer.Wire,
				ObjectLayer.LiquidConduit,
				ObjectLayer.GasConduit,
				ObjectLayer.LogicWire,
				//ObjectLayer.SolidConduit
			];

			public override string LostMessage() => STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.SCISSORS_LOST;

			public override string Message(Hand otherHand, string hoveredThing)
			{
				return $"{Name()}!\n{STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.SCISSORS_LOST}";
			}

			public override HashedString BeatenBy() => "rock";

			public override bool IsPicked(int cell, Element element, out string thing)
			{
				thing = string.Empty;

				if (element == null)
					return false;

				if (element.HasTag(GameTags.Metal) || element.HasTag(GameTags.RefinedMetal))
				{
					thing = global::STRINGS.MISC.TAGS.METAL;
					return true;
				}

				return false;
			}

			public override string Name() => STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.SCISSORS;

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

			public override bool Run()
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

		}

		public class Rock() : Hand("rock")
		{
			public override HashedString BeatenBy() => "paper";

			public override string LostMessage() => string.Empty;

			public override bool IsPicked(int cell, Element element, out string thing)
			{
				thing = string.Empty;

				if (element == null)
					return false;

				if (element.HasTag(GameTags.BuildableRaw) || element.highTempTransitionOreID == SimHashes.Magma)
				{
					thing = STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.ROCK;
					return true;
				}

				return false;
			}

			public override string Name() => STRINGS.AETE_EVENTS.ROCKPAPERSCISSORS.ROCK;

			public override bool Run()
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

					var isMinionInWay = false;
					foreach (var minion in Components.LiveMinionIdentities.items)
					{
						var minionCell = Grid.PosToCell(minion);
						if (minionCell == cell || Grid.CellAbove(minionCell) == cell)
						{
							isMinionInWay = true;
							break;
						}
					}

					if (!isMinionInWay)
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
		}
	}
}
