using UnityEngine;

namespace SmartDeconstruct.Content
{
	public class SmartDeconstructMarkerConfig : CommonPlacerConfig
	{
		public const string ID = "SmartDeconstruct_Marker";

		public GameObject CreatePrefab()
		{
			var prefab = CreatePrefab(ID, "Smart Deconstruction", Assets.instance.mopPlacerAssets.material);
			prefab.AddTag(GameTags.NotConversationTopic);

			prefab.AddOrGet<SD_DeconstructionExtractionPoint>();
			prefab.AddOrGet<Cancellable>();

			return prefab;
		}
	}
}
