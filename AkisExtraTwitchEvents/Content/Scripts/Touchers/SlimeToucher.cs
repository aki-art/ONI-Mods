using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class SlimeToucher : Toucher, ISim33ms, IRenderEveryTick
	{
		[SerializeField] public float morbChance;
		[SerializeField] public float fungusChance;
		[SerializeField] public float slimeBlockChance;

		private BuildingDef slimeGlassTile;
		private string slimeGlassTileId = "DecorPackA_SlimeMoldStainedGlassTile";
		public PlantableSeed plantableSeed;
		private float elapsedTime;
		private float elapsedSinceLastSound = 999;

		private Material material;

		private const float FX_FADE_TIME = 3f;
		private const float AMPLITUDE = 0.01f;
		private const float SOUND_FX_COOLDOWN = 0.1f;
		private const string WAVE_AMPLITUDE = "_WaveAmplitude";

		private static readonly HashSet<SimHashes> slimes = new()
		{
			SimHashes.SlimeMold
		};

		private static readonly HashSet<SimHashes> liquidSlimes = new()
		{
			Elements.PinkSlime
		};

		private static readonly Dictionary<SimHashes, SimHashes> elementLookup = new()
		{
		};

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			slimeGlassTile = Assets.GetBuildingDef(slimeGlassTileId);
			if (Mod.isBeachedHere)
			{
				liquidSlimes.Add((SimHashes)Hash.SDBMLower("Mucus"));
			}
		}

		protected override GameObject CreateMarker()
		{
			var result = Instantiate(ModAssets.Prefabs.slimyPulse);
			material = result.transform.Find("Quad").GetComponent<MeshRenderer>().material;

			return result;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			var seedPrefab = Assets.TryGetPrefab(MushroomPlantConfig.SEED_ID);
			if (seedPrefab != null)
				plantableSeed = seedPrefab.GetComponent<PlantableSeed>();

			elapsedSinceLastSound = 999;
		}

		public override bool UpdateCell(int cell, float dt)
		{
			elapsedSinceLastSound += dt;
			if (elapsedTime < FX_FADE_TIME)
				return false;

			return TurnToSlime(cell);
		}

		public override void SpawnFeedbackAnimation(int cell)
		{
			Game.Instance.SpawnFX(ModAssets.Fx.slimeSplat, cell, Random.Range(0, 360));

			if (elapsedSinceLastSound > SOUND_FX_COOLDOWN)
			{
				AudioUtil.PlaySound(
					ModAssets.Sounds.SPLAT,
					Grid.CellToPos(cell),
					Random.Range(0.2f, 0.3f) * ModAssets.GetSFXVolume(),
					Random.Range(0.5f, 1.5f));

				elapsedSinceLastSound = 0;
			}
		}

		private bool TurnToSlime(int cell)
		{
			var element = Grid.Element[cell];

			if (element == null)
				return false;

			if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				ReplaceElement(cell, element, newElement);
			}
			//liquids
			else if (element.IsLiquid &&
				!slimes.Contains(element.id)
				&& !element.HasTag(GameTags.Metal))
			{
				ReplaceElement(cell, element, liquidSlimes.GetRandom());
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

				else if (Random.value < fungusChance)
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

				else if (Random.value < slimeBlockChance)
				{
					SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.SlimeMold, toucherEvent, 200);
					SpawnFeedbackAnimation(cell);
				}
			}

			return false;
		}

		public void PlantAndGrow(PlantableSeed seed, Vector3 position)
		{
			position += new Vector3(0.5f, 0);
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
			if (!Grid.IsValidCell(cell) || slimeGlassTile == null)
				return false;

			if (Grid.HasDoor[cell])
				return true;

			var tiles = Grid.ObjectLayers[(int)ObjectLayer.FoundationTile];

			if (tiles == null)
				return false;

			if (tiles.TryGetValue(cell, out var go))
			{
				if (go == null)
					return false;

				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (go.PrefabID() == slimeGlassTileId)
						return true;

					if (go.HasTag("DecorPackA_StainedGlass"))
					{
						var temp = Grid.Temperature[cell];

						GameScheduler.Instance.ScheduleNextFrame("spawn slime tile", _ => SpawnTile(cell, slimeGlassTileId, slimeGlassTile.DefaultElements().ToArray(), temp));
						deconstructable.ForceDestroyAndGetMaterials();

						SpawnFeedbackAnimation(cell);
						return true;
					}
				}

				return true;
			}

			return false;
		}

		public void RenderEveryTick(float dt)
		{
			elapsedTime += dt;

			if (elapsedTime <= FX_FADE_TIME)
			{
				var t = elapsedTime / FX_FADE_TIME;
				material.SetFloat(WAVE_AMPLITUDE, Mathf.Lerp(0, AMPLITUDE, t));
				return;
			}

			var remaining = lifeTime - elapsedTime;
			if (remaining <= FX_FADE_TIME)
			{
				var t = remaining / FX_FADE_TIME;
				material.SetFloat(WAVE_AMPLITUDE, Mathf.Lerp(0, AMPLITUDE, t));
			}
		}
	}
}
