using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class SlimeToucher : Toucher, ISim33ms
	{
		[SerializeField] public float lifeTime;
		[SerializeField] public float radius;
		[SerializeField] public int cellsPerUpdate;
		[SerializeField] public float morbChance;
		[SerializeField] public float fungusChance;

		private float elapsedLifeTime = 0;

		private BuildingDef slimeGlassTile;
		private string slimeGlassTileId = "DecorPackA_SlimeMoldStainedGlassTile";
		private Transform circleMarker;
		public PlantableSeed plantableSeed;

		private HashSet<int> cells;

		private static readonly HashSet<SimHashes> slimes = new()
		{
			SimHashes.SlimeMold
		};

		private static readonly Dictionary<SimHashes, SimHashes> elementLookup = new()
		{
		};

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			slimeGlassTile = Assets.GetBuildingDef("DecorPackA_GoldStainedGlassTile");
			alreadyVisitedCells = new HashSet<int>();
			cells = new HashSet<int>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			var seedPrefab = Assets.TryGetPrefab(MushroomPlantConfig.SEED_ID);
			if (seedPrefab != null)
				plantableSeed = seedPrefab.GetComponent<PlantableSeed>();

			if (ModAssets.Prefabs.sparkleCircle == null)
			{
				Debug.LogWarning("marker is null");
				return;
			}

			var markerGo = Instantiate(ModAssets.Prefabs.sparkleCircle);
			markerGo.transform.SetParent(transform);
			markerGo.gameObject.SetActive(true);

			circleMarker = markerGo.transform;

			ModAssets.ConfigureSparkleCircle(markerGo, radius, Util.ColorFromHex("455b00"));

			Mod.touchers.Add(this);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.touchers.Remove(this);
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

		public void Sim33ms(float dt)
		{
			elapsedLifeTime += dt;

			if (elapsedLifeTime > lifeTime)
			{
				circleMarker.GetComponent<ParticleSystem>().Stop();
				Util.KDestroyGameObject(gameObject);
				return;
			}

			var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			var cells = GetTilesInRadius(position, radius);
			var worldIdx = this.GetMyWorldId();

			foreach (var cell in cells)
			{
				if (Grid.IsValidCellInWorld(cell, worldIdx))
					TurnToSlime(cell);
			}
		}

		private void TurnToSlime(int cell)
		{
			if (alreadyVisitedCells.Contains(cell))
				return;

			alreadyVisitedCells.Add(cell);

			var element = Grid.Element[cell];

			if (element == null)
				return;

			//liquids
			if (element.IsLiquid &&
				!slimes.Contains(element.id)
				&& !element.HasTag(GameTags.Metal))
			{
				SimMessages.ReplaceAndDisplaceElement(
					cell,
					Elements.PinkSlime,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}
			else if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				SimMessages.ReplaceAndDisplaceElement(
					cell,
					newElement,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}

			if (CheckTiles(cell))
				return;

			if (slimes.Contains(element.id))
				return;

			// solids
			if (element.IsSolid && !slimes.Contains(element.id))
			{
				SimMessages.ReplaceElement(
					cell,
					slimes.GetRandom(),
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}
			else if (element.IsGas)
			{
				var position = Grid.CellToPos(cell);

				if (Random.value < morbChance)
					FUtility.Utils.Spawn(GlomConfig.ID, position);

				if(Random.value < fungusChance)
				{
					if (IsNaturalCell(Grid.CellBelow(cell)) &&
						!Grid.IsSolidCell(Grid.CellAbove(cell)) &&
						plantableSeed.TestSuitableGround(cell))
					{
						var seed = FUtility.Utils.Spawn(MushroomPlantConfig.SEED_ID, position);
						PlantAndGrow(seed.GetComponent<PlantableSeed>(), position);

						Game.Instance.SpawnFX(ModAssets.Fx.fungusPoof, cell, 0);
						AudioUtil.PlaySound(ModAssets.Sounds.POP, position, ModAssets.GetSFXVolume());
					}
				}
			}
		}

		public void PlantAndGrow(PlantableSeed seed, Vector3 position)
		{
			var gameObject = FUtility.Utils.Spawn(seed.PlantID, position);
			gameObject.SetActive(true);

			var pickupable = seed.pickupable.Take(1f);

			if (pickupable != null)
				Util.KDestroyGameObject(pickupable.gameObject);
			else
				Log.Warning("Seed has fractional total amount < 1f");

			gameObject.GetComponent<Growing>().OverrideMaturityLevel(Random.Range(0.3f, 1f));
		}

		public static bool IsNaturalCell(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.Solid[cell] && Grid.Objects[cell, (int)ObjectLayer.FoundationTile] == null;
		}

		private bool CheckTiles(int cell)
		{
			if (Grid.HasDoor[cell])
				return true;

			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (go.PrefabID() == slimeGlassTileId)
						return true;

					if (go.HasTag("DecorPackA_StainedGlass"))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn slime tile", _ => SpawnTile(cell, slimeGlassTileId, slimeGlassTile.DefaultElements().ToArray(), temp));
						deconstructable.ForceDestroyAndGetMaterials();
						return true;
					}
					/*
										if (deconstructable.constructionElements != null && deconstructable.constructionElements.Length > 0)
										{
											var primary = deconstructable.constructionElements[0];
											var element = ElementLoader.GetElement(primary);

											if (element != null && (element.HasTag(GameTags.Ore) || element.HasTag(GameTags.RefinedMetal)))
											{
												var def = Assets.GetBuildingDef(go.PrefabID().ToString());

												if (def == null)
													return true;

												var newElements = new List<Tag>(def.DefaultElements())
												{
													[0] = SimHashes.SlimeMold.CreateTag()
												};

												var primaryElement = go.GetComponent<PrimaryElement>();

												if (primaryElement == null)
												{
													Log.Debuglog("primary element null");
													return true;
												}

												var temp = primaryElement.Temperature;
												var prefabId = go.PrefabID().ToString();

												GameScheduler.Instance.ScheduleNextFrame(
													"spawn gold tile",
													_ => SpawnTile(cell, prefabId, newElements.ToArray(), temp));

												deconstructable.ForceDestroyAndGetMaterials();
											}
										}*/
				}

				return true;
			}

			return false;
		}

		private void SpawnTile(int cell, string prefabId, Tag[] elements, float temperature)
		{
			var def = Assets.GetBuildingDef(prefabId);

			if (def == null)
				return;

			def.Build(cell, Orientation.Neutral, null, elements, temperature, false, GameClock.Instance.GetTime() + 1);
			World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
		}

		private HashSet<int> GetTilesInRadius(Vector3 position, float radius)
		{
			cells.Clear();
			for (int i = 0; i < cellsPerUpdate; i++)
			{
				var offset = position + (Vector3)(UnityEngine.Random.insideUnitCircle * radius);
				cells.Add(Grid.PosToCell(offset));
			}

			return cells;
		}
	}
}
