﻿using ONITwitchLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts
{
	public class ThingSpawner : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public float minDelay;
		[SerializeField] public float maxDelay;
		[SerializeField] public List<Tag> prefabTags;
		[SerializeField] public int radius;
		[SerializeField] public int minCount;
		[SerializeField] public int maxCount;
		[SerializeField] public string soundFx;
		[SerializeField] public float z;
		[SerializeField] public float volume;
		[SerializeField] public Grid.SceneLayer sceneLayer;
		[SerializeField] public bool followCursor;

		private float elapsed;
		private int spawnedObjectCount;
		private float nextDelay;
		private int count;
		private Func<int, bool> cellCheckFn;

		public ThingSpawner()
		{
			volume = 1.0f;
		}

		public void Begin(Func<int, bool> cellCheckFn)
		{
			this.cellCheckFn = cellCheckFn;
			nextDelay = Random.Range(minDelay, maxDelay);
			count = Random.Range(minCount, maxCount + 1);
		}

		public void Sim33ms(float dt)
		{
			elapsed += dt;

			if (elapsed > nextDelay)
			{
				if (!SpawnObject() || spawnedObjectCount >= count)
				{
					Util.KDestroyGameObject(gameObject);
					return;
				}

				elapsed = 0;
				nextDelay = Random.Range(minDelay, maxDelay);
			}
		}

		void Update()
		{
			if (followCursor)
			{
				var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
				transform.position = position;
			}
		}

		private bool SpawnObject()
		{
			var tries = 32;
			for (int i = tries; i >= 0; i--)
			{
				var cell = PosUtil.ClampedMouseCellWithRange(radius);
				if (cellCheckFn(cell))
				{
					spawnedObjectCount++;
					var pos = Grid.CellToPosCBC(cell, sceneLayer) with { z = z };
					FUtility.Utils.Spawn(prefabTags.GetRandom(), pos, sceneLayer);

					if (soundFx != null)
						AudioUtil.PlaySound(soundFx, pos, ModAssets.GetSFXVolume() * volume, Random.Range(0.8f, 1.2f));

					return true;
				}
			}

			return false;
		}
	}
}
