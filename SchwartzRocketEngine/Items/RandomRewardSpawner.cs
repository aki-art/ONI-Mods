using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace SchwartzRocketEngine.Items
{
    public class RandomRewardSpawner : KMonoBehaviour
    {
        [SerializeField]
        public string source;

        [MyCmpReq] 
        PrimaryElement primaryElement;

        public GameObject SpawnReward()
        {
            if (string.IsNullOrEmpty(source) && ModAssets.siftRewards.TryGetValue(source, out List<SiftResultOption> options))
            {
                GameObject result = null;

                SiftResultOption chosenElement = options.GetWeightedRandom();

                if (ElementLoader.GetElement(chosenElement.Tag) is Element element) { 
                    result = element.substance.SpawnResource(
                        transform.position,
                        chosenElement.Mass,
                        primaryElement.Temperature,
                        primaryElement.DiseaseIdx,
                        primaryElement.DiseaseCount);
                }
                else if(Assets.GetPrefab(chosenElement.Tag) is object)
                {
                    Utils.Spawn(chosenElement.Tag, gameObject);
                }

                if(result is object)
                {
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, chosenElement.Tag.ProperNameStripLink(), result.transform);
                }

                return result;
            }

            return null;
        }

        public void DestroySelf()
        {
            Util.KDestroyGameObject(gameObject);
        }
    }
}
