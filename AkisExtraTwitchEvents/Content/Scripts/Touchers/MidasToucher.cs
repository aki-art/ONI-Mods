using FUtility;
using System.Collections.Generic;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Scripts
{
	public class MidasToucher : Toucher, ISim33ms
	{
		private BuildingDef goldGlassTile;
		private string goldGlassTileId = "DecorPackA_GoldStainedGlassTile";
		private float elapsedSinceLastFloodCollect = 9999;
		private float floodCollectDelayS = 1;

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
			{ SimHashes.BrineIce, SimHashes.DirtyIce},
			{ SimHashes.Magma, SimHashes.MoltenGold},
			{ SimHashes.CrudeOil, SimHashes.Petroleum},
			{ SimHashes.ViscoGel, Elements.Honey},
			{ Elements.PinkSlime, Elements.Honey},
			{ Elements.Jello, Elements.Honey},
			{ Elements.FrozenJello, Elements.FrozenHoney},
			{ SimHashes.SolidViscoGel, Elements.FrozenHoney},
			{ SimHashes.SlimeMold, SimHashes.Isoresin},
		};

		private static readonly Dictionary<SimHashes, SimHashes> floodLookup = new()
		{
			{ SimHashes.Naphtha, SimHashes.DirtyWater},
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
					if (pickupable.HasAnyTags(MidasEntityContainer.ignoreTags))
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
					/*					else if (pickupable.TryGetComponent(out Edible edible))
										{
											if(edible.foodInfo.CaloriesPerUnit > 0 && !edible.spices.Any(spice => spice.Id == TSpices.goldFlake.Id))
											{
												edible.SpiceEdible(new SpiceInstance() { Id = TSpices.goldFlake.Id }, Db.Get().MiscStatusItems.SpicedFood);
											}
										}*/
				}
			}

			entries.Recycle();
		}

		public override bool UpdateCell(int cell, float dt)
		{
			CheckEntities(cell);

			var element = Grid.Element[cell];

			if (element == null)
				return false;

			UpdateFlood(cell, element, dt);

			// gas & liquid
			if (elementLookup.TryGetValue(element.id, out var newElement))
			{
				ReplaceElement(cell, element, newElement);
				return false;
			}

			// molten metals
			if (element.HasTag(GameTags.Metal) && element.IsLiquid && element.id != SimHashes.Mercury)
			{
				ReplaceElement(cell, element, SimHashes.Gold, false);
				return true;
			}

			if (CheckTiles(cell))
				return true;

			if (golds.Contains(element.id))
				return true;

			// solids
			if (element.IsSolid && element.id != SimHashes.Gold)
			{
				ReplaceElement(cell, element, golds.GetRandom());
				return true;
			}

			return false;
		}

		private void UpdateFlood(int cell, Element element, float dt)
		{
			elapsedSinceLastFloodCollect += dt;

			if (elapsedSinceLastFloodCollect < floodCollectDelayS)
				return;

			if (floodLookup.TryGetValue(element.id, out var result))
			{
				elapsedSinceLastFloodCollect = 0;
				var id = ElementLoader.FindElementByHash(result).idx;

				var cells = ONITwitchLib.Utils.GridUtil.FloodCollectCells(
					cell,
					c => Grid.ElementIdx[c] == id,
					128);

				foreach (var convertedCell in cells)
				{
					ReplaceElement(cell, element, result);
				}
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

					else if (go.IsPrefabID(TileConfig.ID))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn gold tile", _ => SpawnTile(cell, MetalTileConfig.ID, new[] { SimHashes.Gold.CreateTag() }, temp));
						deconstructable.ForceDestroyAndGetMaterials();
						return true;
					}

					else if (deconstructable.constructionElements != null && deconstructable.constructionElements.Length > 0)
					{
						var primary = deconstructable.constructionElements[0];
						var element = ElementLoader.GetElement(primary);

						var isMadeOfMetal = element.HasTag(GameTags.Ore) || element.HasTag(GameTags.RefinedMetal);

						if (element != null && isMadeOfMetal)
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
