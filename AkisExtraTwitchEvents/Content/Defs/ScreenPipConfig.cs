/*using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
    public class ScreenPipConfig
    {
        public const string ID = "AkisExtraTwitchEvents_ScreenPip";

        public GameObject CreatePrefab()
        {
            var prefab = new GameObject(ID);

            Object.DontDestroyOnLoad(prefab);

            var kPrefabId = prefab.AddComponent<KPrefabID>();
            kPrefabId.PrefabTag = TagManager.Create(ID, "Pip");

            prefab.AddComponent<StateMachineController>();

            var kbac = prefab.AddComponent<KBatchedAnimController>();

            kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
            //kbac.transform.localPosition.Set(0, 0, 0);
            kbac.animScale = 0.25f;
            kbac.setScaleFromAnim = false;
            kbac.isMovable = false;
            kbac.materialType = KAnimBatchGroup.MaterialType.UI;
            kbac.animOverrideSize = new Vector2(100, 100);
            kbac.usingNewSymbolOverrideSystem = true;

            kbac.animFiles = new[]
            {
                Assets.GetAnim("squirrel_kanim")
            };

            prefab.AddComponent<ScreenPip>();

            return prefab;
        }
    }
}
*/