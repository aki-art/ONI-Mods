using Harmony;
using UnityEngine;

namespace TestObject
{
    class Patches
    {
        [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
        public static class CameraController_OnPrefabInit_Patch
        {
            public static void Prefix()
            {
                Camera.main.gameObject.AddComponent<AudioListener>().enabled = true;
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                //GameTags.UnitCategories.Add(GameTags.IndustrialProduct);
                ModUtil.AddBuildingToPlanScreen("Base", TestBuildingConfig.ID);
            }
        }

        public static Tag myCategory = TagManager.Create("mycat", "My Category");
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                GameTags.UnitCategories.Add(myCategory);
            }
        }

/*        [HarmonyPatch(typeof(WorldInventory), "GetCategoryForEntity")]
        public static class WorldInventory_GetCategoryForEntity_Patch
        {
            public static void Postfix(ref Tag __result, KPrefabID entity)
            {
                if(entity.HasTag(myCategory))
                {
                    __result = myCategory;
                }
            }
        }*/

        [HarmonyPatch(typeof(ResourceCategoryScreen), "OnPrefabInit")]
        public static class ResourceCategoryScreen_Activate_Patch
        {
            public static void Postfix()
            {
            }
        }

        // spawn in 10 test pbjects
        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
                var pos = GameUtil.GetTelepad().transform.position;
                var prefab = Assets.GetPrefab(TestObjectConfig.ID);

                for (int i = 0; i < 10; i++)
                {
                    var obj = Util.KInstantiate(prefab, pos);
                    obj.SetActive(true);
                }
            }
        }

    }
}
