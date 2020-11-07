using KSerialization;
using System.Runtime.Serialization;

namespace WorldCreep.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class EarthQuake : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        public string test;

        public EarthQuake()
        {
            Debug.Log("contructor");
            //test = null;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            test = test.IsNullOrWhiteSpace() ? "WasNull" : "Serialized";
            Debug.Log("OnSpawn");
            Debug.Log("test: " + test);
        }

        [OnSerializing]
        internal void OnSerializing()
        {
            Debug.Log("OnSerializing");
            Debug.Log("test: " + test);
        }

        [OnDeserialized]
        internal void OnDeserialized()
        {
            Debug.Log("OnDeserialized");
            Debug.Log("test: " + test);
        }
    }
}
