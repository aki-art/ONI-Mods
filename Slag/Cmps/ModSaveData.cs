using KSerialization;

namespace Slag.Cmps
{
    // once per save file kind of data goes in here
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ModSaveData : KMonoBehaviour
    {
        [Serialize]
        public float lastMiteorShower;

        [Serialize]
        public int mitiorsSpawned;

        public static ModSaveData Instance;

        public bool CanSpawnNewMiteors()
        {
            if (DebugHandler.InstantBuildMode)
            {
                return true;
            }

            return lastMiteorShower + Mod.Settings.MiteorEventCooldownInCycles < GameClock.Instance.GetTimeInCycles();
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
