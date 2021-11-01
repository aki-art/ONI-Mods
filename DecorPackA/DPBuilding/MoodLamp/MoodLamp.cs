using FUtility;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecorPackA.DPBuilding.MoodLamp
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class MoodLamp : StateMachineComponent<MoodLamp.SMInstance>
    {
        [Serialize]
        private string currentVariant;

        [MyCmpReq]
        private KBatchedAnimController animController;

        private KBatchedAnimController tesseractFX;

        private GameObject tesseractFXb;

        private static Vector3 tesserractOffset = new Vector3(0, 0.95f, Grid.GetLayerZ(Grid.SceneLayer.FXFront));

        // TODO: List of a Serializable Variant class, unity cant serialize dicts
        public Dictionary<string, Color> variants;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
            variants = new Dictionary<string, Color>
            {
                { "unicorn", new Color(2.25f, 0, 2.13f, 1f) },
                { "morb", new Color(.27f, 2.55f, .08f, 1f) },
                { "dense", new Color(0.07f, 0.98f, 3.35f, 1f) },
                { "moon", new Color(1.09f, 1.25f, 1.94f, 1f) },
                { "brothgar", new Color(2.47f, 1.75f, .62f, 1f) },
                { "saturn", new Color(2.24f, 1.33f, .58f, 1f) },
                { "pip", new Color(2.47f, 1.75f, .62f, 1f) },
                { "d6", new Color(2.73f, 0.35f, .60f, 1f) },
                { "ogre", new Color(1.14f, 1.69f, 1.94f, 1f) },
                { "tesseract", new Color(0.09f, 1.2f, 2.44f, 1f) },
            };
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (currentVariant.IsNullOrWhiteSpace() || !variants.ContainsKey(currentVariant))
            {
                currentVariant = SelectRandom();
            }

            UpdateTesseractFX();

            smi.StartSM();
        }

        public string SelectRandom()
        {
            var newIdx = Random.Range(0, variants.Count - 1);
            return variants.ElementAt(newIdx).Key;
        }

        private void OnCopySettings(object obj)
        {
            var lamp = ((GameObject)obj).GetComponent<MoodLamp>();
            if (lamp != null)
            {
                SetVariant(lamp.currentVariant);
            }
        }

        internal void SetVariant(string targetVariant)
        {
            currentVariant = targetVariant;
            string suffix = GetComponent<Operational>().IsOperational ? "_on" : "_off";
            animController.Play(currentVariant + suffix);
            GetComponent<Light2D>().Color = variants[currentVariant];

            UpdateTesseractFX();
        }

        private void UpdateTesseractFX()
        {
            if (currentVariant == "tesseract")
            {
                if (tesseractFXb is null)
                {
                    //tesseractFX = FXHelpers.CreateEffect("tesseract_fx_kanim", transform.position + tesserractOffset, transform);
                    // //tesseractFX.PlaySpeedMultiplier = 0.13f;
                    //tesseractFX.Play("idle", KAnim.PlayMode.Loop);
                    tesseractFXb = Instantiate(ModAssets.Prefabs.tesseractFX, transform);

                    //tesseractFXb.transform.localScale *= 0.33f;

                    if (tesseractFXb.TryGetComponent(out ParticleSystemRenderer renderer))
                    {
                        renderer.trailMaterial.shader = Shader.Find("Klei/BloomedParticleShader");
                        renderer.sortingLayerID = LayerMask.NameToLayer("Pickupable");
                    }

                    tesseractFXb.transform.position += tesserractOffset;
                    tesseractFXb.transform.Rotate(Vector3.right, 90f);
                    //tesseractFXb.SetLayerRecursively(LayerMask.NameToLayer("Default"));
                    tesseractFXb.SetLayerRecursively(LayerMask.NameToLayer("Pickupable"));
                    //tesseractFXb.gameObject.layer = gameObject.layer;
                }

                tesseractFXb.gameObject.SetActive(true);
                tesseractFXb.GetComponent<ParticleSystem>().Play();
            }
            else if (tesseractFXb is object)
            {
                tesseractFXb.gameObject.SetActive(false);
            }
        }

        public class Variant
        {
            public string ID;
            public Color color;
        }

        public class States : GameStateMachine<States, SMInstance, MoodLamp>
        {
            public State off;
            public State on;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                off
                    .PlayAnim(smi => smi.master.currentVariant + "_off", KAnim.PlayMode.Paused)
                    .EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
                on
                    .Enter("SetActive", smi =>
                    {
                        smi.GetComponent<Operational>().SetActive(true);
                        smi.GetComponent<Light2D>().Color = smi.master.variants[smi.master.currentVariant];
                    })
                    .PlayAnim(smi => smi.master.currentVariant + "_on", KAnim.PlayMode.Paused)
                    .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, MoodLamp, object>.GameInstance
        {
            public SMInstance(MoodLamp master) : base(master) { }
        }

#if DEBUG
        string x = "0";
        string y = "0";
        string z = "0";
        string w = "0";

        MoodLamp lastSelected;

        // Displays a small UI showing Debug information
        Vector2 scrollPosition;
        private void OnGUI()
        {
            if (!gameObject.GetComponent<KSelectable>().IsSelected && lastSelected != this) return;
            else lastSelected = this;

            GUILayout.BeginArea(new Rect(10, 200, 200, 500));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(200), GUILayout.Height(500));

            x = GUILayout.TextField(x, 12);
            y = GUILayout.TextField(y, 12);
            z = GUILayout.TextField(z, 12);
            w = GUILayout.TextField(w, 12);

            if (GUILayout.Button("Rotate"))
            {
                if (float.TryParse(x, out float xf) && float.TryParse(y, out float yf) && float.TryParse(z, out float zf) && float.TryParse(w, out float wf))
                {
                    //tesseractFXb.transform.rotation.Set(xf, yf, zf, wf);
                    var tess = lastSelected.tesseractFXb;
                    tess?.transform.position.Set(tess.transform.position.x, tess.transform.position.y, zf);
                }
            }

            GUILayout.EndArea();
            GUILayout.EndScrollView();
        }
#endif
    }
}
