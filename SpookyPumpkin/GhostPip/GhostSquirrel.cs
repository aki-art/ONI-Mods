using SpookyPumpkin.GhostPip.Spawning;
using System.Collections;
using UnityEngine;
using static SpookyPumpkin.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkin.GhostPip
{
    internal class GhostSquirrel : KMonoBehaviour, ISim1000ms
    {
        private const float FADE_DURATION = 3f;
        private const float SHOO_FADE_DURATION = 1f;
        private KBatchedAnimController kbac;
        private Light2D light;
        private Color gone = new Color(1, 1, 1, 0f);
        private Color day = new Color(1, 1, 1, 0.3f);
        private Color night = new Color(1, 1, 1, 1);
        private bool dim = false;
        private bool shooClicked = false;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            light = GetComponent<Light2D>();
            kbac = GetComponent<KBatchedAnimController>();

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            Subscribe((int)GameHashes.HighlightObject, SelectionChanged);

            if (TryGetComponent(out Butcherable butcherable))
            {
                Util.KDestroyGameObject(butcherable);
            }

            if (TryGetComponent(out FactionAlignment faction))
            {
                faction.SetAlignmentActive(false);
            }

            // ModAssets.SetPipWorld(true);
        }

        public void Sim1000ms(float dt)
        {
            bool isNight = GameClock.Instance.IsNighttime();
            if (dim && isNight)
            {
                Appear();
            }
            else if (!dim && !isNight)
            {
                DisAppear(false);
            }
        }

        public void Appear()
        {
            kbac.TintColour = day;
            StartCoroutine(FadeIn());

            if (light != null)
            {
                light.Lux = 400;
            }

            dim = false;
        }

        public void DisAppear(bool delete)
        {
            kbac.TintColour = night;
            StartCoroutine(FadeOut(delete));
            if (light != null)
            {
                light.Lux = 0;
            }

            dim = true;
        }

        private void SendAway()
        {
            if (shooClicked)
            {
                DisAppear(true);
            }
            else
            {
                shooClicked = true;
                GameScheduler.Instance.Schedule("resetShoo", 10f, (obj) => shooClicked = false);
            }
        }

        private void SelectionChanged(object obj)
        {
            if ((bool)obj == false)
            {
                shooClicked = false;
            }
        }

        private void OnRefreshUserMenu(object obj)
        {
            string name = GetComponent<UserNameable>().savedName;
            string toolTip = $"Send {name} away forever";

            KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo(
                    iconName: "action_cancel",
                    text: shooClicked ? CONFIRM : SHOO,
                    on_click: SendAway,
                    tooltipText: toolTip);

            Game.Instance.userMenu.AddButton(gameObject, button);
        }

        private IEnumerator FadeIn()
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

        private IEnumerator FadeOut(bool deleteWhenDone = false)
        {
            float elapsedTime = 0;
            float duration = deleteWhenDone ? SHOO_FADE_DURATION : FADE_DURATION;

            Color startColor = kbac.TintColour;
            Color targetColor = deleteWhenDone ? gone : day;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float dt = Mathf.Clamp01(elapsedTime / duration);
                kbac.TintColour = Color.Lerp(startColor, targetColor, dt);

                yield return new WaitForSeconds(.1f);
            }

            if (deleteWhenDone)
            {
                // re enable spawning a pip from this asteroids printing pod
                GameObject telepad = GameUtil.GetTelepad(gameObject.GetMyWorldId());
                if (telepad is object && telepad.TryGetComponent(out GhostPipSpawner spawner))
                {
                    spawner.SetSpawnComplete(false);
                }

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


