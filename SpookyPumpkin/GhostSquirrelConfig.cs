using Klei.AI;
using UnityEngine;

namespace SpookyPumpkin
{
    class GhostSquirrelConfig : IEntityConfig
    {
        public const string ID = "SP_GhostSquirrel";
        public const string BASE_TRAIT_ID = "SP_GhostSquirrelBaseTrait";

        public GameObject CreatePrefab()
        {
            GameObject placedEntity = EntityTemplates.CreatePlacedEntity(
                id: ID,
                name: "name",
                desc: "desc",
                mass: 0,
                anim: Assets.GetAnim("sp_ghostpip_kanim"),
                initialAnim: "idle_loop",
                sceneLayer: Grid.SceneLayer.Creatures,
                width: 1,
                height: 1,
                decor: TUNING.DECOR.BONUS.TIER0,
                noise: new EffectorValues());

            Db.Get()
                .CreateTrait(BASE_TRAIT_ID, "name", "desc", null, false, null, true, true)
                .Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, "name"));

            placedEntity.AddTag(GameTags.Creatures.Walker);

            Light2D light2d = placedEntity.AddComponent<Light2D>();
            light2d.overlayColour = TUNING.LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
            light2d.Color = Color.green;
            light2d.Range = 1f;
            light2d.shape = LightShape.Circle;
            light2d.Offset = new Vector2(0, 1f);
            light2d.drawOverlay = true;
            light2d.Lux = 400;

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

            //placedEntity.AddOrGetDef<ThreatMonitor.Def>();
            placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();

            //placedEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
            placedEntity.AddOrGet<LoopingSounds>().updatePosition = true; 
            placedEntity.AddComponent<Storage>();

            EntityTemplates.AddCreatureBrain(placedEntity,
                new ChoreTable.Builder()
                //.Add(new DeathStates.Def())
                //.Add(new TrappedStates.Def())
                .Add(new FallStates.Def())
                //.Add(new StunnedStates.Def())
                .Add(new DebugGoToStates.Def())
                //.Add(new FleeStates.Def())
                .Add(new IdleStates.Def()),
                GameTags.Creatures.Species.SquirrelSpecies,
                null);

            placedEntity.AddComponent<SeedTrader>();
           // placedEntity.AddComponent<GhostSquirrel>();

            return placedEntity;
        }

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
