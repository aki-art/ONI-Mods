/*using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin
{
    class GhostSquirrel : KMonoBehaviour//, ISim1000ms
    {
        [MyCmpReq] KBatchedAnimController kbac;
        [MyCmpReq] Light2D light;
        [MyCmpReq] KSelectable selectable;
        Color day = Color.blue;//new Color(1, 1, 1, 0.1f);
        Color night = Color.red;//new Color(1, 1, 1, 1);
        const float DURATION = 3f;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (!GameClock.Instance.IsNighttime())
                FadeOut(this);

        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            //Subscribe((int)GameHashes.NewDay, FadeOut);
            //Subscribe((int)GameHashes.Nighttime, FadeIn);
            GameClock.Instance.Subscribe((int)GameHashes.Nighttime, FadeIn);
            GameClock.Instance.Subscribe((int)GameHashes.NewDay, FadeOut);
        }

        public void FadeIn(object obj)
        {
            Debug.Log("fade in");
           // selectable.IsSelectable = true;
            StartCoroutine(ShiftColors(day, night));
            light.enabled = true;
        }

        public void FadeOut(object obj)
        {
            Debug.Log("fade out");
            //selectable.IsSelectable = false;
            StartCoroutine(ShiftColors(night, day));

            light.enabled = false;
        }

        protected override void OnCleanUp()
        {
            StopAllCoroutines();
            base.OnCleanUp();
        }

        IEnumerator ShiftColors(Color c1, Color c2)
        {
            float elapsedTime = 0;
            while (elapsedTime < DURATION)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / DURATION);

                kbac.TintColour = Color.Lerp(c1, c2, dt);

                yield return new WaitForSeconds(.1f);
            }
        }


*//*        public void Sim1000ms(float dt)
        {
            if (GameClock.Instance.IsNighttime() && !visible)
                FadeIn();
            else if (visible)
                FadeOut();
        }*//*
    }
}
*/