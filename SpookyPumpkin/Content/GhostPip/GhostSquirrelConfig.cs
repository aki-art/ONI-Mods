using Klei.AI;
using TUNING;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.CREATURES.SPECIES.SP_GHOSTPIP;

namespace SpookyPumpkinSO.Content.GhostPip
{
	public class GhostSquirrelConfig : IEntityConfig
	{
		public const string ID = "SP_GhostSquirrel";
		public const string BASE_TRAIT_ID = "SP_GhostSquirrelBaseTrait";

		public GameObject CreatePrefab()
		{
			var placedEntity = EntityTemplates.CreatePlacedEntity(
				ID,
				NAME,
				DESC,
				1f,
				Assets.GetAnim("sp_ghostpip_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				1,
				DECOR.BONUS.TIER1,
				NOISE_POLLUTION.NONE);

			var trait = Db.Get().CreateTrait(BASE_TRAIT_ID, NAME, DESC, null, true, null, true, true);

			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, float.PositiveInfinity));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, float.PositiveInfinity));

			placedEntity.AddTag(GameTags.Creatures.Walker);

			if (Mod.Config.GhostPipLight)
			{
				var light2d = placedEntity.AddComponent<Light2D>();
				light2d.overlayColour = TUNING.LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
				light2d.Color = Color.green;
				light2d.Range = 1f;
				light2d.shape = LightShape.Circle;
				light2d.Offset = new Vector2(0, 0.3f);
				light2d.drawOverlay = true;
				light2d.Lux = 300;
			}

			EntityTemplates.ExtendEntityToBasicCreature(
				placedEntity,
				FactionManager.FactionID.Friendly,
				BASE_TRAIT_ID,
				onDeathDropID: "",
				onDeathDropCount: 0,
				warningLowTemperature: 0,
				warningHighTemperature: 9999,
				lethalLowTemperature: 0,
				lethalHighTemperature: 9999);

			placedEntity.GetComponent<PrimaryElement>().Temperature = GameUtil.GetTemperatureConvertedToKelvin(24, GameUtil.TemperatureUnit.Celsius);

			placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
			placedEntity.AddOrGet<Trappable>();
			placedEntity.AddOrGet<LoopingSounds>().updatePosition = true;
			placedEntity.AddOrGet<UserNameable>();
			placedEntity.AddOrGet<CharacterOverlay>().shouldShowName = true;

			var storage = placedEntity.AddComponent<Storage>();
			storage.showInUI = false;

			var manualDeliveryKg = placedEntity.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg.SetStorage(storage);
			manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
			manualDeliveryKg.RequestedItemTag = GrilledPrickleFruitConfig.ID;
			manualDeliveryKg.refillMass = 1f;
			manualDeliveryKg.MinimumMass = 1f;
			manualDeliveryKg.capacity = 1f;

			ConfigureBrain(placedEntity);

			placedEntity.AddComponent<SeedTrader>();
			placedEntity.AddComponent<GhostSquirrel>();

			EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);

			return placedEntity;
		}

		private static void ConfigureBrain(GameObject placedEntity)
		{
			var choreTable = new ChoreTable.Builder()
				.Add(new FallStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(
				placedEntity,
				choreTable,
				GameTags.Creatures.Species.SquirrelSpecies,
				null);
		}

		string[] IEntityConfig.GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
