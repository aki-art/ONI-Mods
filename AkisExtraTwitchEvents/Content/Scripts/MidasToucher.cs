using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class MidasToucher : KMonoBehaviour, ISim200ms
	{
		[SerializeField]
		public float lifeTime;

		[SerializeField]
		public float radius;

		private float elapsedLifeTime = 0;

		private BuildingDef metalTile;
		private BuildingDef meshTile;
		private BuildingDef goldGlassTile;
		private string goldGlassTileId = "DecorPackA_GoldStainedGlassTile";


		private HashSet<int> alreadyVisitedCells;

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
		}

		public void Sim200ms(float dt)
		{
			elapsedLifeTime += dt;

			if (elapsedLifeTime > lifeTime)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			var position = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
			var cells = GetTilesInRadius(position, radius);
			var worldIdx = this.GetMyWorldId();

			foreach (var offset in cells)
			{
				var cell = Grid.PosToCell(offset);
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

			UpgradeSingleTile(cell);

			// ground
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

			var layers = new[]
			{
			   (int)ObjectLayer.Backwall,
			   (int)ObjectLayer.Wire,
			   (int)ObjectLayer.Building,
			   (int)ObjectLayer.GasConduit,
			   (int)ObjectLayer.LiquidConduit,
			   (int)ObjectLayer.SolidConduit,
			   (int)ObjectLayer.LogicWire
			};

			foreach (var layer in layers)
			{
				if (Grid.ObjectLayers[layer].TryGetValue(cell, out var go))
				{
					if (go.TryGetComponent(out BuildingComplete _)
						&& go.TryGetComponent(out PrimaryElement primaryElement)
						&& go.TryGetComponent(out Deconstructable deconstructale))
					{
						if (primaryElement.Element.id != SimHashes.Gold)
						{
							primaryElement.SetElement(SimHashes.Gold);

							if (deconstructale.constructionElements != null)
							{
								deconstructale.constructionElements[0] = SimHashes.Gold.CreateTag();
							}

							go.GetComponent<KSelectable>().enabled = false;
							go.GetComponent<KSelectable>().enabled = true;
						}
					}
				}
			}
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

		private List<Vector2I> GetTilesInRadius(Vector3 position, float radius)
		{
			return ProcGen.Util.GetFilledCircle(position, radius);
		}
	}
}
