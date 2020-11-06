using KSerialization;

namespace WorldCreep.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EarthQuake : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public string test;

        protected override void OnSpawn()
        {
            test = test == null ? "Null" : "Saved";
            Debug.Log("TEST IS: " + test);
        }
    }
}
