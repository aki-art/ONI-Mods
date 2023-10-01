using FUtility;
using SpookyPumpkinSO.Content.Buildings;

namespace SpookyPumpkinSO.Content
{
	public class SPFriendlyPumpkinFaces : ResourceSet<FriendlyPumpkinFace>
	{
		public SPFriendlyPumpkinFaces()
		{
			for (int i = 0; i < 8; i++)
			{
				if (Assets.TryGetAnim($"spookypumpkin_friendlylamp_{i}_kanim", out var anim))
				{
					var id = $"FriendlyPumpkinFace_{i}";
					var name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{FriendlyPumpkinConfig.ID.ToUpperInvariant()}.VARIANTS.{id.ToUpperInvariant()}");
					Add(new FriendlyPumpkinFace(id, name, i, anim));
				}
				else Log.Warning($"No animation with name spookypumpkin_friendlylamp_{i}");
			}
		}

		public string GetIdForIndex(int currentFace)
		{
			foreach (var face in resources)
			{
				if (face.numericId == currentFace)
					return face.Id;
			}

			return null;
		}
	}
}
