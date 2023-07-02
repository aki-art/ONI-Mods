using FUtility;
using System.Collections.Generic;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Scripts
{
	public class MidasToucher : Toucher, ISim33ms
	{
		private BuildingDef goldGlassTile;
		private string goldGlassTileId = "DecorPackA_GoldStainedGlassTile";

		private static readonly HashSet<SimHashes> golds = new()
		{
			SimHashes.Gold,
			SimHashes.GoldAmalgam,
			SimHashes.FoolsGold
		};

		private static readonly Dictionary<SimHashes, SimHashes> elementLookup = new()
		{
			{ SimHashes.Water, SimHashes.DirtyWater},
			{ SimHashes.Oxygen, SimHashes.ContaminatedOxygen},
			{ SimHashes.SaltWater, SimHashes.DirtyWater},
			{ SimHashes.Ice, SimHashes.DirtyIce},
			{ SimHashes.Magma, SimHashes.MoltenGold},
			{ SimHashes.CrudeOil, SimHashes.Petroleum},
			{ SimHashes.ViscoGel, Elements.Honey},
			{ Elements.PinkSlime, Elements.Honey},
			{ Elements.Jello, Elements.Honey},
			{ Elements.FrozenJello, Elements.FrozenHoney},
			{ SimHashes.SlimeMold, SimHashes.Isoresin},
		};

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			goldGlassTile = Assets.GetBuildingDef("DecorPackA_GoldStainedGlassTile");
		}

		private void CheckEntities(int cell)
		{
			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				Extents.OneCell(cell),
				GameScenePartitioner.Instance.pickupablesLayer,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is Pickupable pickupable)
				{
					if (pickupable.HasTag(TTags.midasSafe) || pickupable.HasTag(GameTags.Stored))
						continue;

					if (pickupable.TryGetComponent(out MinionIdentity minionIdentity))
					{
						var midasContainer = FUtility.Utils.Spawn(MidasEntityContainterConfig.ID, pickupable.gameObject);
						midasContainer.GetComponent<MidasEntityContainer>().StoreMinion(minionIdentity, ModTuning.MIDAS_TOUCH_EFFECT_DURATION);
					}
					else if (pickupable.HasTag(GameTags.CreatureBrain))
					{
						var midasContainer = FUtility.Utils.Spawn(MidasEntityContainterConfig.ID, pickupable.gameObject);
						midasContainer.GetComponent<MidasEntityContainer>().StoreCritter(pickupable.gameObject, ModTuning.MIDAS_TOUCH_EFFECT_DURATION);
					}
					else if (pickupable.TryGetComponent(out ElementChunk _)
						&& pickupable.TryGetComponent(out PrimaryElement originalPrimaryElement)
						&& !golds.Contains(originalPrimaryElement.ElementID))
					{
						var gold = FUtility.Utils.Spawn(golds.GetRandom().CreateTag(), pickupable.transform.position);
						gold.TryGetComponent(out PrimaryElement primaryElement);
						primaryElement.SetMassTemperature(originalPrimaryElement.Mass, originalPrimaryElement.Temperature);
						primaryElement.AddDisease(originalPrimaryElement.DiseaseIdx, originalPrimaryElement.diseaseCount, "copy");

						Util.KDestroyGameObject(pickupable);
					}
				}
			}

			entries.Recycle();
		}

		public override void UpdateCell(int cell)
		{
			TurnToGold(cell);
		}

		private void TurnToGold(int cell)
		{
			CheckEntities(cell);

			var element = Grid.Element[cell];

			if (element == null)
				return;

			// gas & liquid
			if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				ReplaceElement(cell, element, newElement);
				return;
			}

			// molten metals
			if (element.HasTag(GameTags.Metal) && element.IsLiquid && element.id != SimHashes.Mercury)
			{
				ReplaceElement(cell, element, SimHashes.Gold, false);
				return;
			}

			if (CheckTiles(cell))
				return;

			if (golds.Contains(element.id))
				return;

			// solids
			if (element.IsSolid && element.id != SimHashes.Gold)
				ReplaceElement(cell, element, golds.GetRandom());
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
	}
}
