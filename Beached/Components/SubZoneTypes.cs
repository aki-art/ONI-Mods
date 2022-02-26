namespace Beached.Components
{
    public class SubZoneTypes : KMonoBehaviour
    {
        public static SubZoneTypes Instance;

        protected override void OnPrefabInit()
        {
            Instance = this;
            base.OnPrefabInit();
        }

        protected override void OnCleanUp()
        {
            Instance = null;
            base.OnCleanUp();
        }
    }
}
