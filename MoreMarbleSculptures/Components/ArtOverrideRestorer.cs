using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    public class ArtOverrideRestorer : KMonoBehaviour
    {
        [SerializeField]
        public string[] fallbacks;

        [MyCmpReq]
        private Artable artable;

        [MyCmpReq]
        private ArtOverride artOverride;

        private FieldInfo f_currentStage;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            f_currentStage = typeof(Artable).GetField("currentStage", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [OnSerializing]
        private void OnSerialize()
        {
            if (artOverride.IsOverrideActive && artable != null)
            {
                f_currentStage.SetValue(artable, fallbacks[(int)artable.CurrentStatus]);
            }
        }

        // Restore custom override after saving is done
        [OnSerialized]
        private void OnSerialized()
        {
            if (!Game.IsQuitting() && artOverride.IsOverrideActive && artable != null)
            {
                f_currentStage.SetValue(artable, artOverride.overrideStage);
            }
        }
    }
}
