/*namespace WorldTraitsPlus.Traits.CelestialEvents
{
    public class Eclipse : WorldEvent
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (duration == -1)
                duration = ModAssets.settings.EclipseDuration.Get() * power;
        }

        public override void Begin()
        {
            base.Begin();
            if(WorldEventManager.Instance.activeEclipse != null)
            {
                WorldEventManager.Instance.activeEclipse.End();
            }

            WorldEventManager.Instance.activeEclipse = this;
        }

    }
}
*/