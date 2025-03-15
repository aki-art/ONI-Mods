using ImGuiNET;
using System.Collections;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class FallingStuffOverlay : KMonoBehaviour
	{
		[SerializeField] public Color color;

		private readonly static int
			SHIFT_PARAMETER = Shader.PropertyToID("_Shift"),
			LIGHTOVERLAY_PARAMETER = Shader.PropertyToID("_LightOverlay"),
			SCALE_PARAMETER = Shader.PropertyToID("_Scale");

		private MeshRenderer _foreGround;
		private MeshRenderer _midGround;
		private MeshRenderer _backGround;
		private float _elapsed;
		private float _fadeInDuration;
		private Color _transparentColor;

		private float _globalScale, _foreScale, _midScale, _backScale;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			_foreGround = transform.Find("Foreground").GetComponent<MeshRenderer>();
			_midGround = transform.Find("MidGround").GetComponent<MeshRenderer>();
			_backGround = transform.Find("Dust").GetComponent<MeshRenderer>();

			Log.Assert("_fore", _foreGround);
			Log.Assert("_midGround", _midGround);
			Log.Assert("_backGround", _backGround);

			_foreScale = _foreGround.material.GetFloat(SCALE_PARAMETER);
			_midScale = _midGround.material.GetFloat(SCALE_PARAMETER);
			_backScale = _backGround.material.GetFloat(SCALE_PARAMETER);
			_globalScale = 1f;
			_foreGround.material.SetFloat("_LightCap", 0.76f);
			_midGround.material.SetFloat("_LightCap", 0.76f);

		}

		public void FadeIn(float time)
		{
			_elapsed = 0;
			_fadeInDuration = time;
			_transparentColor = color with { a = 0 };
			StartCoroutine(FadeIn());
		}

		private IEnumerator FadeIn()
		{
			while (_elapsed < _fadeInDuration)
			{
				_elapsed += Time.deltaTime;
				UpdateColor(Color.Lerp(_transparentColor, color, _elapsed / _fadeInDuration));

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}
		}

		private IEnumerator FadeOut()
		{
			while (_elapsed < _fadeInDuration)
			{
				_elapsed += Time.deltaTime;
				UpdateColor(Color.Lerp(color, _transparentColor, _elapsed / _fadeInDuration));

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}

			Destroy(gameObject);
		}

		public void FadeOut(float time)
		{
			_elapsed = 0;
			_fadeInDuration = time;
			_transparentColor = color with { a = 0 };

			StartCoroutine(FadeOut());
		}

		public void SetColor(Color color)
		{
			this.color = color;
			_transparentColor = color with { a = 0 };
		}

		private void UpdateColor(Color color)
		{
			_foreGround.material.color = color;
			_midGround.material.color = color with { a = color.a * 0.5f };
			_backGround.material.color = color;
		}

		public void OnShadersReloaded()
		{
			var tex = LightBuffer.Instance.Texture;

			_foreGround.material.SetTexture(LIGHTOVERLAY_PARAMETER, tex);
			_midGround.material.SetTexture(LIGHTOVERLAY_PARAMETER, tex);
			_backGround.material.SetTexture(LIGHTOVERLAY_PARAMETER, tex);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			AlignToWorld();

			var tex = LightBuffer.Instance.Texture;

			OnShadersReloaded();
			//ShaderReloader.Register(OnShadersReloaded);
		}

		public void SetAngleAndSpeed(float angle, float speed)
		{
			var rot = Quaternion.AngleAxis(angle, Vector3.forward);
			var rotatedVec = rot * Vector3.up;
			rotatedVec.Normalize();
			rotatedVec *= speed;

			_foreGround.material.SetVector(SHIFT_PARAMETER, rotatedVec);
			_midGround.material.SetVector(SHIFT_PARAMETER, rotatedVec * 0.5f);
			_backGround.material.SetVector(SHIFT_PARAMETER, rotatedVec * 0.75f);
		}

		public void AlignToWorld()
		{
			var world = this.GetMyWorld();

			transform.position = new Vector3(
				world.WorldOffset.X + world.worldSize.X / 2f,
				world.WorldOffset.X + world.worldSize.Y / 2f,
				Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 2.9f);

			transform.localScale = (Vector2)world.WorldSize;
		}

		public void SetScale(float scale)
		{
			_globalScale = scale;
			_foreGround.material.SetFloat(SCALE_PARAMETER, _foreScale * scale);
			_midGround.material.SetFloat(SCALE_PARAMETER, _midScale * scale);
			_backGround.material.SetFloat(SCALE_PARAMETER, _backScale * scale);
		}

		private float _debugAngle;
		private float _debugSpeed;
		private float _debuglightFade;
		private Vector4 _debugColor;

		public void OnImguiDraw()
		{
			if (ImGui.DragFloat("Global Scale##sandFallGlobalScale", ref _globalScale, 0.01f, 0f, 5f))
			{
				SetScale(_globalScale);
			}

			if (ImGui.DragFloat("Angle##sandFallAngle", ref _debugAngle, 0.1f, 360f)
				|| ImGui.DragFloat("Speed##sandFallSpeed", ref _debugSpeed, 0.001f, 0f, 1f))
			{
				SetAngleAndSpeed(_debugAngle, _debugSpeed);
			}

			if (ImGui.DragFloat("LightFade##sandFallLightFade", ref _debuglightFade, 0.01f, 0f, 1f))
			{
				_foreGround.material.SetFloat("_LightCap", _debuglightFade);
				_midGround.material.SetFloat("_LightCap", _debuglightFade);
			}

			if (ImGui.ColorPicker4("Color##sandFallColor", ref _debugColor))
			{
				SetColor(new Color(_debugColor.x, _debugColor.y, _debugColor.z, _debugColor.w));
				UpdateColor(color);
			}
		}
	}
}
