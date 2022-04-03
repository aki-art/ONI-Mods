using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FUtilityArt.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtOverride : KMonoBehaviour
    {
        [Serialize]
        public string overrideStage;

        [SerializeField]
        public List<Artable.Stage> extraStages;

        [SerializeField]
        public string animFileName;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private Artable artable;

        private KAnimFile[] anim;

        public bool IsOverrideActive => !overrideStage.IsNullOrWhiteSpace();

        private bool IsMyStage(string id)
        {
            return extraStages.Any(s => s.id == id);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            artable.stages.AddRange(extraStages);
            anim = new KAnimFile[] { Assets.GetAnim(animFileName) };
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (IsOverrideActive)
            {
                kbac.SwapAnims(anim);
                kbac.Play(overrideStage);
            }
        }

        public void UpdateOverride(string newId)
        {
            if (IsMyStage(newId))
            {
                UpdateMyStage(newId);
                return;
            }

            if (IsOverrideActive)
            {
                TryRestoreVanilla();
            }

            overrideStage = null;
        }

        private void UpdateMyStage(string newId)
        {
            if (!IsOverrideActive)
            {
                kbac.SwapAnims(anim);
            }

            overrideStage = newId;
        }

        private void TryRestoreVanilla()
        {
            // check if the current kbac is playing the expected vanilla kanim. otherwise leave it alone,
            // it's probably another mod overriding
            var currentlyPlayedAnim = kbac.GetCurrentAnim()?.animFile?.name;
            if (currentlyPlayedAnim == animFileName)
            {
                var originalAnim = GetComponent<Building>().Def.AnimFiles;
                kbac.SwapAnims(originalAnim);
            }
        }
    }
}
