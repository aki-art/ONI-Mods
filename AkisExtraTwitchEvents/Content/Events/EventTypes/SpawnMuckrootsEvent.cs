using ONITwitchLib;
using ONITwitchLib.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	internal class SpawnMuckrootsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SpawnMuckroots";

		private const int RADIUS = 7;

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var centerCell = PosUtil.ClampedMouseCell();

			if (!Grid.IsValidCell(centerCell))
				return;

			var center = Grid.CellToPos(centerCell);

			var worldId = Grid.WorldIdx[centerCell];

			var cells = ProcGen.Util.GetCircle(center, RADIUS);

			foreach (var pos in cells)
			{
				var cell = Grid.PosToCell(pos);

				if (!Grid.IsValidCellInWorld(cell, worldId))
					return;

				FUtility.Utils.Spawn(BasicForagePlantConfig.ID, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
				Game.Instance.SpawnFX(ModAssets.Fx.fungusPoof, cell, Random.Range(0, 360));
				AudioUtil.PlaySound(ModAssets.Sounds.TADA, ModAssets.GetSFXVolume() * 0.7f);

				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.SPAWNMUCKROOTS.TOAST, STRINGS.AETE_EVENTS.SPAWNMUCKROOTS.DESC);
			}
		}
	}
}
