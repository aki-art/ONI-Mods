using ONITwitchLib.Utils;

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
			if (!force && !GridUtil.IsCellFoundationEmpty(cell))
				return false;

			var mass = Grid.Mass[cell];

			if (useMassRatio)
			{
				var elementTo = ElementLoader.FindElementByHash(elementId);

				var maxMassFrom = elementFrom.maxMass;
				var maxMassTo = elementTo.maxMass;

				mass = (mass / maxMassFrom) * maxMassTo;
				mass *= massMultiplier;
			}

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
