using FUtility;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtOverride : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        public List<Artable.Stage> extraStages;

        [SerializeField]
        public Vector3 offset;

        [SerializeField]
        public string animFileName;

        [SerializeField]
        public string[] fallbacks;

        [MyCmpReq]
        private Artable artable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [Serialize]
        public string overrideStage;

        private bool swappedAnim;
        private KAnimFile[] anim;

        private FieldInfo f_currentStage;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            artable.stages.AddRange(extraStages);
            f_currentStage = typeof(Artable).GetField("currentStage", BindingFlags.NonPublic | BindingFlags.Instance);

            anim = new KAnimFile[] { Assets.GetAnim(animFileName) };
        }

        // Switching out the actual status stored on the Artable; this way the game won't soft lock when the mod is removed
        [OnSerializing]
        private void OnSerialize()
        {
            if(!overrideStage.IsNullOrWhiteSpace() && artable != null)
            {
                f_currentStage.SetValue(artable, fallbacks[(int)artable.CurrentStatus]);
            }
        }
        
        // Restore custom override after saving is done
        [OnSerialized]
        private void OnSerialized()
        {
            if (!Game.IsQuitting() && !overrideStage.IsNullOrWhiteSpace() && artable != null)
            {
                f_currentStage.SetValue(artable, overrideStage);
            }
        }
        
        private bool IsValidOverrideId(string id)
        {
            return extraStages.Any(s => s.id == id);
        }

        public void TryOverrideAnim(string stageId)
        {
            if (stageId == "Default") return;

            bool alreadySwapped = stageId == overrideStage && swappedAnim;
            if (alreadySwapped) return;

            if (IsValidOverrideId(stageId))
            {
                OverrideAnim(stageId);
                return;
            }

            if(swappedAnim)
            {
                RestoreAnim();
            }
        }

        private void OverrideAnim(string stageId)
        {
            kbac.SwapAnims(anim);
            kbac.Play(stageId);
            swappedAnim = true;

            overrideStage = stageId;
        }

        // if my animation is active, put it back the the defs anim.
        // otherwise another mod probably already overwrote this, and i shouldn't touch it
        private void RestoreAnim()
        {
            if (kbac.GetCurrentAnim()?.animFile?.name == animFileName)
            {
                kbac.SwapAnims(GetComponent<Building>().Def.AnimFiles);
            }

            overrideStage = null;
            swappedAnim = false;
        }
    }
}
