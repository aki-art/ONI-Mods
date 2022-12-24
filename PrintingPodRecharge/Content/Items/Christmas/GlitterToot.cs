using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Christmas
{
    public class GlitterToot : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText => "Toot";

        public string SidescreenButtonTooltip => "";

        public int ButtonSideScreenSortOrder() => 0;

        public void OnSidescreenButtonPressed()
        {
            // this destroy self when done
            var sparkles = Instantiate(ModAssets.Prefabs.sparklesParticles);
            sparkles.transform.position = transform.position;
            sparkles.SetActive(true);
            sparkles.GetComponent<ParticleSystem>().Play();

            var sound = GlobalAssets.GetSound("Puft_toot");
            PlaySound(sound);
        }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
        {
        }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => true;
    }
}
