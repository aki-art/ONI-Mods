/*using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_GoldFlakeable : KMonoBehaviour
	{
		[MyCmpReq] private Edible edible;

		private bool applied;

		private static readonly Dictionary<string, EdiblesManager.FoodInfo> infos = new();

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (edible.spices != null && edible.spices.Any(spice => spice.Id == TSpices.goldFlake.Id))
				Foil();
		}

		public void Foil()
		{
			if (applied)
				return;

			edible.GetComponent<KBatchedAnimController>().TintColour = Color.yellow;
			applied = true;

			ApplyQuality();
		}

		private void ApplyQuality()
		{
			if (edible.foodInfo.Quality >= 5)
				return;

			if (infos.TryGetValue(edible.foodInfo.Id, out var foodInfo))
				edible.FoodInfo = foodInfo;

			else
			{
				var info = new EdiblesManager.FoodInfo(
					edible.foodInfo.Id + "_AETEGilded",
					edible.foodInfo.DlcId,
					edible.foodInfo.CaloriesPerUnit,
					edible.foodInfo.Quality + 1,
					edible.foodInfo.PreserveTemperature,
					edible.foodInfo.RotTemperature,
					edible.foodInfo.SpoilTime,
					edible.foodInfo.CanRot);

				edible.FoodInfo = info;
				infos[edible.foodInfo.Id] = info;
			}
		}
	}
}
*/