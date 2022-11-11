/* Code by Romen-H
 * https://github.com/romen-h/ONI-Mods/blob/master/src/CommonLib/AudioUtil.cs
 * Used with permission. */

using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace SnowSculptures
{
    public static class AudioUtil
    {
        private static readonly Dictionary<string, FMOD.Sound> sounds = new Dictionary<string, FMOD.Sound>();

        public static bool LoadSound(string key, string soundFile, bool looping = false, bool oneAtATime = false)
        {
            FMOD.MODE mode = FMOD.MODE._2D | FMOD.MODE._3D | FMOD.MODE._3D_WORLDRELATIVE | FMOD.MODE.CREATESAMPLE;

            if (looping)
                mode |= FMOD.MODE.LOOP_NORMAL;

            if (oneAtATime)
                mode |= FMOD.MODE.UNIQUE;

            FMOD.RESULT r1 = RuntimeManager.CoreSystem.createSound(soundFile, mode, out FMOD.Sound sound);
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

        public static int PlaySound(string key, Vector3 position, float volume = 1f)
        {
            if (sounds.TryGetValue(key, out var sound))
            {
                FMOD.RESULT r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup cg);
                if (r1 == FMOD.RESULT.OK)
                {
                    FMOD.RESULT r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out FMOD.Channel channel);
                    if (r2 == FMOD.RESULT.OK)
                    {
                        FMOD.VECTOR pos = CameraController.Instance.GetVerticallyScaledPosition(position, false).ToFMODVector();
                        var vel = new FMOD.VECTOR();
                        channel.set3DAttributes(ref pos, ref vel);
                        channel.setVolume(volume);

                        channel.setPaused(false);

                        channel.getIndex(out int index);
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

        public static FMOD.Channel CreateSound(string key)
        {
            if (sounds.TryGetValue(key, out var sound))
            {
                FMOD.RESULT r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup cg);
                if (r1 == FMOD.RESULT.OK)
                {
                    FMOD.RESULT r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out FMOD.Channel channel);
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

            FMOD.RESULT r1 = RuntimeManager.CoreSystem.getChannel(channelID, out FMOD.Channel channel);
            if (r1 == FMOD.RESULT.OK)
            {
                channel.stop();
                channel.clearHandle();
            }
        }
    }
}