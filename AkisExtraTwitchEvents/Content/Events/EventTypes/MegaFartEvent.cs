using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	internal class MegaFartEvent() : TwitchEventBase("MegaFart")
	{
		public static List<CellOffset> targetTiles = FUtility.Utils.MakeCellOffsetsFromMap(false,
			" XXX ",
			"XXXXX",
			"XXOXX",
			"XXXXX",
			" XXX ");

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => WEIGHTS.COMMON;

		public override void Run()
		{
			var cell = PosUtil.ClampedMouseCell();

			SimMessages.ReplaceElement(cell, SimHashes.Methane, Toucher.toucherEvent, 100);
			ExplosionUtil.Explode(cell, 1f, new UnityEngine.Vector2(8, 14), targetTiles);
			AudioUtil.PlaySound(ModAssets.Sounds.FART_REVERB, ModAssets.GetSFXVolume());

			var fx = FXHelpers.CreateEffect("aete_methane_explosion_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
			fx.Play("idle");
			fx.destroyOnAnimComplete = true;
		}
	}
}
