using HarmonyLib;
using KSerialization;
using System.Collections;
using System.Collections.Generic;

namespace GravitasBigStorage.Content
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GravitasBigStorageUnlockManager : KMonoBehaviour
    {
        public static PlanScreen.RequirementsState needsAnalysis = (PlanScreen.RequirementsState)45454335;

        [Serialize]
        public bool hasUnlockedTech;

        public static GravitasBigStorageUnlockManager Instance;

        private BuildingDef storageDef;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            storageDef = Assets.GetBuildingDef(GravitasBigStorageConfig.ID);
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void UnlockStorage()
        {
            if(!hasUnlockedTech)
            {
                // this is once per playthrough ever, so this is fine :'>
                var buildingDef = Assets.GetBuildingDef(GravitasBigStorageConfig.ID);
                if (buildingDef != null)
                {
                    var planScreenTraverse = Traverse.Create(PlanScreen.Instance);
                    var tagCategoryMap = planScreenTraverse.Field<Dictionary<Tag, HashedString>>("tagCategoryMap");
                    var toggleEntries = planScreenTraverse.Field("toggleEntries").GetValue() as IList;

                    if (tagCategoryMap.Value.ContainsKey(buildingDef.Tag))
                    {
                        var category = tagCategoryMap.Value[buildingDef.Tag];
                        if (GetToggleEntryForCategory(category, toggleEntries, out object toggleEntry))
                        {
                            var toggleTraverse = Traverse.Create(toggleEntry);

                            var pendingAttentions = toggleTraverse.Field<List<Tag>>("pendingResearchAttentions").Value;
                            pendingAttentions.Add(buildingDef.Tag);
                            
                            var toggleInfo = toggleTraverse.Field<KIconToggleMenu.ToggleInfo>("toggleInfo").Value;
                            toggleInfo.toggle
                                .GetComponent<PlanCategoryNotifications>()
                                .ToggleAttention(true);

                            toggleTraverse.Method("Refresh").GetValue();
                        }
                    }

                    var researchCompleteMessage = new FakeResearchCompleteMessage(STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.NAME);
                    MusicManager.instance.PlaySong("Stinger_ResearchComplete");
                    Messenger.Instance.QueueMessage(researchCompleteMessage);

                    if(PlanScreen.Instance != null) PlanScreen.Instance.Refresh();
                    if(BuildMenu.Instance != null) BuildMenu.Instance.Refresh();

                    var toggles = planScreenTraverse.Field<Dictionary<string, PlanBuildingToggle>>("allBuildingToggles").Value;
                    if(toggles.TryGetValue(GravitasBigStorageConfig.ID, out var toggle))
                    {
                        toggle.Refresh();
                    }

                    PlanScreen.Instance.ForceRefreshAllBuildingToggles();
                    planScreenTraverse.Method("BuildButtonList").GetValue();
                }
            }

            hasUnlockedTech = true;
        }

        private bool GetToggleEntryForCategory( HashedString category, IList entries, out object toggleEntry)
        {
            toggleEntry = null;
            var planCategoryField = entries[0].GetType().GetField("planCategory");
            foreach (var toggleEntry1 in entries)
            {
                if ((HashedString)planCategoryField.GetValue(toggleEntry1) == category)
                {
                    toggleEntry = toggleEntry1;
                    return true;
                }
            }

            return false;
        }
    }
}
