using ONITwitchLib;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class MacaroniEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Macaroni";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var pos = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			var center = Grid.PosToCell(pos);
			var worldIdx = Grid.WorldIdx[center];

			var positions = ProcGen.Util.GetBlob(pos, 1, new KRandom());

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

				if (!isMinionInWay)
					SimMessages.ReplaceAndDisplaceElement(cell, Elements.Macaroni, AGridUtil.cellEvent, 200);
			}

			var fx = FXHelpers.CreateEffect("aete_puff_kanim", Grid.CellToPosCCC(center, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
			fx.TintColour = new Color(0.6f, 0.6f, 0.6f, 1f);
			fx.animScale *= 1.5f;
			fx.Play("idle");
			fx.destroyOnAnimComplete = true;

			AudioUtil.PlaySound(ModAssets.Sounds.PASTA, ModAssets.GetSFXVolume() * 1f);
			ToastManager.InstantiateToastWithPosTarget(STRINGS.AETE_EVENTS.MACARONI.TOAST, STRINGS.AETE_EVENTS.MACARONI.DESC, pos);
		}
	}
}
