using SpookyPumpkinSO.Content.GhostPip.Spawning;
using System.Collections;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkinSO.Content.GhostPip
{
	internal class GhostSquirrel : KMonoBehaviour, ISim1000ms
	{
		[MyCmpGet] private KBatchedAnimController kbac;
		[MyCmpGet] private Light2D light;

		private const float FADE_DURATION = 3f;
		private const float SHOO_FADE_DURATION = 1f;

		private Color gone = new Color(1, 1, 1, 0f);
		private Color day = new Color(1, 1, 1, 0.3f);
		private Color night = new Color(1, 1, 1, 1);
		private bool dim = false;
		private bool shooClicked = false;

		public override void OnSpawn()
		{
			base.OnSpawn();

			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.HighlightObject, SelectionChanged);

			if (TryGetComponent(out Butcherable butcherable))
				Destroy(butcherable);

			if (TryGetComponent(out FactionAlignment faction))
				faction.SetAlignmentActive(false);
		}

		public void Sim1000ms(float dt)
		{
			var isNight = GameClock.Instance.IsNighttime();

			if (dim && isNight)
				Appear();
			else if (!dim && !isNight)
				DisAppear(false);
		}

		public void Appear()
		{
			kbac.TintColour = day;
			StartCoroutine(FadeIn());

			if (light != null)
				light.Lux = 400;

			dim = false;
		}

		public void DisAppear(bool delete)
		{
			kbac.TintColour = night;
			StartCoroutine(FadeOut(delete));

			if (light != null)
				light.Lux = 0;

			dim = true;
		}

		private void SendAway()
		{
			if (shooClicked)
				DisAppear(true);
			else
			{
				shooClicked = true;
				GameScheduler.Instance.Schedule("resetShoo", 10f, (obj) => shooClicked = false);
			}
		}

		private void SelectionChanged(object obj)
		{
			if ((bool)obj == false)
				shooClicked = false;
		}

		private void OnRefreshUserMenu(object obj)
		{
			var name = GetComponent<UserNameable>().savedName;
			var toolTip = SEND_AWAY.Replace("{Name}", name);

			var button = new KIconButtonMenu.ButtonInfo(
					"action_cancel",
					shooClicked ? CONFIRM : SHOO,
					SendAway,
					tooltipText: toolTip);

			Game.Instance.userMenu.AddButton(gameObject, button);
		}

		private IEnumerator FadeIn()
		{
			var elapsedTime = 0f;

			while (elapsedTime < FADE_DURATION)
			{
				elapsedTime += Time.deltaTime;
				var dt = Mathf.Clamp01(elapsedTime / FADE_DURATION);
				kbac.TintColour = Color.Lerp(day, night, dt);

				yield return new WaitForSeconds(.1f);
			}
		}

		private IEnumerator FadeOut(bool deleteWhenDone = false)
		{
			var elapsedTime = 0f;
			var duration = deleteWhenDone ? SHOO_FADE_DURATION : FADE_DURATION;

			var startColor = kbac.TintColour;
			var targetColor = deleteWhenDone ? gone : day;

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				var dt = Mathf.Clamp01(elapsedTime / duration);
				kbac.TintColour = Color.Lerp(startColor, targetColor, dt);

				yield return new WaitForSeconds(.1f);
			}

			if (deleteWhenDone)
			{
				gameObject.GetComponent<Storage>().DropAll();
				kbac.StopAndClear();

				Util.KDestroyGameObject(gameObject);
			}
		}

		public override void OnCleanUp()
		{
			StopAllCoroutines();

			if (!Game.IsQuitting())
			{
				// re enable spawning a pip from this asteroids printing pod
				var telepad = GameUtil.GetTelepad(gameObject.GetMyWorldId());

				if (telepad is object && telepad.TryGetComponent(out GhostPipSpawner spawner))
					spawner.SetSpawnComplete(false);
			}

			base.OnCleanUp();
		}
	}
}


