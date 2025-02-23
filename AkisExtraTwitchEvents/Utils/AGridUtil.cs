using ONITwitchLib.Utils;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Utils
{
	public class AGridUtil
	{
		public static CellElementEvent cellEvent = new("AETE_CellEVent", "Twitch AETE Spawn", true);

		public static HashSet<int> protectedCells = [];

		public static void OnWorldLoad()
		{
			protectedCells = [];
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
			var bounds = template.GetTemplateBounds(rootLocation, 0);

			var extents = new Extents(bounds.x, bounds.y, bounds.width, bounds.height);

			var pooledList = ListPool<ScenePartitionerEntry, ItemSucker>.Allocate();
			var movedList = new List<Transform>();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);

			foreach (var entry in pooledList)
			{
				var pickupable = (entry.obj as Pickupable);
				if (pickupable.HasAnyTags(saveTags))
				{
					pickupable.transform.SetPosition(Vector3.zero);
					movedList.Add(pickupable.transform);
				}
			}

			pooledList.Recycle();

			onCompleteCallback += () => Rescue(movedList, rootLocation + safeLocationInside);
			TemplateLoader.Stamp(template, rootLocation, onCompleteCallback);
		}

		private static void Rescue(List<Transform> movedList, Vector2 rescueLocation)
		{
			if (movedList != null)
			{
				foreach (var transform in movedList)
				{
					transform.transform.SetPosition(rescueLocation);
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
	}
}
