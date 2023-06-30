using HarmonyLib;
using UnityEngine;
using static ModInfo;

namespace Twitchery.Content.Scripts
{
	public class AETE_EggPostFx : KMonoBehaviour
	{
		public Material material;
		public static AETE_EggPostFx Instance;
		public bool active;
		public float durationSeconds = ModTuning.EGG_DURATION;
		public float elapsed = 0;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
			material = ModAssets.Materials.eggMaterial;
		}

		public void Activate()
		{
			elapsed = 0;
			active = true;
			AudioUtil.PlaySound(ModAssets.Sounds.EGG_SMASH, ModAssets.GetSFXVolume() * 0.8f);
		}

		void Update()
		{
			if (active)
			{
				elapsed += Time.deltaTime;

				if (elapsed > durationSeconds)
				{
					active = false;
					return;
				}

				float t = elapsed / durationSeconds;
				var offset = new Vector2(-0.09f, t);
				material.SetTextureOffset("_DisplacementTex", offset);
				material.SetTextureOffset("_Egg", offset);
				material.SetFloat("_Droopyness", t * 0.5f);
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		[HarmonyPatch(typeof(LightBufferCompositor), "OnRenderImage")]
		public class LightBufferCompositor_OnRenderImage_Patch
		{
			public static void Postfix(RenderTexture src, RenderTexture dest)
			{
				if (Instance.active)
				{
					var buffer = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

					Graphics.Blit(src, buffer, Instance.material);
					Graphics.Blit(buffer, dest);

					RenderTexture.ReleaseTemporary(buffer);
				}
			}
		}

		void Blit(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, material);
		}
	}
}
