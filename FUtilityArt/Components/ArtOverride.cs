using FUtility;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace FUtilityArt.Components
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtOverride : KMonoBehaviour
    {
        [Serialize]
        public string overrideStage;

        [SerializeField]
        public List<string> extraStages;

        public bool IsOverrideActive => !overrideStage.IsNullOrWhiteSpace();

        private bool IsMyStage(string id)
        {
            if(id == null)
            {
                return false;
            }

            if(extraStages == null)
            {
                Log.Warning("stages not defined");
            }

            return extraStages.Contains(id);
        }


        public void UpdateOverride(string newId)
        {
            overrideStage = IsMyStage(newId) ? newId : null;
        }
    }
}
