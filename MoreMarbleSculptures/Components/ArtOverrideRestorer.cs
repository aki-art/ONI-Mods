using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    // This class makes it so that removing the mod won't soft lock the save file.
    // It switches out the artable currentStage id to some vanilla ID just before saving, and then reverts it once saving is complete.
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

        // pushing value directly to the field, instead of calling SetStage, this way no side effects should happen
        // this is arguably hacky but i have to make sure Artable.SetStage won't crash looking for an incorrect ID even after my mod is gone
        [OnSerializing]
        private void OnSerialize()
        {
            if (artOverride.IsOverrideActive && artable != null)
            {
                f_currentStage.SetValue(artable, fallbacks[(int)artable.CurrentStatus]);
            }
        }

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
