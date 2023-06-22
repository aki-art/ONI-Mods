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
		private ParticleSystem renderer;

		private HashSet<int> alreadyVisitedCells;
		private HashSet<int> cells;

		private static HashSet<SimHashes> golds = new()
		{
			SimHashes.Gold,
			SimHashes.GoldAmalgam,
			SimHashes.FoolsGold
		};

		private static Dictionary<SimHashes, SimHashes> elementLookup = new()
		{
			{ SimHashes.Water, SimHashes.DirtyWater},
			{ SimHashes.Oxygen, SimHashes.ContaminatedOxygen},
			{ SimHashes.SaltWater, SimHashes.DirtyWater},
			{ SimHashes.Ice, SimHashes.DirtyIce},
			{ SimHashes.Magma, SimHashes.MoltenGold},
			{ SimHashes.CrudeOil, SimHashes.Petroleum},
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
			renderer = markerGo.GetComponent<ParticleSystem>();

			ModAssets.ConfigureSparkleCircle(markerGo, radius, new Color(1f, 1f, 0.4f));

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


		/*		private void CheckEntities(int cell)
				{
					var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

					GameScenePartitioner.Instance.GatherEntries(
						Extents.OneCell(cell),
						GameScenePartitioner.Instance.pickupablesLayer,
						entries);

					foreach (var entry in entries)
					{
						if(entry.obj is Pickupable pickupable)
						{
							if(pickupable.TryGetComponent(out MinionIdentity minionIdentity))
							{
								var equipment = minionIdentity.GetEquipment();
								if (equipment == null)
									continue;

								var suit = equipment.GetSlot(Db.Get().AssignableSlots.Outfit);

								if (suit.assignable.IsPrefabID(FunkyVestConfig.ID))
									continue;

								if(suit != null)
									suit.Unassign();

								var snazzy = FUtility.Utils.Spawn(FunkyVestConfig.ID, minionIdentity.transform.position);
								equipment.Equip(snazzy.GetComponent<Equippable>());
							}
						}
					}

					entries.Recycle();
				}
		*/

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
				SimMessages.ReplaceAndDisplaceElement(
					cell,
					newElement,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}
			else if(element.HasTag(GameTags.Metal) && element.IsLiquid && element.id != SimHashes.Mercury)
			{
				SimMessages.ReplaceAndDisplaceElement(
					cell,
					SimHashes.Gold,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}

			if (CheckTiles(cell))
				return;

			if (golds.Contains(element.id))
				return;

			// solids
			if (element.IsSolid && element.id != SimHashes.Gold)
			{
				SimMessages.ReplaceElement(
					cell,
					golds.GetRandom(),
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[cell],
					Grid.Temperature[cell],
					Grid.DiseaseIdx[cell],
					Grid.DiseaseCount[cell]);
			}
		}

		private bool CheckTiles(int cell)
		{
			if (Grid.HasDoor[cell])
				return true;

			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (go.PrefabID() == goldGlassTileId)
						return true;

					if (go.HasTag("DecorPackA_StainedGlass"))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn gold tile", _ => SpawnTile(cell, goldGlassTileId, goldGlassTile.DefaultElements().ToArray(), temp));
						deconstructable.ForceDestroyAndGetMaterials();
						return true;
					}

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
								[0] = SimHashes.Gold.CreateTag()
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
					}
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
