using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    // this component should be attached to the controller of the accordion
    class FAccordion : KMonoBehaviour
    {
        public GameObject TargetPanel;

        private LayoutElement layoutElement;
        private RectTransform arrowIcon;

        private bool open = true;
        private bool finishedTransition = true;
        public float targetHeight;
        public float tweenDuration = 1f;

        private FButton controlButton;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            controlButton = gameObject.AddComponent<FButton>();
            controlButton.OnClick += TogglePanel;

            arrowIcon = transform.Find("Arrow").GetComponent<RectTransform>();
        }

        // assign one or more panels controlled by this accordion. the element is expected to have a layoutElement component
        public void SetTarget(GameObject target, float height, bool openByDefault = true)
        {
            TargetPanel = target;
            layoutElement = TargetPanel.GetComponent<LayoutElement>();

            targetHeight = height;

            if (openByDefault)
                OpenTargetWithoutDelay();
            else
                CloseTargetWithoutDelay();
        }

        private void TogglePanel()
        {
            if (finishedTransition)
            {
                if (open)
                {
                    finishedTransition = false;
                    StartCoroutine(CloseTarget());
                    arrowIcon.Rotate(0, 0, 90f);
                    open = false;
                }
                else
                {
                    finishedTransition = false;
                    StartCoroutine(OpenTarget());
                    arrowIcon.Rotate(0, 0, -90f);
                    open = true;
                }
            }
        }


        public IEnumerator CloseTarget()
        {
            float elapsedTime = tweenDuration;
            while (elapsedTime > 0)
            {
                yield return null;
                elapsedTime -= Time.unscaledDeltaTime;
                var percentage = Mathf.Clamp01(elapsedTime / tweenDuration);
                SetTargetHeight(percentage);
            }
            finishedTransition = true;
        }

        public IEnumerator OpenTarget()
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < tweenDuration)
            {
                yield return null;
                elapsedTime += Time.unscaledDeltaTime;
                var percentage = Mathf.Clamp01(elapsedTime / tweenDuration);
                SetTargetHeight(OutBack(percentage));
            }
            finishedTransition = true;
        }


        public void CloseTargetWithoutDelay()
        {
            SetTargetHeight(0);
            finishedTransition = true;
            open = false;
        }
        public void OpenTargetWithoutDelay()
        {
            SetTargetHeight(1f);
            finishedTransition = true;
            open = true;
        }

        private void SetTargetHeight(float percentage)
        {
            layoutElement.preferredHeight = targetHeight * percentage;
        }

        // From Tween.js - Licensed under the MIT license
        // https://github.com/tweenjs/tween.js

        static readonly float s = 1.70158f;
        public static float InBack(float k)
        {
            return k * k * ((s + 1f) * k - s);
        }
        public static float OutBack(float k)
        {
            return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
        }
    }

}

