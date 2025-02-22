using ONITwitchLib.Utils;
using UnityEngine;

namespace Twitchery.Utils
{
	public class AGridUtil
	{
		private static CellElementEvent cellEvent = new("AETE_CellEVent", "Twitch AETE Spawn", true);

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
