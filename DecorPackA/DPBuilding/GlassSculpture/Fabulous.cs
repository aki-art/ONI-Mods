using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.UI.USERMENUACTIONS.FABULOUS;

namespace DecorPackA.DPBuilding.GlassSculpture
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class Fabulous : KMonoBehaviour
    {
        [MyCmpReq]
        private readonly KBatchedAnimController anim;

        [MyCmpReq]
        private readonly Artable artable;

        [MyCmpReq]
        private readonly Rotatable rotatable;

        private List<string> fabStages;
        private List<Color> colors;
        private GameObject fx;
        const float duration = 0.33f;
        private int currentIndex = 0;
        float elapsedTime;
        bool shiftColors = false;

        [Serialize]
        public bool Fab { get; set; }

        [SerializeField]
        public Vector3 offset;

        private bool CanBeFab => fabStages.Contains(artable.CurrentStage);

        protected override void OnPrefabInit()
        {
            Fab = false;

            fabStages = new List<string> { "Good5" };

            // just some hand picked colors of the spectrum to cycle through
            colors = new List<Color> {
                new Color32(230, 124, 124, 255),
                new Color32(250, 239, 117, 255),
                new Color32(124, 230, 127, 255),
                new Color32(87, 255, 202, 255),
                new Color32(86, 158, 255, 255),
                new Color32(219, 148, 235, 255),
            };
        }

        protected override void OnSpawn()
        {
            RefreshFab();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
        }

        public void RefreshFab()
        {
            if (!CanBeFab)
            {
                Fab = false;
            }

            anim.SetSymbolVisiblity("fx", Fab);

            if (Fab)
            {
                if(fx == null) CreateSparkleFX();

                if (!shiftColors)
                {
                    // randomizing so the statues aren't synced on world load
                    elapsedTime = Random.Range(0f, duration);
                    currentIndex = Random.Range(0, colors.Count - 1);

                    shiftColors = true;
                    StartCoroutine(ShiftColors());
                    fx.SetActive(true);
                }
            }
            else if (!Fab)
            {
                shiftColors = false;
                StopCoroutine(ShiftColors());
                fx?.SetActive(false);
            }
        }

        private void OnToggleFab()
        {
            Fab = !Fab;
            RefreshFab();
        }

        IEnumerator ShiftColors()
        {
            while (shiftColors)
            {
                elapsedTime += Time.deltaTime;
                float dt = elapsedTime / duration;
                int nextIndex = (currentIndex + 1) % colors.Count;

                anim.SetSymbolTint("fx", Color.Lerp(colors[currentIndex], colors[nextIndex], dt));

                if (elapsedTime >= duration)
                {
                    currentIndex = nextIndex;
                    elapsedTime = 0;
                }

                yield return new WaitForSeconds(.2f);
            }
        }

        private Vector3 GetOffset()
        {
            //return rotatable.GetOrientation() == Orientation.FlipH ? -offset : offset;
            return rotatable.GetRotatedOffset(offset);
        }

        private GameObject CreateSparkleFX()
        {
            fx = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, transform.GetPosition() + GetOffset());
            fx.name = "unicorn_sparkle_fx";
            fx.transform.SetParent(transform);
            fx.SetActive(false);

            return fx;
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (CanBeFab)
            {
                KIconButtonMenu.ButtonInfo button;

                var text = Fab ? DISABLED.NAME : ENABLED.NAME;
                var toolTip = Fab ? DISABLED.TOOLTIP : ENABLED.TOOLTIP;

                button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, OnToggleFab, tooltipText: toolTip);

                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }
    }
}
