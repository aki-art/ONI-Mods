using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Buildings;
using UnityEngine;

namespace Twitchery.Content.Scripts.Touchers
{
	public class ForestToucher : Toucher
	{
		private static readonly HashSet<SimHashes> earths =
		[
			SimHashes.Dirt,
			SimHashes.Algae,
			(SimHashes)Hash.SDBMLower("Beached_Moss")
		];

		private PlantableSeed arborTreeSeed;
		private PlantableSeed buddySeed;
		private PlantableSeed oxyFernSeed;

		private List<BuildingDef> stainedGlassDefs;
		private List<string> glassTileIds =
		[
			$"DecorPackA_{SimHashes.WoodLog}StainedGlassTile",
			$"DecorPackA_{SimHashes.Dirt}StainedGlassTile",
		];

		private float arborTreeChance = 0.1f;
		private float buddyChance = 0.1f;
		private float oxyFernChance = 0.1f;
		private float pipChance = 0.005f;
		private float bigTreeChance = 0.002f;

		private static HashSet<int> wallsPlaced;

		protected override GameObject CreateMarker()
		{
			var result = Instantiate(ModAssets.Prefabs.forestToucher);

			return result;
		}

		public override void SpawnFeedbackAnimation(int cell)
		{
			//Game.Instance.SpawnFX(ModAssets.Fx.slimeSplat, cell, Random.Range(0, 360));
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			stainedGlassDefs = [.. glassTileIds.Select(Assets.GetBuildingDef)];

			arborTreeSeed = Assets.GetPrefab(ForestTreeConfig.SEED_ID).GetComponent<PlantableSeed>();
			buddySeed = Assets.GetPrefab(BulbPlantConfig.SEED_ID).GetComponent<PlantableSeed>();
			oxyFernSeed = Assets.GetPrefab(OxyfernConfig.SEED_ID).GetComponent<PlantableSeed>();

			earths.RemoveWhere(e => ElementLoader.FindElementByHash(e) == null);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			wallsPlaced = [];
		}

		public override bool UpdateCell(int cell, float dt)
		{
			var existingElement = Grid.Element[cell];
			bool hasChangedSolid = false;

			if (GridUtil.IsCellEmpty(cell))
			{
				if (!wallsPlaced.Contains(cell))
				{
					var skip = false;

					if (Grid.ObjectLayers[(int)ObjectLayer.Backwall].TryGetValue(cell, out var existingWall))
						if (existingWall.TryGetComponent(out Deconstructable deconstructable))
						{
							var roll = Random.value < 0.05f;
							if (roll)
								deconstructable.ForceDestroyAndGetMaterials();
							else
								skip = true;
						}

					if (!skip)
					{
						Assets.GetBuildingDef(LeafWallConfig.ID).Build(cell, Orientation.Neutral, null, [SimHashes.WoodLog.CreateTag()], 300);
					}

					wallsPlaced.Add(cell);
				}

				if (!existingElement.IsLiquid)
				{
					ReplaceElement(cell, existingElement, SimHashes.Oxygen, tempOverride: 300f);

					if (Random.value < pipChance)
					{
						FUtility.Utils.Spawn(SquirrelConfig.ID, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
					}

					if (Random.value < arborTreeChance && arborTreeSeed.TestSuitableGround(cell))
					{
						var tree = FUtility.Utils.Spawn(ForestTreeConfig.ID, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
						tree.GetComponent<Growing>().OverrideMaturityLevel(Random.Range(0.3f, 1f));
						return true;
					}

					if (Random.value < buddyChance && buddySeed.TestSuitableGround(cell))
					{
						FUtility.Utils.Spawn(BulbPlantConfig.ID, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
						return true;
					}

					if (Random.value < oxyFernChance && oxyFernSeed.TestSuitableGround(cell))
					{
						FUtility.Utils.Spawn(OxyfernConfig.ID, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
						return true;
					}

					if (Random.value < bigTreeChance)
					{
						var branch = FUtility.Utils.Spawn(SmallBranchWalkerConfig.ID, Grid.CellToPos(cell));
						branch.GetComponent<BranchWalker>().Generate();
					}
				}
				else
				{
					if (ReplaceElement(cell, existingElement, SimHashes.Water, tempOverride: 300f))
						return true;
				}
			}
			else
			{
				if (CheckTiles(cell))
					return true;

				if (ReplaceElement(cell, existingElement, earths.GetRandom(), tempOverride: 300f))
					return true;
			}

			return hasChangedSolid;
		}


		private bool CheckTiles(int cell)
		{
			if (Grid.HasDoor[cell])
				return true;

			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (glassTileIds.Contains(go.PrefabID().ToString()))
						return true;

					if (go.HasTag("DecorPackA_StainedGlass"))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						var roll = Random.Range(0, stainedGlassDefs.Count);

						GameScheduler.Instance.ScheduleNextFrame("spawn wood tile", _ => SpawnTile(cell, glassTileIds[roll], [.. stainedGlassDefs[roll].DefaultElements()], temp));
						deconstructable.ForceDestroyAndGetMaterials();

						return true;
					}

					else if (go.IsPrefabID(TileConfig.ID))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn wood tile", _ => SimMessages.ReplaceAndDisplaceElement(cell, Elements.FakeLumber, toucherEvent, 200f, 300));
						deconstructable.ForceDestroyAndGetMaterials();

						return true;
					}
				}

				return true;
			}

			return false;
		}
	}
}
