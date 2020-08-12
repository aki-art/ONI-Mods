using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteriorDecorationVolI.STRINGS.UI.USERMENUACTIONS.FABULOUS;

namespace InteriorDecorationVolI.Buildings.GlassSculpture
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class Fabulous : KMonoBehaviour
    {
        [MyCmpReq]
        private readonly KBatchedAnimController anim;
        [MyCmpReq]
        private readonly Artable artable;

        private List<string> fabStages;
        private List<Color> colors;
        private GameObject fx;
        const float duration = 0.33f;
        private int currentIndex = 0;
        float elapsedTime;
        bool shiftColors = false;

        [Serialize]
        public bool Fab { get; set; }
        private bool CanBeFab => fabStages.Contains(artable.CurrentStage);
        protected override void OnPrefabInit()
        {
            Fab = true;

            fabStages = new List<string>
            {
                "Good5"
            };

            colors = new List<Color> {
                new Color32(230, 124, 124, 255),
                new Color32(250, 239, 117, 255),
                new Color32(124, 230, 127, 255),
                new Color32(87, 255, 202, 255),
                new Color32(86, 158, 255, 255),
                new Color32(219, 148, 235, 255),
            };

            fx = CreateSparkleFX();
        }

        protected override void OnSpawn()
        {
            RefreshFab();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
        }

        public void RefreshFab()
        {
            if (!CanBeFab)
                Fab = false;

            anim.SetSymbolVisiblity("fx", Fab);

            if(Fab && !shiftColors)
            {
                elapsedTime = Random.Range(0f, duration);
                currentIndex = Random.Range(0, colors.Count - 1);

                shiftColors = true;
                StartCoroutine(ShiftColors());
                fx.SetActive(true);
            }
            else if(!Fab)
            {
                shiftColors = false;
                StopCoroutine(ShiftColors());
                fx.SetActive(false);
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


        private GameObject CreateSparkleFX()
        {
            var offset = new Vector3(.5f, .5f, .4f);
            fx = Util.KInstantiate
                (EffectPrefabs.Instance.SparkleStreakFX,
                transform.GetPosition() + offset);
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

                button = new KIconButtonMenu.ButtonInfo(
                    iconName: "action_switch_toggle",
                    text: text,
                    on_click: OnToggleFab,
                    tooltipText: toolTip);

                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }
    }
}
