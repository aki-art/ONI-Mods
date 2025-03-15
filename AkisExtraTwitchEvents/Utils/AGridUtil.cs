using ONITwitchLib.Utils;
using System.Collections;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Utils
{
	public class AGridUtil
	{
		public static CellElementEvent cellEvent = new("AETE_CellEVent", "Twitch AETE Spawn", true);

		public static readonly CellModifyMassEvent modifyEvent = new(
			"AETE_CellModifiedEvent",
			"Modified by Twitch"
		);

		public static HashSet<int> protectedCells = [];

		public static void OnWorldLoad()
		{
			protectedCells = [];
		}


		internal static bool Destroyable(int targetCell, bool evenNeutronium)
		{
			if (protectedCells.Contains(targetCell))
				return false;


			if (!evenNeutronium)
			{
				if (Grid.Element[targetCell].id == SimHashes.Unobtanium)
					return false;
			}

			var element = Grid.Element[targetCell];

			if (!evenNeutronium)
				if (element.hardness == byte.MaxValue && element.molarMass > 5000)
					return false;

			return true;
		}

		public static void Vacuum(int targetCell)
		{
			SimMessages.ReplaceElement(targetCell, SimHashes.Vacuum, cellEvent, 0);
		}


		public static bool PlaceElement(int cell, SimHashes elementId, float mass, float? temperature = null, byte diseaseIdx = byte.MaxValue, int disaseCount = 0)
		{
			if (!GridUtil.IsCellFoundationEmpty(cell))
				return false;

			SimMessages.ReplaceElement(
				cell,
				elementId,
				cellEvent,
				mass,
				temperature.GetValueOrDefault(ElementLoader.FindElementByHash(elementId).defaultValues.temperature),
				diseaseIdx,
				disaseCount);

			return true;
		}

		private static readonly Tag[] saveTags =
		[
			GameTags.DupeBrain,
			GameTags.CreatureBrain,
			GameTags.Artifact,
		];

		public static void PlaceStampSavePickupables(TemplateContainer template, Vector2 rootLocation, Vector2 safeLocationInside, System.Action onCompleteCallback)
		{
			var originCell = Grid.PosToCell(rootLocation);

			// clear tiles
			foreach (var cell in template.cells)
			{
				var buildingCell = Grid.OffsetCell(originCell, cell.location_x, cell.location_y);
				TileUtil.ClearTile(buildingCell);

				if (ElementLoader.FindElementByHash(cell.element).IsSolid)
					TileUtil.ClearBuildings(buildingCell, ObjectLayer.Building, (building, c) =>
					{
						return building.TryGetComponent(out Door _)
						|| building.TryGetComponent(out FakeFloorAdder _)
						|| building.TryGetComponent(out MakeBaseSolid _);
					});

				SimMessages.Dig(buildingCell);
			}


			AkisTwitchEvents.Instance.StartCoroutine(StampNextFrame(template, rootLocation, safeLocationInside, onCompleteCallback));
		}

		private static IEnumerator StampNextFrame(TemplateContainer template, Vector2 rootLocation, Vector2 safeLocationInside, System.Action onCompleteCallback)
		{
			yield return SequenceUtil.waitForEndOfFrame;

			// clear pickupables
			var bounds = template.GetTemplateBounds(rootLocation, 0);

			var extents = new Extents(bounds.x, bounds.y, bounds.width, bounds.height);

			var pooledList = ListPool<ScenePartitionerEntry, ItemSucker>.Allocate();
			var movedList = new List<Transform>();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);

			foreach (var entry in pooledList)
			{
				var pickupable = (entry.obj as Pickupable);
				if (!pickupable.HasTag(GameTags.Stored) && (pickupable.HasAnyTags(saveTags) || pickupable.handleFallerComponents))
				{
					pickupable.transform.SetPosition(Vector3.zero with { z = pickupable.transform.position.z });
					movedList.Add(pickupable.transform);
				}
			}

			pooledList.Recycle();

			onCompleteCallback += () => Rescue(movedList, rootLocation + safeLocationInside);
			TemplateLoader.Stamp(template, rootLocation, onCompleteCallback);
		}

		private static void Rescue(List<Transform> movedList, Vector3 rescueLocation)
		{
			if (movedList != null)
			{
				foreach (var transform in movedList)
				{
					if (!transform.IsNullOrDestroyed()) // a frame passed things could be gone
					{
						var z = transform.transform.position.z;
						transform.transform.SetPosition(rescueLocation with { z = z });
					}
				}
			}
		}

		public static bool ReplaceElement(int cell, Element elementFrom, SimHashes elementId, bool useMassRatio = true, float massMultiplier = 1f, bool force = false, float? tempOverride = null)
		{
			if (elementFrom == null)
				return false;

			if (!force && !GridUtil.IsCellFoundationEmpty(cell))
				return false;

			var mass = Grid.Mass[cell];

			if (Grid.HasDoor[cell])
				return false;

			if (mass <= 0)
				return false;

			if (useMassRatio)
			{
				var elementTo = ElementLoader.FindElementByHash(elementId);

				if (elementTo == null)
					return false;

				var maxMassFrom = elementFrom.maxMass;
				var maxMassTo = elementTo.maxMass;

				mass = (mass / maxMassFrom) * maxMassTo;
				mass *= massMultiplier;
				mass = Mathf.Clamp(mass, 0.0001f, 9999f);
			}

			var temp = tempOverride.GetValueOrDefault(Grid.Temperature[cell]);
			if (temp == 0)
				return false;

			SimMessages.ReplaceElement(
				cell,
				elementId,
				cellEvent,
				mass,
				tempOverride.GetValueOrDefault(Grid.Temperature[cell]),
				Grid.DiseaseIdx[cell],
				Grid.DiseaseCount[cell]);

			return true;
		}

		public static bool DestroyTile(int cell)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].ContainsKey(cell))
			{
				WorldDamage.Instance.ApplyDamage(cell, 999, -1);
				return true;
			}

			return false;
		}
	}
}
