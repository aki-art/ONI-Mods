using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
    public class AETE_DitherPostFx : KMonoBehaviour
    {
        public bool isDithered;
        public Material ditherMat;
        public float elapsedTime = 0f;
        public float fadeDurationSeconds = 1f;
        public float durationSeconds = ModTuning.RETRO_VISION_DURATION;
        public int downSamples;
        public bool pointFilterDown = true;

        public DitherConfig fullyDitheredConfig = new(0.04f, 10, 2, 2);
        public DitherConfig defaultDitherConfig = new(0, 64, 0, 0);

        public static AETE_DitherPostFx Instance;

        public struct DitherConfig
        {
            public float spread;
            public int colorCount;
            public int bayerLevel;
            public int downSamples;

            public DitherConfig(float spread, int colorCount, int bayerLevel, int downSamples)
            {
                this.spread = spread;
                this.colorCount = colorCount;
                this.bayerLevel = bayerLevel;
                this.downSamples = downSamples;
            }
        }

        private IEnumerator DitherIn()
        {
            elapsedTime = 0;

            fullyDitheredConfig.downSamples = Screen.width > 2000 ? 3 : 2;

			while (elapsedTime < fadeDurationSeconds)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / fadeDurationSeconds;
                t = Mathf.Clamp01(t);

                SetFade(t);

                yield return new WaitForSeconds(0.03f);
			}

			var totalTime = durationSeconds + fadeDurationSeconds * 2;

			while (elapsedTime < totalTime - fadeDurationSeconds)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(0.03f);
            }

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                var t = 1f - (elapsedTime / fadeDurationSeconds);
                t = Mathf.Clamp01(t);

                SetFade(t);

                yield return new WaitForSeconds(0.03f);
            }

            yield return null;
        }

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            ditherMat = ModAssets.Materials.ditherMaterial;
            UpdateDitherMaterial(defaultDitherConfig.spread, defaultDitherConfig.colorCount, defaultDitherConfig.bayerLevel);
        }

        [HarmonyPatch(typeof(LightBufferCompositor), "OnRenderImage")]
        public class LightBufferCompositor_OnRenderImage_Patch
        {
            public static void Postfix(RenderTexture src, RenderTexture dest)
            {
                if (Instance == null)
                    return;

                if (Instance.isDithered)
				{
					var buffer = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

					Instance.BlitDithering(src, buffer);
					Graphics.Blit(buffer, dest);
					RenderTexture.ReleaseTemporary(buffer);
				}
			}
        }

        public void UpdateDitherMaterial(float spread, int colorCount, int bayerLevel)
        {
            ditherMat.SetFloat("_Spread", spread);
            ditherMat.SetInt("_RedColorCount", colorCount);
            ditherMat.SetInt("_GreenColorCount", colorCount);
            ditherMat.SetInt("_BlueColorCount", colorCount);
            ditherMat.SetInt("_BayerLevel", bayerLevel);
        }

        void BlitDithering(RenderTexture source, RenderTexture destination)
        {
            var width = source.width;
            var height = source.height;

            var textures = new RenderTexture[8];

            var currentSource = source;

            for (int i = 0; i < downSamples; ++i)
            {
                width /= 2;
                height /= 2;

                if (height < 2)
                    break;

                RenderTexture currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, source.format);

                if (pointFilterDown)
                    Graphics.Blit(currentSource, currentDestination, ditherMat, 1);
                else
                    Graphics.Blit(currentSource, currentDestination);

                currentSource = currentDestination;
            }

            RenderTexture dither = RenderTexture.GetTemporary(width, height, 0, source.format);
            Graphics.Blit(currentSource, dither, ditherMat, 0);

            Graphics.Blit(dither, destination, ditherMat, 1);
            RenderTexture.ReleaseTemporary(dither);
            for (int i = 0; i < downSamples; ++i)
            {
                RenderTexture.ReleaseTemporary(textures[i]);
            }
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void SetFade(float t)
        {
            isDithered = t > 0;

            // effect isn't visible, don't waste calculating anything
            if (!isDithered)
                return;

            downSamples = (int)Mathf.Lerp(defaultDitherConfig.downSamples, fullyDitheredConfig.downSamples, t);

            UpdateDitherMaterial(
                Mathf.Lerp(defaultDitherConfig.spread, fullyDitheredConfig.spread, t),
                (int)Mathf.Lerp(defaultDitherConfig.colorCount, fullyDitheredConfig.colorCount, t),
                (int)Mathf.Lerp(defaultDitherConfig.spread, fullyDitheredConfig.spread, t));
        }

        public void DoDither()
        {
            isDithered = true;
            StartCoroutine(DitherIn());
        }

        public void SetDitherExactly(float t)
        {
            StopCoroutine(DitherIn());
            SetFade(t);
        }
    }
}
