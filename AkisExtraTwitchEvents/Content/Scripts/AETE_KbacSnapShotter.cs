/*using ImGuiNET;
using System.IO;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_KbacSnapShotter : KMonoBehaviour
	{
		private Camera camera;
		private Vector3 voidPosition = new(-100, 0);
		private RenderTexture targetTexture;
		private Texture2D debugWaterTex;
		private static readonly string layer = "Default";//"Pickupable");

		public static AETE_KbacSnapShotter Instance { get; set; }

		public override void OnPrefabInit()
		{
			Instance = this;
		}

		public override void OnCleanUp()
		{
			Instance = null;
		}

		public RenderTexture SnapShot(KBatchedAnimController kbac)
		{
			if (camera == null)
				InitCamera();

			camera.enabled = true;

			var pos = GameUtil.GetActiveTelepad().transform.position + new Vector3(1, 1, 0);

			var target = CopyAnim(kbac.gameObject);
			target.transform.position = pos;
			target.gameObject.SetActive(true);

			camera.transform.position = (pos + new Vector3(1.5f, 1.5f)) with { z = -10 };
			camera.targetTexture = targetTexture;
			camera.Render();

			camera.enabled = false;
			camera.targetTexture = null;

			Destroy(target.gameObject);

			return targetTexture;
		}

		private void InitCamera()
		{
			targetTexture = new RenderTexture(300, 300, 24);

			var reference = CameraController.Instance.baseCamera;

			camera = CameraController.CloneCamera(reference, "AETE_SnapshotCamera");
			camera.transform.parent = reference.transform.parent;
			camera.cullingMask = LayerMask.NameToLayer(layer);
			camera.targetTexture = targetTexture;

			camera.enabled = false;
			//camera.transform.position = voidPosition;
		}

		private KBatchedAnimController CopyAnim(GameObject original)
		{
			var go = new GameObject("AETE_TempKbac");
			go.SetActive(false);

			original.TryGetComponent(out KBatchedAnimController mKbac);
			original.TryGetComponent(out SymbolOverrideController mController);

			var kbac = go.AddComponent<KBatchedAnimController>();
			kbac.animFiles = mKbac.animFiles;

			var controller = SymbolOverrideControllerUtil.AddToPrefab(go);

			kbac.SwapAnims(mKbac.animFiles);

			if (mKbac.OverrideAnimFiles != null)
			{
				foreach (var animOverride in mKbac.OverrideAnimFiles)
				{
					kbac.AddAnimOverrides(animOverride.file, animOverride.priority);
				}
			}

			foreach (var symbol in mController.symbolOverrides)
			{
				controller.AddSymbolOverride(symbol.targetSymbol, symbol.sourceSymbol);
			}

			if (mKbac.animFiles != null)
			{
				foreach (var mAnim in mKbac.animFiles)
				{
					foreach (var anim in kbac.animFiles)
					{
						if (anim?.data.name == anim.name)
						{
							var build = anim.GetData().build;

							foreach (var symbol in build.symbols)
							{
								kbac.SetSymbolVisiblity(symbol.hash, mKbac.GetSymbolVisiblity(symbol.hash));
							}

							break;
						}
					}
				}
			}

			kbac.animScale = mKbac.animScale;
			kbac.TintColour = mKbac.TintColour;
			kbac.offset = mKbac.offset;
			kbac.FlipX = mKbac.flipX;
			kbac.FlipY = mKbac.flipY;
			kbac.Rotation = mKbac.Rotation;

			go.SetLayerRecursively(LayerMask.NameToLayer(layer));

			go.SetActive(true);

			kbac.SetDirty();

			kbac.Play(mKbac.currentAnim, KAnim.PlayMode.Paused);
			kbac.SetPositionPercent(mKbac.GetPositionPercent());

			return kbac;
		}


		public void ImguiDraw()
		{
			if (SelectTool.Instance.selected != null && SelectTool.Instance.selected.TryGetComponent(out KBatchedAnimController kbac))
				if (ImGui.Button("Test Snap"))
				{
					SnapShot(kbac);
					FUtility.Assets.SaveImage(targetTexture, Path.Combine(FUtility.Utils.ModPath, "snaps", $"{kbac.PrefabID()}_{System.DateTime.Now.ToFileTime()}.png"));
				}
		}
	}
}
*/