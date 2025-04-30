using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
	class MacaroniConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.Macaroni;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID, [GameTags.Edible]);
			JelloConfig.ExtendEntityToFood(prefab, TFoodInfos.macaroni);

			return prefab;
		}
	}
}
