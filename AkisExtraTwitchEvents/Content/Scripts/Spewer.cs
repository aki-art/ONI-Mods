using FUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	internal class Spewer : KMonoBehaviour
	{
		private static readonly CellElementEvent cellElementEvent = new("AkisExtraTwitchEvents_SpewerSpawn", "event", false);

		[SerializeField] public List<ItemOption> options;
		[SerializeField] public float minDistance, maxDistance;
		[SerializeField] public float minDelay, maxDelay;
		[SerializeField] public bool rotate;
		[SerializeField] public bool onlyUp;
		[SerializeField] public float minAmount, maxAmount;
		[SerializeField] public SpawnFXHashes fx;
		[SerializeField] public string sound;
		[SerializeField] public float volume;
		[SerializeField] public Transform targetTransform;

		private float amountToSpawn;
		private float spawnedAmount;

		private void ValidateOptions()
		{
			if (options == null)
			{
				Log.Warning($"Tried to instantiate a Spewer component on {name}, but the options are null.");
				Util.KDestroyGameObject(gameObject);
				return;
			}

			options.RemoveAll(Invalid);

			if (options.Count < 0)
			{
				Log.Warning($"Tried to instantiate a Spewer component on {name}, but there are no valid options to spew.");
				Util.KDestroyGameObject(gameObject);
				return;
			}
		}

		public void StartSpewing()
		{
			ValidateOptions();
			spawnedAmount = 0;
			amountToSpawn = Random.Range(minAmount, maxAmount);
			StartCoroutine(SpewStuff());
		}

		private bool Invalid(ItemOption option)
		{
			if (option == null)
				return true;

			if (option.type == ItemType.Entity &&
				Assets.GetPrefab(option.tag) == null)
				return true;

			var isElement = option.type == ItemType.FallingLiquid || option.type == ItemType.Tile || option.type == ItemType.Bubble;
			if (isElement && ElementLoader.GetElement(option.tag) == null)
				return true;

			return false;
		}

		public IEnumerator SpewStuff()
		{
			while (spawnedAmount < amountToSpawn)
			{
				Log.Debuglog("while");

				var item = options.GetWeightedRandom();

				switch (item.type)
				{
					case ItemType.Entity:
						var go = FUtility.Utils.Spawn(item.tag, targetTransform.position);
						if(item.mass.HasValue)
							go.GetComponent<PrimaryElement>().Mass = item.mass.Value;
						FUtility.Utils.YeetRandomly(go, onlyUp, minDistance, maxDistance, rotate);
						break;

					case ItemType.Tile:
						var element = ElementLoader.GetElement(item.tag);
						SimMessages.ReplaceAndDisplaceElement(
							Grid.PosToCell(targetTransform.position),
							ElementLoader.GetElement(item.tag).id,
							cellElementEvent,
							item.mass ?? element.defaultValues.mass);
						break;

					case ItemType.FallingLiquid:
						var element2 = ElementLoader.GetElement(item.tag);
						FallingWater.instance.AddParticle(
							Grid.PosToCell(targetTransform.position),
							element2.idx,
							item.mass ?? element2.defaultValues.mass,
							element2.defaultValues.temperature,
							byte.MaxValue,
							0);
						break;
				}

				spawnedAmount += item.spawnValue;

				if (fx != SpawnFXHashes.None)
					Game.Instance.SpawnFX(fx, targetTransform.position, 0);

				if(!sound.IsNullOrWhiteSpace())
					AudioUtil.PlaySound(sound, targetTransform.position, ModAssets.GetSFXVolume() * volume);

				yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
			}

			Util.KDestroyGameObject(gameObject);
			yield return null;
		}

		[System.Serializable]
		public class ItemOption : ProcGen.IWeighted
		{
			public float weight { get; set; } = 1f;
			public Tag tag;
			public float? mass;
			public float spawnValue = 1f;
			public ItemType type = ItemType.Entity;

			public ItemOption(Tag tag, float? mass = null, float weight = 1f, ItemType type = ItemType.Entity, float spawnValue = 1f)
			{
				this.weight = weight;
				this.tag = tag;
				this.mass = mass;
				this.spawnValue = spawnValue;
				this.type = type;
			}
		}

		public enum ItemType
		{
			Entity,
			Tile,
			Bubble,
			FallingLiquid
		}
	}
}
