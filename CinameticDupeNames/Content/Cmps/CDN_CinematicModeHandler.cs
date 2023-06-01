using FUtility;
using KSerialization;
using UnityEngine;
using UnityEngine.UI;

namespace CinematicDupeNames.Content.Cmps
{
    public class CDN_CinematicModeHandler : KMonoBehaviour
    {
        [Serialize] public bool alwaysShowNames;

        private Transform screenshotModeWorldSpaceCanvas;

        public static CDN_CinematicModeHandler Instance { get; private set; }

        public CDN_CinematicModeHandler() => alwaysShowNames = true;

        public Transform GetCamera()
        {
            if (screenshotModeWorldSpaceCanvas == null)
                screenshotModeWorldSpaceCanvas = CreateWorldSpaceNamesCanvas()?.transform;

            Log.Assert("screenshotModeWorldSpaceCanvas", screenshotModeWorldSpaceCanvas);

            return screenshotModeWorldSpaceCanvas;
        }

        private static Canvas CreateWorldSpaceNamesCanvas()
        {
            var go = new GameObject("CDN_NamesCanvas");

            var wsCanvas = GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>();
            var canvas = go.AddComponent<Canvas>();
            canvas.scaleFactor = wsCanvas.scaleFactor;
            canvas.worldCamera = wsCanvas.worldCamera;
            canvas.planeDistance = wsCanvas.planeDistance;
            canvas.sortingLayerID = wsCanvas.sortingLayerID;
            canvas.sortingLayerName = wsCanvas.sortingLayerName;
            canvas.sortingOrder = wsCanvas.sortingOrder;
            canvas.additionalShaderChannels = wsCanvas.additionalShaderChannels;
            canvas.renderMode = RenderMode.WorldSpace;

            canvas.transform.position = wsCanvas.transform.position;
            canvas.transform.parent = wsCanvas.transform.parent;
            canvas.transform.position = wsCanvas.transform.position;

            var wsScaler = wsCanvas.GetComponent<CanvasScaler>();
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.scaleFactor = wsScaler.scaleFactor;
            scaler.useGUILayout = wsScaler.useGUILayout;

            var canvasGroup = go.AddComponent<CanvasGroup>();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.ignoreParentGroups = false;
            canvasGroup.alpha = 1.0f;

            go.AddComponent<GraphicRaycaster>();

            go.SetLayerRecursively(wsCanvas.gameObject.layer);
            go.SetActive(true);
            return canvas;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void ToggleNames()
        {
            alwaysShowNames = !alwaysShowNames;
            UpdateNames();
        }

        public void UpdateNames()
        {
            var shouldShow = DebugHandler.ScreenshotMode && alwaysShowNames;
            var parent = shouldShow
                ? GetCamera()
                : GameScreenManager.Instance.worldSpaceCanvas.transform;

            NameDisplayScreen.Instance.transform.parent = parent ?? GameScreenManager.Instance.worldSpaceCanvas.transform;
        }
    }
}
