using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

namespace DecorPackA.Buildings.MoodLamp
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>, ISaveLoadable
    {
        [MyCmpReq]
        private readonly KBatchedAnimController animController;

        [MyCmpReq]
        private readonly Operational operational;

        [MyCmpReq]
        private readonly Light2D light2D;

        [Serialize]
        public string currentVariantID;

        public const string GLITTER_PUFT = "glitterpuft";

        public static Dictionary<string, Variant> variants = new Dictionary<string, Variant>()
        {
            { "unicorn", new Variant(VARIANT.UNICORN, 2.25f, 0, 2.13f) },
            { "morb", new Variant(VARIANT.MORB, .27f, 2.55f, .08f) },
            { "dense", new Variant(VARIANT.DENSE, 0.07f, 0.98f, 3.35f) },
            { "moon", new Variant(VARIANT.MOON, 1.09f, 1.25f, 1.94f) },
            { "brothgar", new Variant(VARIANT.BROTHGAR, 2.47f, 1.75f, .62f) },
            { "saturn", new Variant(VARIANT.SATURN, 2.24f, 1.33f, .58f) },
            { "pip", new Variant(VARIANT.PIP, 2.47f, 1.75f, .62f) },
            { "d6", new Variant(VARIANT.D6, 2.73f, 0.35f, .60f) },
            { "ogre", new Variant(VARIANT.OGRE, 1.14f, 1.69f, 1.94f) },
            { "tesseract", new Variant(VARIANT.TESSERACT, 0.09f, 1.2f, 2.44f, true) },
            { "cat", new Variant(VARIANT.CAT, 2.55f, 2.22f, 1.48f) },
            { "owo", new Variant(VARIANT.OWO, 2.55f, 1.13f, 2.2f) },
            { "star", new Variant(VARIANT.STAR, 2.47f, 1.75f, .62f) },

            // v1.2
            { "konny87", new Variant(VARIANT.KONNY87, 0.14f, 1.46f, 2.55f) },
            { "kleimug", new Variant(VARIANT.KLEI_MUG, 2.55f, 1f, 0) },
            { "redstonelamp", new Variant(VARIANT.REDSTONE_LAMP, 2.55f, 1f, 0) },
            //{ "cuddlepip", new Variant(Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.NAME), 1.11f, 0.35f, 2.05f) },
            { "cuddlepip", new Variant(VARIANT.CUDDLE_PIP, 1.11f, 0.35f, 2.05f) },
            { "archivetube", new Variant(VARIANT.ARCHIVE_TUBE, 0.38f, 2.55f, 0.58f, true) },
            { "lumaplays", new Variant(VARIANT.LUMAPLAYS, 0.96f, 2.55f, 0.1f) },
            { "diamondhatch", new Variant(VARIANT.DIAMONDHATCH, 0.55f, 0.75f, 2.01f) },
            { "beeta", new Variant(Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.BEE.NAME), 0, 2.55f, 0) },
            { GLITTER_PUFT, new Variant(VARIANT.GLITTERPUFT, 0, 0, 0, true) },
            { "ai", new Variant(VARIANT.AI, 0.38f, 2.55f, 0.58f, true) },
            { "slagmite", new Variant(VARIANT.SLAGMITE, 1.14f, 1.69f, 1.94f) },
        };

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            // roll a new one if there is nothing set yet
            if (currentVariantID.IsNullOrWhiteSpace())
            {
                SetVariant(GetRandom());
            }

            light2D.IntensityAnimation = 1.5f;
            base.OnSpawn();
            smi.StartSM();
        }

        // gives a randomly selected key of a variant
        public string GetRandom()
        {
            var newIdx = UnityEngine.Random.Range(0, variants.Count - 1);
            return variants.ElementAt(newIdx).Key;
        }

        // copy the selected lamp type when the user copies building settings
        private void OnCopySettings(object obj)
        {
            var lamp = ((GameObject)obj).GetComponent<MoodLamp>();
            if (lamp != null)
            {
                SetVariant(lamp.currentVariantID);
            }
        }

        internal void SetVariant(string targetVariant)
        {
            if (variants.ContainsKey(targetVariant))
            {
                currentVariantID = targetVariant;
                RefreshAnimation();
            }
        }

        private void RefreshAnimation()
        {
            if (operational.IsOperational)
            {
                var variant = variants[currentVariantID];
                animController.Play(currentVariantID + "_on", variant.mode);
                light2D.Color = variant.color;
            }
            else
            {
                animController.Play(currentVariantID + "_off");
            }

            gameObject.AddOrGet<GlitterLight2D>().enabled = currentVariantID == GLITTER_PUFT;
        }

        [Serializable]
        public class Variant
        {
            public string description;
            public Color color;
            public KAnim.PlayMode mode;

            public Variant(string description, float r, float g, float b, bool loop = false)
            {
                this.description = description;

                if (!Mod.Settings.MoodLamp.VibrantColors)
                {
                    r = Mathf.Clamp01(r);
                    g = Mathf.Clamp01(g);
                    b = Mathf.Clamp01(b);
                }

                color = new Color(r, g, b, 1f) * 0.5f;
                mode = loop ? KAnim.PlayMode.Loop : KAnim.PlayMode.Paused;
            }
        }

        public class States : GameStateMachine<States, SMInstance, MoodLamp>
        {
            public State off;
            public State on;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                off
                    .Enter("SetInactive", smi => smi.master.RefreshAnimation())
                    .EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
                on
                    .Enter("SetActive", smi =>
                    {
                        smi.GetComponent<Operational>().SetActive(true);
                        smi.master.RefreshAnimation();
                    })
                    .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, MoodLamp, object>.GameInstance
        {
            public SMInstance(MoodLamp master) : base(master) { }
        }
    }
}
