using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	internal class MeatballEvent() : TwitchEventBase(ID)
	{
		public const string ID = "MeatBall";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Common;

		private enum Doneness
		{
			ExtraRare,
			Rare,
			MediumRare,
			Medium,
			MediumWell,
			WellDone,
			ExtraWellDone
		}

		public override void Run()
		{
			var pos = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			var center = Grid.PosToCell(pos);
			var worldIdx = Grid.WorldIdx[center];


			var doneness = (Doneness)UnityEngine.Random.Range(0, 7);

			SimHashes innerMaterial = Elements.FakeMeat;
			SimHashes outerMaterial = Elements.FakeMeat;
			var innerRadius = 0;
			var message = "";

			switch (doneness)
			{
				case Doneness.ExtraRare:
					innerMaterial = Elements.FakeMeat;
					outerMaterial = Elements.FakeMeat;
					message = STRINGS.AETE_EVENTS.MEATBALL.EXTRARARE;
					innerRadius = 0;
					break;
				case Doneness.Rare:
					innerMaterial = Elements.FakeMeat;
					outerMaterial = Elements.FakeBarbeque;
					message = STRINGS.AETE_EVENTS.MEATBALL.RARE;
					innerRadius = 2;
					break;
				case Doneness.MediumRare:
					innerMaterial = Elements.FakeMeat;
					outerMaterial = Elements.FakeBarbeque;
					message = STRINGS.AETE_EVENTS.MEATBALL.MEDIUMRARE;
					innerRadius = 4;
					break;
				case Doneness.Medium:
					innerMaterial = Elements.FakeBarbeque;
					outerMaterial = Elements.FakeBarbeque;
					message = STRINGS.AETE_EVENTS.MEATBALL.MEDIUM;
					innerRadius = 0;
					break;
				case Doneness.MediumWell:
					innerMaterial = Elements.FakeBarbeque;
					outerMaterial = SimHashes.Carbon;
					message = STRINGS.AETE_EVENTS.MEATBALL.MEDIUMWELLDONE;
					innerRadius = 4;
					break;
				case Doneness.WellDone:
					innerMaterial = Elements.FakeBarbeque;
					outerMaterial = SimHashes.Carbon;
					message = STRINGS.AETE_EVENTS.MEATBALL.WELLDONE;
					innerRadius = 2;
					break;
				case Doneness.ExtraWellDone:
					innerMaterial = Elements.FakeBarbeque;
					outerMaterial = SimHashes.Carbon;
					message = STRINGS.AETE_EVENTS.MEATBALL.EXTRAWELLDONE;
					innerRadius = 1;
					break;
			}

			var circle = ProcGen.Util.GetFilledCircle(pos, 5);
			var blob = ProcGen.Util.GetBlob(pos, innerRadius, new KRandom());

			var outerPositions = circle.Except(blob);
			var innerPositions = blob.Intersect(circle);

			PlaceElement(worldIdx, outerPositions, outerMaterial);
			PlaceElement(worldIdx, innerPositions, innerMaterial);

			var fx = FXHelpers.CreateEffect("aete_puff_kanim", Grid.CellToPosCCC(center, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
			fx.TintColour = Util.ColorFromHex("c42e23");
			fx.animScale *= 1.5f;
			fx.Play("idle");
			fx.destroyOnAnimComplete = true;


			AudioUtil.PlaySound(ModAssets.Sounds.TADA, ModAssets.GetSFXVolume() * 1f);
			ToastManager.InstantiateToastWithPosTarget(STRINGS.AETE_EVENTS.MEATBALL.TOAST, $"{message} {STRINGS.AETE_EVENTS.MEATBALL.DESC}", pos);
		}

		private static void PlaceElement(byte worldIdx, IEnumerable<Vector2I> positions, SimHashes elementId)
		{
			foreach (var position in positions)
			{
				var cell = Grid.PosToCell(position);

				if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] != worldIdx)
					continue;

				var isMinionInWay = false;

				if (!Mod.Settings.Cursed)
				{
					foreach (var minion in Components.LiveMinionIdentities.items)
					{
						var minionCell = Grid.PosToCell(minion);
						if (minionCell == cell || Grid.CellAbove(minionCell) == cell)
						{
							isMinionInWay = true;
							break;
						}
					}
				}

				var mass = elementId == SimHashes.Carbon ? 200 : 0.25f;

				if (!isMinionInWay)
					SimMessages.ReplaceAndDisplaceElement(cell, elementId, AGridUtil.cellEvent, mass, GameUtil.GetTemperatureConvertedToKelvin(60, GameUtil.TemperatureUnit.Celsius));
			}
		}
	}
}
