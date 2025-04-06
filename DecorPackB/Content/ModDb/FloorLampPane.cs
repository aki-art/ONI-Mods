using UnityEngine;

namespace DecorPackB.Content.ModDb
{
	public class FloorLampPane(string id, string name, string animFile, Color lightColor) : Resource(id, name)
	{
		public string animFile = animFile;
		public Color lightColor = lightColor;

		public static FloorLampPane FromElement(SimHashes element, Color lightColor) => FromElement(element.ToString(), lightColor);

		public static string GetIdFromElement(string elementName) => $"floorlamp_{elementName.ToLowerInvariant()}";

		public static FloorLampPane FromElement(string element, Color lightColor)
		{
			return new FloorLampPane(
				GetIdFromElement(element),
				$"{element} pane",
				$"dpii_floorlamppane_{element.ToLowerInvariant()}_kanim",
				lightColor);
		}
	}
}
