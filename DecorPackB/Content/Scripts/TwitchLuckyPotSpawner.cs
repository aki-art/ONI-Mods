using DecorPackB.Content.Defs.Buildings;
using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class TwitchLuckyPotSpawner : KMonoBehaviour, ISim200ms
	{
		private Transform circleMarker;

		[SerializeField]
		public float duration;

		[SerializeField]
		public int radius;

		private float elapsedTime = 0;

		private static readonly List<Tag> clay = new List<Tag>()
		{
			SimHashes.Clay.CreateTag()
		};

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (ModAssets.Prefabs.sparkleCircle == null)
			{
				Debug.LogWarning("marker is null");
				return;
			}

			var markerGo = Instantiate(ModAssets.Prefabs.sparkleCircle);
			markerGo.transform.SetParent(transform);
			markerGo.gameObject.SetActive(true);

			circleMarker = markerGo.transform;

			ModAssets.ConfigureSparkleCircle(markerGo, radius * 2, new Color(1f, 1f, 0.4f));
			ONITwitchLib.ToastManager.InstantiateToast("Lucky Pots!", "Pots are appearing filled with goodies!");
		}

		void Update()
		{
			if (circleMarker != null)
			{
				var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
				circleMarker.position = position;
			}
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();

			if (circleMarker != null)
			{
				Destroy(circleMarker.gameObject);
			}
		}

		private bool IsLocationPotValid(int cell, BuildingDef def)
		{
			bool valid = true;

			def.RunOnArea(cell, Orientation.Neutral, offset =>
			{
				if (!Grid.IsValidBuildingCell(offset) || Grid.IsSolidCell(offset))
				{
					valid = false;
				}
			});

			if (!valid)
			{
				return false;
			}

			return def.IsValidPlaceLocation(def.BuildingComplete, cell, Orientation.Neutral, out string _)
				 && def.IsValidBuildLocation(def.BuildingComplete, Grid.CellToPos(cell), Orientation.Neutral);
		}

		public void Sim200ms(float dt)
		{
			if (elapsedTime > duration)
			{
				circleMarker.GetComponent<ParticleSystem>().Stop();
				return;
			}

			var mouse = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			Grid.CellToXY(Grid.PosToCell(this), out int x, out int y);

			var cellsInRadius = ProcGen.Util.GetFilledCircle(new Vector2I(x, y), radius);
			cellsInRadius.Shuffle();

			var def = Assets.GetBuildingDef(PotConfig.ID);

			foreach (var offset in cellsInRadius)
			{
				var pos = mouse + new Vector3(offset.X, offset.Y);
				var cell = Grid.PosToCell(pos);

				if (IsLocationPotValid(cell, def))
				{
					var potGo = def.Build(cell, Orientation.Neutral, null, clay, 300);

					if (potGo != null)
					{
						GameScheduler.Instance.ScheduleNextFrame("Set Pot", _ =>
						{
							if (potGo != null
								&& potGo.TryGetComponent(out Pot pot)
								&& potGo.TryGetComponent(out Storage storage)
								&& potGo.TryGetComponent(out TreeFilterable treeFilterable))
							{
								pot.SetRandomStage();
								var item = SpawnItem();
								storage.Store(item);
								treeFilterable.UpdateFilters(new HashSet<Tag>() { item.PrefabID() });
							}
						});

						break;
					}
				}
			}

			elapsedTime += dt;
		}

		private static readonly List<(Tag tag, float amount)> possibleItems = new List<(Tag, float)>()
		{
			(BasicFabricConfig.ID, 20),
			(SimHashes.Fossil.CreateTag(), 100f),
			(SimHashes.Lime.CreateTag(), 100f),
			(SimHashes.Obsidian.CreateTag(), 1000f),
			(SimHashes.GoldAmalgam.CreateTag(), 1000f),
			(SimHashes.Wolframite.CreateTag(), 1000f),
			(SimHashes.Steel.CreateTag(), 300f),
			(SimHashes.Katairite.CreateTag(), 1000f),
			(SimHashes.Isoresin.CreateTag(), 1000f),
			(SimHashes.SuperInsulator.CreateTag(), 200f),
			(SimHashes.Sulfur.CreateTag(), 2000f),
			(SimHashes.ViscoGel.CreateTag(), 2000f),
			(SimHashes.Water.CreateTag(), 5000f),
			(SimHashes.Lead.CreateTag(), 2000f),
			(SimHashes.Cuprite.CreateTag(), 2000f),
			(SimHashes.CrudeOil.CreateTag(), 3000f),
			(SimHashes.Algae.CreateTag(), 2000f),
			(BasicFabricMaterialPlantConfig.SEED_ID, 20),
			(BasicSingleHarvestPlantConfig.SEED_ID, 20),
			(BeanPlantConfig.SEED_ID, 20),
			(SaltPlantConfig.SEED_ID, 20)
		};

		private GameObject SpawnItem()
		{
			var prefab = possibleItems.GetRandom();
			var item = Utils.Spawn(prefab.tag, Vector3.zero);
			item.GetComponent<PrimaryElement>().Mass = prefab.amount;
			return item;
		}
	}
}
