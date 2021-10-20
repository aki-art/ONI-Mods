using FUtility;
using Slag.Critter;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Items
{
    public class RandomRewardSpawner : KMonoBehaviour
    {
        [SerializeField] 
        public List<WeightedMetalOption> options;
        [MyCmpReq] 
        PrimaryElement primaryElement;

        public GameObject SpawnReward()
        {
            if (!options.IsNullOrEmpty())
            {
                SimHashes chosenElement = options.GetWeightedRandom().element;

                Element element = ElementLoader.FindElementByHash(chosenElement);
                GameObject result = element.substance.SpawnResource(
                    transform.position,
                    primaryElement.Mass,
                    primaryElement.Temperature,
                    primaryElement.DiseaseIdx,
                    primaryElement.DiseaseCount);

                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, element.name, result.transform);
                return result;
            }

            else return null;
        }

        public void DestroySelf()
        {
            Util.KDestroyGameObject(gameObject);
        }
    }
}
