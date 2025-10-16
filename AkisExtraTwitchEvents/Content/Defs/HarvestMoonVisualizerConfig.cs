using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class HarvestMoonVisualizerConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_HarvestMoonVisualizer";

		override public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Harvest Moon", false);

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("aete_harvest_moon_kanim")];
			kbac.isMovable = true;
			kbac.initialAnim = "idle";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

			prefab.AddComponent<HarvestMoonVisualizer>();

			return prefab;
		}
	}
}
