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
		//[SerializeField] public Color color;

		public static readonly int
			DEBUG = Hash.SDBMLower("Debug"),
			SOLAR = Hash.SDBMLower("Solar"),
			SAND_STORM = Hash.SDBMLower("SandStorm"),
			BLIZZARD = Hash.SDBMLower("Blizzard"),
			WATER = Hash.SDBMLower("Water"),
			BLOOD_MOON = Hash.SDBMLower("BloodMoon"),
			MAGMA = Hash.SDBMLower("Magma"),
			HELLFIRE = Hash.SDBMLower("HellFire");

		private static readonly List<OverlayInfo> overlays =
		[
			new OverlayInfo("Debug", Color.white),
			new OverlayInfo("Solar", Util.ColorFromHex("BDECA2")),
			new OverlayInfo("SandStorm", Util.ColorFromHex("F0D86D")),
			new OverlayInfo("Blizzard", Util.ColorFromHex("77F5FF")),
			new OverlayInfo("Water", Util.ColorFromHex("9BDFFF")),
			new OverlayInfo("Magma", Util.ColorFromHex("FF9F9F")),
			new OverlayInfo("HellFire", Util.ColorFromHex("FFA25F")),
			//new OverlayInstance("BloodMoon", Util.ColorFromHex("FF0000")),
			// nice "hellish" #FFA25F00
			// sunset ##B8593300
		];

		private class OverlayInfo(string id, Color color)
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

			var coroutine = FadeInOverlay(instant);
			currentOverlayFadeCoroutine = StartCoroutine(coroutine);
		}

		private IEnumerator FadeInOverlay(bool instant)
		{
			overlayElapsed = instant ? 0 : OVERLAY_FADE;

			if (overlay == null)
			{
				overlay = Instantiate(ModAssets.Prefabs.overlayQuad);

				var world = worldIdx == -1 ? null : ClusterManager.Instance.GetWorld(worldIdx);

				// global
				if (world == null || worldIdx == -1)
				{
					overlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
					overlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);
				}
				else
				{
					overlay.transform.localScale = new Vector3(world.Width, world.Height, 1);
					overlay.transform.position = new Vector3(world.minimumBounds.x + world.Width / 2f, world.minimumBounds.y + world.Height / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);
				}

				overlayMaterial = overlay.GetComponent<MeshRenderer>().materials[0];
				overlayMaterial.renderQueue = 4504;
				overlayColor = WHITE;
				overlayMaterial.color = WHITE;
			}

			overlay.SetActive(true);

			do
			{
				overlayElapsed += Time.deltaTime;

				if (overlayMaterial == null)
					overlayMaterial = overlay.GetComponent<MeshRenderer>().materials[0];

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
