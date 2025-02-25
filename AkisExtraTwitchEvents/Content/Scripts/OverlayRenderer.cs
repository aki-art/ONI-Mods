using ImGuiNET;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SkipSaveFileSerialization]
	[SerializationConfig(MemberSerialization.OptIn)]
	public class OverlayRenderer : KMonoBehaviour
	{
		private const float OVERLAY_FADE = 5f;
		private readonly static Color WHITE = Color.white;

		private GameObject overlay;
		private Color targetOverlayColor;
		private Color overlayColor;
		private float overlayElapsed;
		private Coroutine currentOverlayFadeCoroutine;
		private Material overlayMaterial;
		private Vector4 debugOverlayColor;
		private bool debugInstant;

		[SerializeField] public int worldIdx;

		public static readonly int
			DEBUG = Hash.SDBMLower("Debug"),
			SOLAR = Hash.SDBMLower("Solar"),
			SAND_STORM = Hash.SDBMLower("SandStorm"),
			BLIZZARD = Hash.SDBMLower("Blizzard"),
			BLOOD_MOON = Hash.SDBMLower("BloodMoon");

		private static readonly List<OverlayInstance> overlays =
		[
			new OverlayInstance("Debug", Color.white),
			new OverlayInstance("Solar", Util.ColorFromHex("BDECA2")),
			new OverlayInstance("SandStorm", Util.ColorFromHex("F0D86D")),
			new OverlayInstance("Blizzard", Util.ColorFromHex("77F5FF")),
			//new OverlayInstance("BloodMoon", Util.ColorFromHex("FF0000")),
			// nice "hellish" #FFA25F00
			// sunset ##B8593300
		];


		private class OverlayInstance(string id, Color color)
		{
			public readonly int id = Hash.SDBMLower(id);
			public string stringId = id;
			public Color color = color;
			public bool isActive;
		}

		public void Disable()
		{
			if (overlay != null)
				overlay.SetActive(false);
		}

		private void FadeOut() => StartCoroutine(FadeOutOverlay());

		public void ToggleOverlay(int id, bool enabled, bool instant)
		{
			var color = Color.black;
			var toggledOverlay = overlays.Find(o => o.id == id);

			if (toggledOverlay == null)
			{
				Log.Warning("Invalid overlay id");
				return;
			}

			toggledOverlay.isActive = enabled;

			UpdateOverlays(instant);
		}

		private void UpdateOverlays(bool instant)
		{
			var color = Color.black;
			var anyOverlayActive = false;

			foreach (var overlay in overlays)
			{
				if (overlay.isActive)
				{
					color += overlay.color;
					anyOverlayActive = true;
				}
			}

			if (anyOverlayActive)
			{
				color.a = 1f;

				var maxValue = Mathf.Max(color.r, color.g, color.b);
				if (maxValue > 1f)
				{
					var mult = 1f / maxValue;
					color *= mult;
				}

				SetOverlayColor(color, instant);
			}
			else
				FadeOut();
		}

		private void SetOverlayColor(Color color, bool instant)
		{
			targetOverlayColor = color;

			if (currentOverlayFadeCoroutine != null)
				StopCoroutine(currentOverlayFadeCoroutine);

			currentOverlayFadeCoroutine = StartCoroutine(FadeInOverlay(instant));
		}

		private IEnumerator FadeInOverlay(bool instant)
		{
			overlayElapsed = instant ? OVERLAY_FADE : 0;

			if (overlay == null)
			{
				overlay = Instantiate(ModAssets.Prefabs.overlayQuad);
				overlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
				overlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

				overlayMaterial = overlay.GetComponent<MeshRenderer>().materials[0];

				overlayColor = WHITE;
				overlayMaterial.color = WHITE;
			}

			overlay.SetActive(true);

			do
			{
				overlayElapsed += Time.deltaTime;
				overlayMaterial.color = Color.Lerp(overlayColor, targetOverlayColor, overlayElapsed / OVERLAY_FADE);

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}
			while (overlayElapsed < OVERLAY_FADE);

			overlayColor = targetOverlayColor;
			yield return null;
		}

		private IEnumerator FadeOutOverlay()
		{
			if (overlay != null && overlayMaterial != null)
			{
				overlayElapsed = 0;
				targetOverlayColor = WHITE;

				while (overlayElapsed < OVERLAY_FADE)
				{
					overlayElapsed += Time.deltaTime;
					overlayMaterial.color = Color.Lerp(overlayColor, targetOverlayColor, overlayElapsed / OVERLAY_FADE);

					yield return SequenceUtil.WaitForSeconds(0.033f);
				}

				overlay.SetActive(false);
			}

			overlayColor = targetOverlayColor;
			yield return null;
		}

		public void OnImguiRender()
		{
			if (ImGui.CollapsingHeader("Overlays##AETE_Overlays"))
			{
				ImGui.Checkbox("Instant##AETE_DebugInstant", ref debugInstant);

				ImGui.Spacing();

				foreach (var overlay in overlays)
				{
					if (ImGui.Checkbox(overlay.stringId, ref overlay.isActive))
						UpdateOverlays(debugInstant);
				}

				ImGui.Spacing();

				if (ImGui.ColorPicker4("Debug Overlay Color##AETE_DebugColor", ref debugOverlayColor))
				{
					foreach (var overlay in overlays)
					{
						if (overlay.id == DEBUG)
						{
							overlay.color = new Color(debugOverlayColor.x, debugOverlayColor.y, debugOverlayColor.z, 1f);
							break;
						}
					}

					UpdateOverlays(debugInstant);
				}
			}
		}
	}
}
