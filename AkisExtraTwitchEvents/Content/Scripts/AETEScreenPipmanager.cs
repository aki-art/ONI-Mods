/*using FUtility;
using System;
using System.Collections.Generic;
using Twitchery.Content.Defs;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Scripts
{
    public class AETEScreenPipmanager : KMonoBehaviour, ISim1000ms
    {
        public static AETEScreenPipmanager Instance;

        public static Components.Cmps<DesktopPip> pips = new();
        public bool debugMode;

        public List<Vector3> nodes;

        public List<Vector3> ceilingNodes;
        public List<Vector3> floorNodes;
        public List<Image> markers = new();

        public bool HasActivePip => pips.items.Count > 0;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        public void Refresh()
        {
            floorNodes = new List<Vector3>();
            AddBuildNodes();
            AddNodeFromGo(PlanScreen.Instance.copyBuildingButton);

            UpdateMarkers();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            ScreenResize.Instance.OnResize += OnResizeScreen;
            Refresh();

            *//*            var toolButtons = ToolMenu.Instance.basicTools;

                        foreach (var button in toolButtons)
                        {
                            floorNodes.Add(new Vector3(button.toggle.transform.position.x, 0));
                        }*//*
        }

        private void OnResizeScreen()
        {
            if(pips == null || pips.Count == 0)
            {
                return;
            }

            Refresh();

            //var newUIScale = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale();

            foreach(DesktopPip pip in pips)
            {
                //pip.kbac.animScale = 0.005f * (1f / newUIScale) * 2f;
                pip.UpdatePositionAndTarget();
                pip.Trigger(ModEvents.OnScreenResize);
            }
        }

        private void UpdateMarkers()
        {
            if(!debugMode)
            {
                return;
            }

            if (markers != null)
            {
                foreach (var marker in markers)
                {
                    Util.KDestroyGameObject(marker.gameObject);
                }
            }

            markers = new List<Image>();
            foreach (var node in floorNodes)
            {
                var go = new GameObject();
                go.transform.position = node;
                go.transform.SetParent(FUtility.FUI.Helper.GetACanvas("pip").transform);
                go.SetActive(true);

                var image = go.AddComponent<Image>();
                image.color = new Color(0, 1, 0, 0.4f);
                image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector3.zero);

                go.transform.localScale = new Vector3(0.3f, 0.3f);

                markers.Add(image);
            }
        }

        private void AddBuildNodes()
        {
            var buildMenuToggles = PlanScreen.Instance.toggles;
            var canvas = PlanScreen.Instance.GetComponentInParent<Canvas>();
            foreach (var toggle in buildMenuToggles)
            {
                if(toggle.gameObject.activeSelf)
                {
                    AddNodeFromGo(toggle.gameObject, canvas);
                }
            }
        }

        private void AddNodeFromGo(GameObject go, Canvas canvas = null)
        {
            if(go == null)
            {
                return;
            }

            if (go.TryGetComponent(out RectTransform rect))
            {
                //var pos = RectTransformUtility.PixelAdjustRect(rect, GameScreenManager.Instance.ssOverlayCanvas.GetComponent<Canvas>());
                //Log.Debuglog(pos);
                floorNodes.Add(new Vector3(rect.position.x, 0));
            }
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public DesktopPip CreatePip()
        {
            var go = new GameObject("pip_container");

            go.transform.SetParent(FUtility.FUI.Helper.GetACanvas("pip").transform);

            go.SetActive(true);
            go.transform.position = new Vector3(100, 100);

            var kbac = FXHelpers.CreateEffect(
                "squirrel_kanim",
                go.transform.position,
                go.transform,
                layer: 0);

            var pipGo = kbac.gameObject;

            pipGo.AddOrGet<RectTransform>().localScale = new Vector3(1.5f, 1.5f);

            kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
            kbac.animScale = 0.25f;
            kbac.setScaleFromAnim = false;
            kbac.isMovable = true;
            kbac.materialType = KAnimBatchGroup.MaterialType.UI;
            kbac.animOverrideSize = new Vector2(150, 150);
            kbac.usingNewSymbolOverrideSystem = true;

            kbac.SetLayer(5);
            kbac.SetDirty();
            
            kbac.Play("idle_loop", KAnim.PlayMode.Loop);

            SymbolOverrideControllerUtil.AddToPrefab(pipGo);

            pipGo.AddComponent<StateMachineController>();

            var blockerGo = new GameObject("blocker");
            blockerGo.transform.parent = pipGo.transform.parent;
            blockerGo.SetActive(true);
            blockerGo.AddOrGet<RectTransform>().localScale = new Vector3(0.6f, 1f);

            var pip = pipGo.AddComponent<DesktopPip>();
            pip.blocker = blockerGo.transform;
            pip.PickTarget(floorNodes, true);

            var image = blockerGo.AddComponent<Image>();
            image.color = new Color(1, 1, 0, 0f);

            // TODO: not very efficient to recreate each time, but since it only happens 1-2 times its fine
            image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector3.zero);

            blockerGo.AddOrGet<RectTransform>().localScale = new Vector3(1.5f, 1.5f);

            blockerGo.AddComponent<PipHoverable>().pipKbac = kbac;

            return pip;
        }

        public void Sim1000ms(float dt)
        {
            if(pips == null || pips.Count == 0)
            {
                return;
            }

            Refresh();

            foreach (DesktopPip pip in pips)
            {
                pip.UpdatePositionAndTarget();
            }
        }
    }
}
*/