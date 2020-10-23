using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin.GhostPip
{
    class GhostSquirrel : KMonoBehaviour, ISim1000ms
    {
        const float DURATION = 3f;

        KBatchedAnimController kbac;
        Light2D light;

        Color gone = new Color(1, 1, 1, 0f);
        Color day = new Color(1, 1, 1, 0.3f);
        Color night = new Color(1, 1, 1, 1);

        bool dim = false;
        bool shooClicked = false;

        public void Appear()
        {
            kbac.TintColour = day;
            StartCoroutine(FadeIn());

            if (light != null)  light.Lux = 400;
            dim = false;

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            light = GetComponent<Light2D>();
            kbac = GetComponent<KBatchedAnimController>();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            Subscribe((int)GameHashes.HighlightObject, SelectionChanged);

            var b = GetComponent<Butcherable>();
            if(b != null) Util.KDestroyGameObject(b);

            var faction = GetComponent<FactionAlignment>();
            if(faction != null) faction.SetAlignmentActive(false);

            ModAssets.SetPipWorld(true);

        }

        private void SelectionChanged(object obj)
        {
            if((bool)obj == false) shooClicked = false;
        }

        public void DisAppear(bool delete)
        {
            kbac.TintColour = night;
            StartCoroutine(FadeOut(delete));
            if (light != null)  light.Lux = 0;
            dim = true;
        }

        protected override void OnCleanUp()
        {
            StopAllCoroutines();
            base.OnCleanUp();
        }

        private void OnRefreshUserMenu(object obj)
        {
            var text = STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN.SHOOBUTTON;
            string name = GetComponent<UserNameable>().savedName;
            var toolTip = $"Send {name} away forever";

            var button = new KIconButtonMenu.ButtonInfo(
                    iconName: "action_cancel",
                    text: shooClicked ? (LocString)"Are you sure?" : text,
                    on_click: SendAway,
                    tooltipText: toolTip);

            Game.Instance.userMenu.AddButton(gameObject, button);
        }

        private void SendAway()
        {
            if(shooClicked)
            {
                DisAppear(true);
                ModAssets.SetPipWorld(false);
            }
            else
            {
                shooClicked = true;
                GameScheduler.Instance.Schedule("resetShoo", 10f, (obj) => shooClicked = false);
            }
        }

        IEnumerator FadeIn()
        {
            float elapsedTime = 0;
            while (elapsedTime < DURATION)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / DURATION);
                kbac.TintColour = Color.Lerp(day, night, dt);

                yield return new WaitForSeconds(.1f);
            }
        }

        IEnumerator FadeOut(bool deleteWhenDone = false)
        {
            float elapsedTime = 0;
            Color startColor = kbac.TintColour;
            Color targetColor = deleteWhenDone ? gone : day;

            while (elapsedTime < DURATION)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / DURATION);
                kbac.TintColour = Color.Lerp(startColor, targetColor, dt);

                yield return new WaitForSeconds(.1f);
            }

            if (deleteWhenDone)
            {
                gameObject.GetComponent<Storage>().items.ForEach(s => Util.KDestroyGameObject(s));
                //kbac.Stop();
                kbac.StopAndClear();
                Util.KDestroyGameObject(gameObject);
            }
        }

        public void Sim1000ms(float dt)
        {
            bool isNight = GameClock.Instance.IsNighttime();
            if (dim && isNight) Appear(); 
            else if(!dim && !isNight) DisAppear(false);
        }
    }
}
