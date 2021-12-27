using UnityEngine;

namespace FUtility.BuildingUtil
{
    public class SelfBuildingDefReplacer : KMonoBehaviour
    {
        [SerializeField]
        public string targetId;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            BuildingDef replacement = global::Assets.GetBuildingDef(targetId);
            if(replacement != null)
            {
                BuildingComplete buildingComplete = GetComponent<BuildingComplete>();
                Log.Debuglog($"Replacing building {buildingComplete.Def.Tag} with {replacement.Tag}");
                buildingComplete.Def = replacement;
            }
        }
    }
}
