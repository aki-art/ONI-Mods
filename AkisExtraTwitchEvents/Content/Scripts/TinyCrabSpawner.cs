

using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Critters;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class TinyCrabSpawner : KMonoBehaviour, ISim33ms
	{
		private int _crabsToSpawn;
		private bool _spewing;

		public override void OnSpawn()
		{
			base.OnSpawn();
			_crabsToSpawn = Random.Range(15, 20);
			_spewing = true;
		}

		public void Sim33ms(float dt)
		{
			if (!_spewing)
				return;

			var spawn = Random.Range(0, 3);
			for (int i = 0; i < spawn; i++)
			{
				var mousePos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
				var cell = Grid.PosToCell(mousePos);

				if (GridUtil.IsCellEmpty(cell))
				{
					_crabsToSpawn--;

					var go = FUtility.Utils.Spawn(TinyAngryCrabConfig.ID, mousePos);
					FUtility.Utils.YeetRandomly(go, true, 3, 7, false);
					AudioUtil.PlaySound(ModAssets.Sounds.POP, mousePos, ModAssets.GetSFXVolume() * 1f, Random.Range(0.8f, 1.1f));
				}

				if (_crabsToSpawn == 0)
				{
					_spewing = false;
					Util.KDestroyGameObject(gameObject);
					return;
				}
			}
		}
	}
}
