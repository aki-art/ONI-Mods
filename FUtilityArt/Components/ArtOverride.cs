using KSerialization;

namespace FUtilityArt.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtOverride : KMonoBehaviour
    {
        [Serialize]
        public string overrideStage; // backwards compatibilitx
    }
}
