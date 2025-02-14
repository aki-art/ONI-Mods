using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Scripts;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class ColossalFartEvent() : TwitchEventBase("ColossalFart")
	{
		public override Danger GetDanger() => Danger.High;

		public override int GetWeight() => WEIGHTS.COMMON;

		public override void Run()
		{
			var cell = PosUtil.ClampedMouseCell();

			var cells = ExplosionUtil.ExplodeInRadius(cell, 100f, new Vector2(8, 14), 10);

			foreach (var c in cells)
				if (GridUtil.IsCellFoundationEmpty(c))
					SimMessages.ReplaceElement(c, SimHashes.Methane, Toucher.toucherEvent, 10);

			if (GridUtil.IsCellFoundationEmpty(cell))
				SimMessages.ReplaceElement(cell, SimHashes.Methane, Toucher.toucherEvent, 200);

			AudioUtil.PlaySound(ModAssets.Sounds.FART_REVERB, ModAssets.GetSFXVolume() * 1.2f);
			AudioUtil.PlaySound(ModAssets.Sounds.ROCK, ModAssets.GetSFXVolume() * 0.5f);

			/*			var fx = FXHelpers.CreateEffect("aete_methane_explosion_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront2), layer: Grid.SceneLayer.FXFront2);
						fx.Play("idle");
						fx.destroyOnAnimComplete = true;*/

			var shockwaveFx = new GameObject("AETE_Fart_fx");
			shockwaveFx.transform.position = Grid.CellToPos(cell) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront) };
			var viz = shockwaveFx.AddComponent<ShockwaveVisualizer>();

			shockwaveFx.SetActive(true);

			viz.Play();
		}
	}
}
