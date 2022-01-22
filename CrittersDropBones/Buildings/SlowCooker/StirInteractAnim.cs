using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class StirInteractAnim : KMonoBehaviour
    {
        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private LoopingSounds sounds;

        private float animationLength;

        private PositionKey currentPosition;
        private PositionKey nextPosition;

        private AnimKey currentAnim;
        private AnimKey nextAnim;

        private float timer;
        private float positionDt;
        private float animDt;

        private bool finished = false;
        private bool paused = true;

        [Serialize]
        private bool needsReset;

        List<PositionKey> positionKeys;
        int positionIndex;

        List<AnimKey> animKeys;
        int animIndex;

        struct PositionKey
        {
            public float time;
            public Vector3 position;

            public PositionKey(float time, Vector3 position)
            {
                this.time = time;
                this.position = position;
            }
        }

        struct SoundKey
        {
            public float time;
            public string sound;

            public SoundKey(float time, string sound)
            {
                this.time = time;
                this.sound = sound;
            }
        }

        struct AnimKey
        {
            public float time;
            public string anim;
            public string overrideAnim;
            public KAnim.PlayMode playMode;
            public int startPercent;
            public int endPercent;
            public float speed;

            public AnimKey(float time, string anim, string overrideAnim = "", KAnim.PlayMode playMode = KAnim.PlayMode.Once, int startPercent = 0, int endPercent = 100, float speed = 1f)
            {
                this.time = time;
                this.anim = anim;
                this.overrideAnim = overrideAnim;
                this.playMode = playMode;
                this.startPercent = startPercent;
                this.endPercent = endPercent;
                this.speed = speed;
            }
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            // hard coded mess for the time being
            positionKeys = new List<PositionKey>()
            {
                new PositionKey(0, new Vector3(0, 0)),
                new PositionKey(0.5f, new Vector3(0, 0f)),
                new PositionKey(5f, new Vector3(0, 0)),
            };
            positionKeys.OrderBy(pk => pk.time);

            animKeys = new List<AnimKey>()
            {
                new AnimKey(0, "working_pre", "anim_interacts_fabricator_generic_kanim", KAnim.PlayMode.Loop),
                new AnimKey(0.5f, "working_loop", "anim_interacts_compost_kanim", KAnim.PlayMode.Loop),
                new AnimKey(0.5f, "working_loop"),
                new AnimKey(20f, "working_pst", "anim_interacts_fabricator_generic_kanim", KAnim.PlayMode.Loop)
            };
            positionKeys.OrderBy(pk => pk.time);

            animationLength = positionKeys.Max(pk => pk.time);
            animationLength = Math.Max(animKeys.Max(pk => pk.time), animationLength);

            currentPosition = positionKeys.ElementAt(positionIndex++);
            nextPosition = positionKeys.ElementAt(positionIndex++);

            currentAnim = animKeys.ElementAt(animIndex++);
            nextAnim = animKeys.ElementAt(animIndex++);
        }

        public void Begin()
        {
            timer = 0;
            finished = false;
            needsReset = true;
            Unpause();

            //sounds.StopAllSounds();
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;

            if (!currentAnim.overrideAnim.IsNullOrWhiteSpace())
            {
                var curAnim = Assets.GetAnim(currentAnim.overrideAnim);
                kbac.Stop();
                kbac.AddAnimOverrides(curAnim);
                kbac.Play(currentAnim.anim, currentAnim.playMode);
            }
        }

        public void LateUpdate()
        {
            sounds.StopSound("Compost_shovel_in");
            sounds.StopSound("Compost_shovel_out");

            if (finished || paused || SpeedControlScreen.Instance.IsPaused) return;

            if (timer > animationLength)
            {
                Trigger((int)ModHashes.AnimationFinished, this);
                finished = true;
                timer = 0;
            }

            timer += Time.deltaTime;

            if (timer > nextPosition.time)
            {
                currentPosition = nextPosition;
                nextPosition = positionKeys.Count > positionIndex ? positionKeys.ElementAt(positionIndex++) : default;
                positionDt = 0;
            }

            positionDt += Time.deltaTime / (nextPosition.time - currentPosition.time);
            kbac.Offset = Vector3.Lerp(currentPosition.position, nextPosition.position, positionDt);

            animDt += Time.deltaTime;

            if (animDt >= nextAnim.time)
            {

                if (!nextAnim.overrideAnim.IsNullOrWhiteSpace())
                {
                    var anim = Assets.GetAnim(nextAnim.overrideAnim);
                    var curAnim = Assets.GetAnim(currentAnim.overrideAnim);
                    kbac.Stop();
                    kbac.RemoveAnimOverrides(curAnim);
                    kbac.AddAnimOverrides(anim);
                    kbac.Play(nextAnim.anim, nextAnim.playMode);
                }
                else if (!currentAnim.overrideAnim.IsNullOrWhiteSpace())
                {
                    kbac.RemoveAnimOverrides(Assets.GetAnim(currentAnim.overrideAnim));
                }

                animDt = 0;
                currentAnim = nextAnim;
                nextAnim = animKeys.Count > animIndex ? animKeys.ElementAt(animIndex++) : default;
            }

            if (kbac.CurrentAnim.name != currentAnim.anim || kbac.IsStopped() && currentAnim.playMode == KAnim.PlayMode.Loop)
            {
                kbac.Stop();
                kbac.Play(currentAnim.anim, currentAnim.playMode);
                kbac.SetPositionPercent(currentAnim.startPercent);
            }
            else if (kbac.GetPositionPercent() >= currentAnim.endPercent)
            {
                kbac.SetPositionPercent(currentAnim.startPercent);
            }

        }

        protected override void OnCleanUp()
        {
            ResetEverything();
            base.OnCleanUp();
        }

        private void ResetEverything()
        {
            if(!needsReset)
            {
                return;
            }

            needsReset = false;

            kbac.Offset = Vector3.zero;
            if (!currentAnim.overrideAnim.IsNullOrWhiteSpace())
            {
                kbac.RemoveAnimOverrides(Assets.GetAnim(currentAnim.overrideAnim));
            }
        }

        public void OnEnable()
        {
            Begin();
        }

        public void OnDisable()
        {
            ResetEverything();
        }
    }
}
