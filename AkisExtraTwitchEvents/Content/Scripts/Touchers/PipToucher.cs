using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Debris;
using UnityEngine;

namespace Twitchery.Content.Scripts.Touchers
{
	public class PipToucher : Toucher
	{
		private PlantableSeed arborTreeSeed;
		/*
				private List<BuildingDef> stainedGlassDefs;
				private List<string> glassTileIds =
				[
					$"DecorPackA_{SimHashes.WoodLog}StainedGlassTile"
				];*/

		private float arborTreeChance = 0.1f;
		private float pipChance = 0.005f;

		public override void SpawnFeedbackAnimation(int cell)
		{
			Game.Instance.SpawnFX(ModAssets.Fx.pinkPoof, cell, Random.Range(0, 360));
			AudioUtil.PlaySound(ModAssets.Sounds.PIP, ModAssets.GetSFXVolume() * Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f));
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			//stainedGlassDefs = [.. glassTileIds.Select(Assets.GetBuildingDef)];

			arborTreeSeed = Assets.GetPrefab(ForestTreeConfig.SEED_ID).GetComponent<PlantableSeed>();
		}

		public override bool UpdateCell(int cell, float dt)
		{
			var existingElement = Grid.Element[cell];
			var hasChangedSolid = false;

			if (GridUtil.IsCellEmpty(cell))
			{
				if (!existingElement.IsLiquid)
				{
					if (Random.value < pipChance)
					{
						var prefabId = PipiumConfig.options.GetWeightedRandom().id;
						FUtility.Utils.Spawn(prefabId, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
					}

					if (Random.value < arborTreeChance && arborTreeSeed.TestSuitableGround(cell))
					{
						var tree = FUtility.Utils.Spawn(ForestTreeConfig.ID, Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures));
						tree.GetComponent<Growing>().OverrideMaturityLevel(Random.Range(0.3f, 1f));
						return true;
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

				if (ReplaceElement(cell, existingElement, Elements.Pipium, tempOverride: 300f))
					return true;
			}

			CheckEntities(cell);

			return hasChangedSolid;
		}

		private void CheckEntities(int cell)
		{
			var entries = ListPool<ScenePartitionerEntry, PipToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				Extents.OneCell(cell),
				GameScenePartitioner.Instance.pickupablesLayer,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is Pickupable pickupable)
				{
					if (pickupable.HasAnyTags(MidasEntityContainer.ignoreTags))
						continue;

					if (pickupable.TryGetComponent(out MinionIdentity minionIdentity))
					{
						if (minionIdentity.model == GameTags.Minions.Models.Standard)
						{
							Polymorph(minionIdentity, minionIdentity.GetProperName());
						}
					}
					else if (pickupable.HasTag(GameTags.CreatureBrain) && pickupable.TryGetComponent(out CreatureBrain _))
					{
						FUtility.Utils.Spawn(PipiumConfig.options.GetRandom().id, pickupable.gameObject);
						Util.KDestroyGameObject(pickupable.gameObject);
					}
				}
			}
		}

		public static void Polymorph(MinionIdentity identity, string minionName)
		{
#if POLYMORPH
			var creaturePrefabId = PolymorphFloorCritterConfig.ID;

			var critter = FUtility.Utils.Spawn(creaturePrefabId, identity.transform.position);
			var morph = TDb.polymorphs.Get(TPolymorphs.PIP);

			critter.GetComponent<AETE_PolymorphCritter>().SetMorph(identity, morph);
#endif
		}


		private bool CheckTiles(int cell)
		{
			if (Grid.HasDoor[cell])
				return true;

			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					/*					if (glassTileIds.Contains(go.PrefabID().ToString()))
											return true;

										if (go.HasTag("DecorPackA_StainedGlass"))
										{
											var temp = go.GetComponent<PrimaryElement>().Temperature;

											var roll = Random.Range(0, stainedGlassDefs.Count);

											GameScheduler.Instance.ScheduleNextFrame("spawn wood tile", _ => SpawnTile(cell, glassTileIds[roll], [.. stainedGlassDefs[roll].DefaultElements()], temp));
											deconstructable.ForceDestroyAndGetMaterials();

											return true;
										}

										else */
					if (go.IsPrefabID(TileConfig.ID))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn wood tile", _ => SimMessages.ReplaceAndDisplaceElement(cell, Elements.Pipium, toucherEvent, 100f, 300));
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
