using ONITwitchLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MilkRainSpawner : KMonoBehaviour, ISim33ms, ISim200ms
	{
		[SerializeField] public string animFile;
		[SerializeField] public string animation;
		[SerializeField] public int count;
		[SerializeField] public float duration;
		[SerializeField] public float radius;
		[SerializeField] public SimHashes liquid;
		[SerializeField] public float totalLiquidSpawned;
		[SerializeField] public float temperature;
		[SerializeField] public float fadeOutDuration;
		[SerializeField] public float appearDuration;
		[SerializeField] public float finalSpeed;
		[SerializeField] public SimHashes gas;
		[SerializeField] public float gasPerTile;

		private List<OrbitingThing> anims;
		private Stage stage;
		private ushort element;
		private float massPerDroplet;
		private float elapsed;
		private static readonly HashSet<KAnimHashedString> symbolsToHide =
		[
			"snapto_pivot",
			"tag"
		];

		private static float defaultAnimScale = 0.005f;

		private enum Stage
		{
			Offline,
			Appearing,
			Running,
			Disappearing
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			element = ElementLoader.GetElementIndex(liquid);

			var massPerCow = totalLiquidSpawned / count;
			var massPerSecond = massPerCow / duration;

			massPerDroplet = massPerSecond * 0.033f;
		}

		public void Sim200ms(float dt)
		{
			if (stage == Stage.Disappearing)
			{
				foreach (var cow in anims)
				{
					var cell = Grid.PosToCell(cow.GetTrackerPosition());
					if (GridUtil.IsCellEmpty(cell))
					{
						SimMessages.ReplaceElement(
							cell,
							gas,
							Toucher.toucherEvent,
							gasPerTile,
							temperature);
					}
				}
			}
		}

		public void Sim33ms(float dt)
		{
			if (stage == Stage.Offline)
				return;

			switch (stage)
			{
				case Stage.Appearing:
					{
						if (elapsed > appearDuration)
						{
							elapsed = 0;
							stage = Stage.Running;

							foreach (var cow in anims)
							{
								cow.speed = Mathf.MoveTowards(cow.speed, finalSpeed, fadeOutDuration * dt);
								cow.kbac.animScale = defaultAnimScale;
							}

							break;
						}

						foreach (var cow in anims)
						{
							cow.speed = Mathf.MoveTowards(cow.speed, finalSpeed, fadeOutDuration * dt);
							cow.kbac.animScale = Mathf.Lerp(0, defaultAnimScale, elapsed / appearDuration);
						}

						break;
					}
				case Stage.Running:
					{
						if (elapsed > duration)
						{
							elapsed = 0;
							stage = Stage.Disappearing;

							AudioUtil.PlaySound(ModAssets.Sounds.BALLOON_DEFLATE.GetRandom(), transform.position, ModAssets.GetSFXVolume() * 3f, Random.Range(0.9f, 1f));

							foreach (var cow in anims)
							{
								cow.maxSpeed = Mathf.MoveTowards(cow.maxSpeed, 4f, 0.1f);
							}
						}
						else
						{
							foreach (var cow in anims)
							{
								FallingWater.instance.AddParticle(cow.GetTrackerPosition(), element, massPerDroplet, temperature, byte.MaxValue, 0, skip_decor: true);
							}
						}

						break;
					}

				case Stage.Disappearing:
					{
						if (elapsed > fadeOutDuration)
						{
							foreach (var cow in anims)
							{
								Util.KDestroyGameObject(cow.gameObject);
							}

							Util.KDestroyGameObject(gameObject);
							return;
						}
						else
						{
							foreach (var cow in anims)
							{
								cow.speed = Mathf.MoveTowards(cow.speed, finalSpeed, fadeOutDuration * dt);
								cow.kbac.animScale = Mathf.Lerp(defaultAnimScale, 0f, elapsed / fadeOutDuration);
							}
						}

						break;
					}
			}

			elapsed += dt;
		}

		public void StartRaining()
		{
			anims = [];

			for (int i = 0; i < count; i++)
			{
				var kbac = FXHelpers.CreateEffect(animFile, transform.position, transform.parent, false, Grid.SceneLayer.FXFront2);
				kbac.destroyOnAnimComplete = false;
				kbac.Play(animation, KAnim.PlayMode.Loop);
				kbac.isMovable = true;
				kbac.transform.parent = transform.parent;
				kbac.transform.position = transform.position;

				kbac.gameObject.SetActive(true);
				defaultAnimScale = kbac.animScale;
				kbac.animScale = 0;
				kbac.BatchSetSymbolsVisiblity(symbolsToHide, false);

				var orbiter = kbac.gameObject.AddComponent<OrbitingThing>();
				orbiter.orbitSpeedModifier = new Vector2(1f, 1.5f);
				orbiter.erratic = 0.005f;
				orbiter.maxSpeed = 8f;
				orbiter.damp = 0.94f;
				orbiter.speed = 25f;

				orbiter.Begin();

				anims.Add(orbiter);
			}

			stage = Stage.Appearing;
		}
	}
}
