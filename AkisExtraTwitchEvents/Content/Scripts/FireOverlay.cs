using FUtility;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Twitchery.Content.Scripts
{
	public class FireOverlay : KMonoBehaviour
	{
		public VideoPlayer videoPlayer;
		public RenderTexture renderTexture;

		[MyCmpReq] private RawImage rawImage;

		private bool isPlaying;

		public void OnHotTubToggled(bool isEnabled)
		{
			var isTempMode = OverlayScreen.Instance.GetMode() == OverlayModes.Temperature.ID;

			if (isEnabled && !isPlaying && isTempMode)
				Play();
			else if (!isEnabled && isPlaying)
				Stop();
		}

		public void Play()
		{
			if (!AkisTwitchEvents.Instance.HotTubActive)
				return;

			isPlaying = true;

			if (videoPlayer == null)
				videoPlayer = transform.Find("Video").GetComponent<VideoPlayer>();

			if (videoPlayer.clip == null)
			{
				Log.Warning("clip is null");
				Stop();
				return;
			}

			videoPlayer.gameObject.SetActive(true);


			renderTexture = new RenderTexture(Convert.ToInt32(videoPlayer.clip.width), Convert.ToInt32(videoPlayer.clip.height), 16);

			rawImage.texture = renderTexture;
			videoPlayer.targetTexture = renderTexture;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			//videoPlayer.clip = clip;
			videoPlayer.Play();
		}

		public void Stop()
		{
			isPlaying = false;
			if (videoPlayer != null)
			{
				videoPlayer.Stop();
				videoPlayer.gameObject.SetActive(false);
			}

			renderTexture = null;
		}
	}
}
