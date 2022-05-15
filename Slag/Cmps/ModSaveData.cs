using KSerialization;

namespace Slag.Cmps
{
    public class ModSaveData : KMonoBehaviour
    {
        [Serialize]
        public bool hasObtainedSlagmiteYet;

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }
    }
}
