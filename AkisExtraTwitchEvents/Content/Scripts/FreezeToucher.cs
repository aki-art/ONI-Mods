using System.Collections.Generic;
using Twitchery.Content.Defs;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class FreezeToucher : Toucher
	{
		[SerializeField] public float temperatureShift;
		private BuildingDef iceGlassTile;
		private const string ICE_GLASSTILE_ID = "DecorPackA_IceStainedGlassTile";

		private static readonly CellElementEvent cellEvent = new("AETE_FreezeToucher", "freeze gun", false);

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			iceGlassTile = Assets.GetBuildingDef(ICE_GLASSTILE_ID);
		}

		public override bool UpdateCell(int cell, float dt)
		{
			if (!Grid.IsValidCell(cell)) return false;

			CheckEntities(cell);
			CheckTiles(cell);

			var temp = Grid.Temperature[cell];
			temp += temperatureShift;
			temp = Mathf.Clamp(temp, 0, 9999);

			SimMessages.ReplaceElement(
				cell,
				Grid.Element[cell].id,
				cellEvent,
				Grid.Mass[cell],
				temp,
				Grid.DiseaseIdx[cell],
				Grid.DiseaseCount[cell]);

			return false;
		}

		private bool CheckTiles(int cell)
		{
			if (iceGlassTile == null)
				return false;

			if (Grid.HasDoor[cell])
				return true;

			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (go.PrefabID() == ICE_GLASSTILE_ID)
						return true;

					if (go.HasTag("DecorPackA_StainedGlass") || go.IsPrefabID(TileConfig.ID) || go.IsPrefabID(GlassTileConfig.ID))
					{
						var temp = go.GetComponent<PrimaryElement>().Temperature;

						GameScheduler.Instance.ScheduleNextFrame("spawn ice tile", _ => SpawnTile(cell, ICE_GLASSTILE_ID, iceGlassTile.DefaultElements().ToArray(), temp));
						deconstructable.ForceDestroyAndGetMaterials();
						return true;
					}
				}

				return true;
			}

			return false;
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
						var midasContainer = FUtility.Utils.Spawn(FrozenEntityContainerConfig.ID, pickupable.gameObject);
						midasContainer.GetComponent<FrozenEntityContainer>().StoreMinion(minionIdentity, float.PositiveInfinity);
					}
					else if (pickupable.HasTag(GameTags.CreatureBrain))
					{
						var midasContainer = FUtility.Utils.Spawn(FrozenEntityContainerConfig.ID, pickupable.gameObject);
						midasContainer.GetComponent<MidasEntityContainer>().StoreCritter(pickupable.gameObject, float.PositiveInfinity);
					}
				}
			}

			entries.Recycle();
		}
	}
}
