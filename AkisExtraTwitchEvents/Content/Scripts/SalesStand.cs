using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class SalesStand : ComplexFabricator
	{
		[MyCmpAdd] private SetLocker locker;

		[SerializeField] public int minRummagableCountPerItem = 2;
		[SerializeField] public int maxRummagableCountPerItem = 6;

		public override void OnSpawn()
		{
			base.OnSpawn();
			locker.numDataBanks = new int[] { }; // game assumes non null
			Subscribe((int)GameHashes.LockerDroppedContents, _ => Rummage());
		}

		public void Rummage()
		{
			var items = new List<ComplexRecipe>(GetRecipes());
			items.Shuffle();

			var itemCount = 0;
			while (itemCount <= 5)
			{
				foreach (var rummagable in items)
				{
					foreach (var result in rummagable.results)
					{
						var amount = result.amount * Random.Range(minRummagableCountPerItem, maxRummagableCountPerItem + 1);
						var go = FUtility.Utils.Spawn(result.material, transform.gameObject);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, result.material.ProperName(), go.transform);
						FUtility.Utils.YeetRandomly(go, true, 1, 4, true);
						itemCount++;
					}
				}
			}
		}
	}
}
