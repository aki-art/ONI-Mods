using System.Collections;
using UnityEngine;

namespace SpookyPumpkin.GhostPip
{
    class GhostSquirrel : KMonoBehaviour, ISim1000ms
    {
        const float FADE_DURATION = 3f;

        KBatchedAnimController kbac;
        Light2D light;

        Color gone = new Color(1, 1, 1, 0f);
        Color day = new Color(1, 1, 1, 0.3f);
        Color night = new Color(1, 1, 1, 1);

        bool dim = false;
        bool shooClicked = false;

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

        public void Sim1000ms(float dt)
        {
            bool isNight = GameClock.Instance.IsNighttime();
            if (dim && isNight) Appear();
            else if (!dim && !isNight) DisAppear(false);
        }

        public void Appear()
        {
            kbac.TintColour = day;
            StartCoroutine(FadeIn());

            if (light != null) light.Lux = 400;
            dim = false;
        }

        public void DisAppear(bool delete)
        {
            kbac.TintColour = night;
            StartCoroutine(FadeOut(delete));
            if (light != null) light.Lux = 0;
            dim = true;
        }

        private void SendAway()
        {
            if (shooClicked)
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

        private void SelectionChanged(object obj)
        {
            if((bool)obj == false) shooClicked = false;
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

        IEnumerator FadeIn()
        {
            float elapsedTime = 0;
            while (elapsedTime < FADE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / FADE_DURATION);
                kbac.TintColour = Color.Lerp(day, night, dt);

                yield return new WaitForSeconds(.1f);
            }
        }

        IEnumerator FadeOut(bool deleteWhenDone = false)
        {
            float elapsedTime = 0;
            Color startColor = kbac.TintColour;
            Color targetColor = deleteWhenDone ? gone : day;

            while (elapsedTime < FADE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / FADE_DURATION);
                kbac.TintColour = Color.Lerp(startColor, targetColor, dt);

                yield return new WaitForSeconds(.1f);
            }

            if (deleteWhenDone)
            {
                gameObject.GetComponent<Storage>().items.ForEach(s => Util.KDestroyGameObject(s));
                kbac.StopAndClear();
                Util.KDestroyGameObject(gameObject);
            }
        }

        protected override void OnCleanUp()
        {
            StopAllCoroutines();
            base.OnCleanUp();
        }
    }
}
