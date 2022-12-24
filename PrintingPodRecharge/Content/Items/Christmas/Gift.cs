using FUtility;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Christmas
{
    public class Gift : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText => "Open";

        public string SidescreenButtonTooltip => "";

        public int ButtonSideScreenSortOrder() => 0;

        [Serialize]
        private List<Tag> itemsToSpawn;

        [Serialize]
        private bool started;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if(started)
            {
                StartCoroutine(SpawnGifts());
            }
        }

        public void OnSidescreenButtonPressed()
        {
            itemsToSpawn = new List<Tag>();
            PopulateGifts(itemsToSpawn);
            itemsToSpawn.Shuffle();
            StartCoroutine(SpawnGifts());
            SpawnSparkles(transform.position);
            started = true;
        }

        private void PopulateGifts(List<Tag> itemsToSpawn)
        {
            var eggnogToSpawn = Components.MinionIdentities.Count;
            var sludgeToSpawn = Mathf.Max(1, eggnogToSpawn / 4);

            AddToList(itemsToSpawn, EggnogConfig.ID, eggnogToSpawn);
            AddToList(itemsToSpawn, FruitCakeConfig.ID, sludgeToSpawn);
            itemsToSpawn.Add(RainbowPuftPlushConfig.ID);
            itemsToSpawn.Add(PuftOxyliteConfig.EGG_ID);
        }

        // this destroy self when done
        private void SpawnSparkles(Vector3 position)
        {
            var sparkles = Instantiate(ModAssets.Prefabs.sparklesParticles);
            sparkles.transform.position = position;
            sparkles.SetActive(true);
            sparkles.GetComponent<ParticleSystem>().Play();
        }

        private IEnumerator SpawnGifts()
        {
            GetComponent<KBatchedAnimController>().Play("open");
            while(itemsToSpawn.Count > 0)
            {
                var item = itemsToSpawn[0];
                var spawnedbject = Utils.Spawn(item, gameObject);

                Utils.YeetRandomly(spawnedbject, true, 2, 3, false);

                itemsToSpawn.RemoveAt(0);

                yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
            }

            // wait a little for hopefully comedic timing
            yield return new WaitForSeconds(4f);

            var leek = Utils.Spawn(LeekConfig.ID, gameObject);
            Utils.YeetRandomly(leek, true, 3, 4, true);
            Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, transform.position, 0);

            Util.KDestroyGameObject(gameObject);

            yield return null;
        }

        private void AddToList(List<Tag> list, Tag item, int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                list.Add(item);
            }
        }

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
        {
        }

        public bool SidescreenButtonInteractable() => !started;

        public bool SidescreenEnabled() => !started;
    }
}
