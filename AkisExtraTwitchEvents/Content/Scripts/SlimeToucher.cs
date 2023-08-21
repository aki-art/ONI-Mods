using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class SlimeToucher : Toucher, ISim33ms
	{
		[SerializeField] public float morbChance;
		[SerializeField] public float fungusChance;

		private BuildingDef slimeGlassTile;
		private string slimeGlassTileId = "DecorPackA_SlimeMoldStainedGlassTile";
		public PlantableSeed plantableSeed;

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
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			var seedPrefab = Assets.TryGetPrefab(MushroomPlantConfig.SEED_ID);
			if (seedPrefab != null)
				plantableSeed = seedPrefab.GetComponent<PlantableSeed>();

		}

		public override bool UpdateCell(int cell, float dt)
		{
			return TurnToSlime(cell);
		}

		private bool TurnToSlime(int cell)
		{
			var element = Grid.Element[cell];

			if (element == null)
				return false;

			//liquids
			if (element.IsLiquid &&
				!slimes.Contains(element.id)
				&& !element.HasTag(GameTags.Metal))
			{
				ReplaceElement(cell, element, Elements.PinkSlime);
			}
			else if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				ReplaceElement(cell, element, newElement);
			}

			if (TryUpdateTile(cell))
				return true;

			if (slimes.Contains(element.id))
				return true;

			// solids
			if (element.IsSolid && !slimes.Contains(element.id))
			{
				ReplaceElement(cell, element, slimes.GetRandom());
				return true;
			}
			else if (element.IsGas)
			{
				var position = Grid.CellToPos(cell);

				if (Random.value < morbChance)
					FUtility.Utils.Spawn(GlomConfig.ID, position);

				if (Random.value < fungusChance)
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

			return false;
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

		private bool TryUpdateTile(int cell)
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
				}

				return true;
			}

			return false;
		}


	}
}
