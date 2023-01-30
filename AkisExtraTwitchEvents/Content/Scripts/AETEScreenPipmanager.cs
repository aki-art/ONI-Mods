using System;
using System.Collections.Generic;
using Twitchery.Content.Defs;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Scripts
{
    public class AETEScreenPipmanager : KMonoBehaviour
    {
        public static AETEScreenPipmanager Instance;

        public List<Vector3> nodes;

        public bool HasActivePip { get; private set; }

        public TargetOfTheft Target { get; private set; }

        public ScreenPip pip;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            ConfigurePotentialTargets();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            nodes = new List<Vector3>()
            {
                Vector3.zero,
                new Vector3(Screen.width, 0),
                new Vector3(Screen.width, Screen.height),
                new Vector3(0, Screen.height),
            };
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void ActivatePip()
        {
            if(pip != null)
            {
                pip.SetPosition(0);
                return;
            }

            var go = new GameObject("pip_container");

/*            var image = go.AddComponent<Image>();
            image.color = new Color(1, 0, 0, 0.4f);
            image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector3.zero);
*/
            go.transform.SetParent(FUtility.FUI.Helper.GetACanvas("pip").transform);

            go.SetActive(true);
            go.transform.position = new Vector3(100, 100);

            var kbac = FXHelpers.CreateEffect(
                "squirrel_kanim",
                go.transform.position,
                go.transform,
                layer: 0);
            //SpawnOrGetPip().gameObject.SetActive(true);

            kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
            //kbac.transform.localPosition.Set(0, 0, 0);
            kbac.animScale = 0.5f;
            kbac.setScaleFromAnim = false;
            kbac.isMovable = true;
            kbac.materialType = KAnimBatchGroup.MaterialType.UI;
            kbac.animOverrideSize = new Vector2(175, 175);
            kbac.usingNewSymbolOverrideSystem = true;

            kbac.SetLayer(5);
            kbac.SetDirty();

            kbac.Play("idle_loop", KAnim.PlayMode.Loop);

            kbac.gameObject.AddComponent<StateMachineController>();
            pip = kbac.gameObject.AddComponent<ScreenPip>();

            var target = potentialTargets.GetRandom();
            var targetGo = target.getTargetGameObjectFn.Invoke();
            if(targetGo != null)
            {
                target.hideFn.Invoke(targetGo);
            }
        }


        private static List<TargetOfTheft> potentialTargets;

        private void ConfigurePotentialTargets()
        {
            potentialTargets = new List<TargetOfTheft>()
            {
                new TargetOfTheft()
                {
                    bottom = true,
                    getTargetGameObjectFn = () => PlanScreen.Instance.toggles[0].gameObject,
                    getTargetPosition = go => go.transform.position,
                    hideFn = HideBuildButton,
                    restoreFn = RestoreBuildButton
                }
            };

        }

        private void HideBuildButton(GameObject go)
        {
            foreach(var image in go.GetComponents<Image>())
            {
                image.enabled = false;
            }

            foreach (var locText in go.GetComponents<LocText>())
            {
                locText.enabled = false;
            }
        }

        private void RestoreBuildButton(GameObject go)
        {
            foreach (var image in go.GetComponents<Image>())
            {
                image.enabled = true;
            }

            foreach (var locText in go.GetComponents<LocText>())
            {
                locText.enabled = true;
            }
        }

        public class NavNode
        {
            public Vector3 position;

        }

        public class TargetOfTheft
        {
            public bool bottom;
            public Func<GameObject> getTargetGameObjectFn;
            public Func<GameObject, Vector3> getTargetPosition;
            public Action<GameObject> hideFn;
            public Action<GameObject> restoreFn;
        }
    }
}
