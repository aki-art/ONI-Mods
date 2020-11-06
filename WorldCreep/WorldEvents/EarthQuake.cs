namespace WorldCreep.WorldEvents
{
    public class EarthQuake : WorldEvent
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            Debug.Log("POWER IS: " + power);
            power = 88f;
        }
        public override void Begin()
        {
            Debug.Log("Rumble rumble");
        }

        public override void End()
        {
            //throw new System.NotImplementedException();
        }

        protected override void SetPower()
        {
           // throw new System.NotImplementedException();
        }
    }
}
