using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.USERMENUACTIONS.FABULOUS;

namespace DecorPackA.Buildings.GlassSculpture
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Fabulous : KMonoBehaviour
	{
		private const float DURATION = 0.33f;

		[MyCmpReq] private readonly KBatchedAnimController anim;
		[MyCmpReq] private readonly Artable artable;
		[MyCmpReq] private readonly Rotatable rotatable;

		private List<string> fabStages;
		private List<Color> colors;
		private GameObject fx;
		private int currentIndex = 0;
		private float elapsedTime;
		private bool shiftColors = false;

		[Serialize]
		public bool Fab { get; set; }

		[SerializeField]
		public Vector3 offset;

		private bool CanBeFab => fabStages.Contains(artable.CurrentStage);

		public override void OnPrefabInit()
		{
			Fab = false;

			fabStages = new List<string> { "DecorPackA_GlassSculpture_Good5" };

			// just some hand picked colors of the spectrum to cycle through
			// these are more pastelly and spend less time on blues than a regular spectrum, hence this is not scripted with code
			colors = new List<Color> {
				new Color32(230, 124, 124, 255),
				new Color32(250, 239, 117, 255),
				new Color32(124, 230, 127, 255),
				new Color32(87, 255, 202, 255),
				new Color32(86, 158, 255, 255),
				new Color32(219, 148, 235, 255),
			};
		}

		public override void OnSpawn()
		{
			RefreshFab();
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
		}

		public void RefreshFab()
		{
			if (!CanBeFab)
			{
				Fab = false;
			}

			anim.SetSymbolVisiblity("fx", Fab);

			if (Fab)
			{
				if (fx == null)
				{
					CreateSparkleFX();
				}

				if (!shiftColors)
				{
					// randomizing so the statues aren't synced on world load
					elapsedTime = Random.Range(0f, DURATION);
					currentIndex = Random.Range(0, colors.Count - 1);

					shiftColors = true;
					StartCoroutine(ShiftColors());
					fx.SetActive(true);
				}
			}
			else if (!Fab)
			{
				shiftColors = false;
				StopCoroutine(ShiftColors());
				fx?.SetActive(false);
			}
		}

		private void OnToggleFab()
		{
			Fab = !Fab;
			RefreshFab();
		}

		private IEnumerator ShiftColors()
		{
			while (shiftColors)
			{
				elapsedTime += Time.deltaTime;
				var dt = elapsedTime / DURATION;
				var nextIndex = (currentIndex + 1) % colors.Count;

				anim.SetSymbolTint("fx", Color.Lerp(colors[currentIndex], colors[nextIndex], dt));

				if (elapsedTime >= DURATION)
				{
					currentIndex = nextIndex;
					elapsedTime = 0;
				}

				yield return new WaitForSeconds(.066f);
			}
		}

		private GameObject CreateSparkleFX()
		{
			fx = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, transform.GetPosition() + rotatable.GetRotatedOffset(offset));
			fx.name = "unicorn_sparkle_fx";
			fx.transform.SetParent(transform);
			fx.SetActive(false);

			return fx;
		}

		private void OnRefreshUserMenu(object obj)
		{
			if (CanBeFab)
			{
				KIconButtonMenu.ButtonInfo button;

				var text = Fab ? DISABLED.NAME : ENABLED.NAME;
				var toolTip = Fab ? DISABLED.TOOLTIP : ENABLED.TOOLTIP;

				button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggleFab, tooltipText: toolTip);

				Game.Instance.userMenu.AddButton(gameObject, button);
			}
		}
	}
}
