using UnityEngine;
using UnityEngine.UI;

namespace Randomizer.Content.Scripts
{
    public class RandomizerClusterCategorySelectionScreen : KMonoBehaviour
    {
        [MyCmpReq] private ClusterCategorySelectionScreen selectionScreen;

        private MultiToggle randomizerButton;
        private Image randomizerButtonHeader;
        private Image randomizerButtonSelectionFrame;

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (randomizerButton == null)
            {
                selectionScreen.spacedOutButton.gameObject.SetActive(false);

                var rect = selectionScreen.transform.Find("Panel")
                    .gameObject.AddOrGet<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x + 200, rect.sizeDelta.y);

                randomizerButton = Instantiate(selectionScreen.spacedOutButton, selectionScreen.spacedOutButton.transform.parent)
                    .GetComponent<MultiToggle>();

                randomizerButton.name = "RandomizerGame";

                SetKbac();

                randomizerButton.transform
                    .Find("Label")
                    .GetComponent<LocText>()
                    .SetText(STRINGS.RANDOMIZER.UI.RANDOMIZER);

                selectionScreen.spacedOutButton.gameObject.SetActive(true);
                randomizerButton.gameObject.SetActive(true);
            }

            if (randomizerButton.TryGetComponent(out HierarchyReferences references))
            {
                randomizerButtonHeader = references.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
                randomizerButtonSelectionFrame = references.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
            }

            randomizerButton.onEnter += OnHoverEnterRandomizer;
            randomizerButton.onExit += OnHoverExitRandomizer;
            randomizerButton.onClick += OnClickRandomizer;
        }

        private void SetKbac()
        {
            var kbac = randomizerButton.transform.Find("Border/Anim").GetComponent<KBatchedAnimController>();
            kbac.SwapAnims(new[] { Assets.GetAnim("randomizer_cluster_category_random_kanim") });
            kbac.randomiseLoopedOffset = true;
            kbac.Play("idle_loop", KAnim.PlayMode.Loop);
        }

        private void OnHoverEnterRandomizer()
        {
            PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
            randomizerButtonSelectionFrame.SetAlpha(1f);
            randomizerButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.533333361f, 1f);
            selectionScreen.descriptionArea.text = STRINGS.RANDOMIZER.UI.CHAOS;
        }

        private void OnHoverExitRandomizer()
        {
            PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
            randomizerButtonSelectionFrame.SetAlpha(0.0f);
            randomizerButtonHeader.color = new Color(0.309803933f, 0.34117648f, 0.384313732f, 1f);
            selectionScreen.descriptionArea.text = (string)global::STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
        }

        private void OnClickRandomizer()
        {
            selectionScreen.Deactivate();
            DestinationSelectPanel.ChosenClusterCategorySetting = ModDb.RANDOM_CLUSTER_CATEGORY;
            selectionScreen.NavigateForward();
        }
    }
}
