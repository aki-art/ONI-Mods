using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class GenericEggCometConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_GenericEggComet";

		public override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Egg", true);

			prefab.AddOrGet<SaveLoadRoot>();
			prefab.AddOrGet<LoopingSounds>();

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("egg_hatch_kanim")];
			kbac.isMovable = true;
			kbac.initialAnim = "idle";
			kbac.initialMode = KAnim.PlayMode.Loop;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

			var falling = prefab.AddOrGet<EggComet>();
			falling.addTiles = 1;
			falling.tempMinC = 25f;
			falling.tempMaxC = 32f;
			falling.massMin = 0.5f;
			falling.massMax = 0.5f;
			falling.speedMin = 2.5f;
			falling.speedMax = 3.75f;
			falling.spawnAngleMin = 55f;
			falling.spawnAngleMax = 60f;
			falling.destroyOnExplode = true;

			var primaryElement = prefab.AddOrGet<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Creature);

			var collider = prefab.AddOrGet<KCircleCollider2D>();
			collider.radius = 0.5f;

			return prefab;
		}
	}
}
