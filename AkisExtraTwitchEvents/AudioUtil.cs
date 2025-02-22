/* Code by Romen-H
* https://github.com/romen-h/ONI-Mods/blob/master/src/CommonLib/AudioUtil.cs
* Used with permission. */

using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery
{
	public static class AudioUtil
	{
		public static float soundMultiplier = -10;

		private static readonly Dictionary<int, FMOD.Sound> sounds = [];

		public static bool LoadSound(int key, string soundFile, bool looping = false, bool oneAtATime = false)
		{
			var mode = FMOD.MODE._2D | FMOD.MODE._3D | FMOD.MODE._3D_WORLDRELATIVE | FMOD.MODE.CREATESAMPLE;

			if (looping)
				mode |= FMOD.MODE.LOOP_NORMAL;

			if (oneAtATime)
				mode |= FMOD.MODE.UNIQUE;

			var r1 = RuntimeManager.CoreSystem.createSound(soundFile, mode, out var sound);
			if (r1 == FMOD.RESULT.OK)
			{
				sounds.Add(key, sound);
				return true;
			}
			else
			{
				Debug.LogError($"AutoUtil: Failed to create sound. (key={key}, error={r1})");
				return false;
			}
		}

		public static int PlaySound(int key, float volume = 1f, float pitch = 1)
		{
			if (sounds.TryGetValue(key, out var sound))
			{
				var r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out var cg);
				if (r1 == FMOD.RESULT.OK)
				{
					var r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out var channel);
					if (r2 == FMOD.RESULT.OK)
					{
						var position = new Vector3(SoundListenerController.Instance.transform.position.x,
							 SoundListenerController.Instance.transform.position.y,
							 SoundListenerController.Instance.transform.position.z);

						var pos = position.ToFMODVector();
						var vel = new FMOD.VECTOR();
						channel.set3DAttributes(ref pos, ref vel);
						channel.setVolume(volume);
						channel.setPitch(pitch);

						channel.setPaused(false);

						channel.getIndex(out var index);
						return index;
					}
					else
					{
						Debug.LogError($"AutoUtil: Failed to create sound instance. (key={key}, error={r2})");
					}
				}
				else
				{
					Debug.LogError($"AutoUtil: Failed to get master channel group. (key={key}, error={r1})");
				}
			}
			else
			{
				Debug.LogWarning($"AudioUtil: Tried to play sound that does not exist. (key={key})");
			}

			return -1;
		}

		public static int PlaySound(int key, Vector3 position, float volume = 1f, float pitch = 1)
		{
			if (sounds.TryGetValue(key, out var sound))
			{
				var r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out var cg);
				if (r1 == FMOD.RESULT.OK)
				{
					var r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out var channel);
					if (r2 == FMOD.RESULT.OK)
					{
						var pos = CameraController.Instance.GetVerticallyScaledPosition(position, false).ToFMODVector();
						var vel = new FMOD.VECTOR();
						channel.set3DAttributes(ref pos, ref vel);
						channel.setVolume(volume);
						channel.setPitch(pitch);

						channel.setPaused(false);

						channel.getIndex(out var index);
						return index;
					}
					else
					{
						Debug.LogError($"AutoUtil: Failed to create sound instance. (key={key}, error={r2})");
					}
				}
				else
				{
					Debug.LogError($"AutoUtil: Failed to get master channel group. (key={key}, error={r1})");
				}
			}
			else
			{
				Debug.LogWarning($"AudioUtil: Tried to play sound that does not exist. (key={key})");
			}

			return -1;
		}

		public static FMOD.Channel CreateSound(int key)
		{
			if (sounds.TryGetValue(key, out var sound))
			{
				var r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out var cg);
				if (r1 == FMOD.RESULT.OK)
				{
					var r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out var channel);
					if (r2 == FMOD.RESULT.OK)
					{
						return channel;
					}
					else
					{
						Debug.LogError($"AutoUtil: Failed to create sound instance. (key={key}, error={r2})");
					}
				}
				else
				{
					Debug.LogError($"AutoUtil: Failed to get master channel group. (key={key}, error={r1})");
				}
			}
			else
			{
				Debug.LogWarning($"AudioUtil: Tried to play sound that does not exist. (key={key})");
			}

			return new FMOD.Channel();
		}

		public static void StopSound(int channelID)
		{
			if (channelID < 0)
				return;

			var r1 = RuntimeManager.CoreSystem.getChannel(channelID, out var channel);
			if (r1 == FMOD.RESULT.OK)
			{
				channel.stop();
				channel.clearHandle();
			}
		}
	}
}