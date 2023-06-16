using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MidasToucher : KMonoBehaviour, ISim33ms
	{
		[SerializeField] public float lifeTime;
		[SerializeField] public float radius;
		[SerializeField] public int cellsPerUpdate;

		private float elapsedLifeTime = 0;

		private BuildingDef metalTile;
		private BuildingDef meshTile;
		private BuildingDef goldGlassTile;
		private string goldGlassTileId = "DecorPackA_GoldStainedGlassTile";
		private Transform circleMarker;

		private HashSet<int> alreadyVisitedCells;
		private HashSet<int> cells;

		private static Dictionary<SimHashes, SimHashes> elementLookup = new()
		{
			{ SimHashes.Water, SimHashes.DirtyWater},
			{ SimHashes.Oxygen, SimHashes.ContaminatedOxygen},
			{ SimHashes.SaltWater, SimHashes.DirtyWater},
			{ SimHashes.Ice, SimHashes.DirtyIce},
			{ SimHashes.Magma, SimHashes.MoltenGold},
		};

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			metalTile = Assets.GetBuildingDef(MetalTileConfig.ID);
			goldGlassTile = Assets.GetBuildingDef("DecorPackA_GoldStainedGlassTile");
			alreadyVisitedCells = new HashSet<int>();
			cells = new HashSet<int>();
		}

		public override void OnSpawn()
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

			ModAssets.ConfigureSparkleCircle(markerGo, radius / 2f, new Color(1f, 1f, 0.4f));

			Mod.midasTouchers.Add(this);
		}

		public void IgnoreCell(int cell)
		{
			alreadyVisitedCells?.Add(cell);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.midasTouchers.Remove(this);
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
					TurnToGold(cell);
			}
		}

		private void TurnToGold(int cell)
		{
			if (alreadyVisitedCells.Contains(cell))
				return;

			alreadyVisitedCells.Add(cell);

			var element = Grid.Element[cell];

			if (element == null)
				return;

			// gas & liquid
			if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				SimMessages.ReplaceElement(
					cell,
					newElement,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}

			// solids
			if (element.IsSolid && element.id != SimHashes.Gold)
			{
				SimMessages.ReplaceElement(
					cell,
					SimHashes.Gold,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}

			UpgradeSingleTile(cell);
		}

		private void UpgradeSingleTile(int cell)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (go.PrefabID() == goldGlassTileId)
						return;

					if (go.HasTag("DecorPackA_StainedGlass"))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn gold tile", _ => SpawnTile(cell, goldGlassTileId, goldGlassTile.DefaultElements().ToArray(), temp));
						deconstructable.ForceDestroyAndGetMaterials();
						return;
					}

					if (deconstructable.constructionElements != null && deconstructable.constructionElements.Length > 0)
					{
						var primary = deconstructable.constructionElements[0];
						var element = ElementLoader.GetElement(primary);

						if (element != null && (element.HasTag(GameTags.Ore) || element.HasTag(GameTags.RefinedMetal)))
						{
							var def = Assets.GetBuildingDef(go.PrefabID().ToString());

							if (def == null)
								return;

							var newElements = new List<Tag>(def.DefaultElements())
							{
								[0] = SimHashes.Gold.CreateTag()
							};

							var primaryElement = go.GetComponent<PrimaryElement>();

							if (primaryElement == null)
							{
								Log.Debuglog("primary element null");
								return;
							}

							var temp = primaryElement.Temperature;
							var prefabId = go.PrefabID().ToString();

							GameScheduler.Instance.ScheduleNextFrame(
								"spawn gold tile",
								_ => SpawnTile(cell, prefabId, newElements.ToArray(), temp));

							deconstructable.ForceDestroyAndGetMaterials();
						}
					}
				}
			}
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
			for(int i = 0; i < cellsPerUpdate; i++)
			{
				var offset = position + (Vector3)(UnityEngine.Random.insideUnitCircle * radius);
				cells.Add(Grid.PosToCell(offset));
			}

			return cells;
		}
	}
}
