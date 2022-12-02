using KSerialization;

namespace PrintingPodRecharge.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BioInksD6Manager : KMonoBehaviour
    {
        public static BioInksD6Manager Instance;

        [Serialize]
        public int diceCount;

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

        public bool HasDice()
        {
            return diceCount > 0;
        }

        public bool UseDie()
        {
            if(diceCount <= 0)
            {
                return false;
            }

            diceCount--;
            return true;
        }

        public void AddDice(int count)
        {
            diceCount += count;
        }
    }
}
