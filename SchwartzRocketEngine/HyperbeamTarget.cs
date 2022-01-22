namespace SchwartzRocketEngine
{
    public class HyperbeamTarget : KMonoBehaviour
    {
        public static HyperbeamTarget Instance { get; private set; }

        protected override void OnPrefabInit()
        {
            Instance = this;
        }
    }
}
