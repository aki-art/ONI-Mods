using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class FrozenEntityContainerConfig : IEntityConfig
	{
		public const string ID = "AkisTwitchEvents_FrozenEntityContainer";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.MISC.MIDAS_STATE,
				"",
				100f,
				Assets.GetAnim("barbeque_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				1,
				1,
				TUNING.DECOR.BONUS.TIER2,
				element: SimHashes.Ice,
				defaultTemperature: GameUtil.GetTemperatureConvertedToKelvin(-6, GameUtil.TemperatureUnit.Celsius));

			prefab.AddComponent<MinionStorage>();
			prefab.AddComponent<FrozenEntityContainer>();

			var fx = prefab.AddComponent<HighlightFx>();
			fx.goalTintColor = Color.blue;
			fx.duration = 3f;

			prefab.AddComponent<Storage>();
			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
