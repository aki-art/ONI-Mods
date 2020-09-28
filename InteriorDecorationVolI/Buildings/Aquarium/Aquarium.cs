using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class Aquarium : KMonoBehaviour
    {
        public const int MIN_YEET_DISTANCE = 3;
        public const int MAX_YEET_DISTANCE = 6;

        private AquariumStages.Instance smi;
        GameObject fakeFish;
        GameObject originalFish;

        [MyCmpGet] private Storage storage;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi = new AquariumStages.Instance(this);
            smi.StartSM();
        }

        public void ReplaceFish()
        {
            var receptacle = GetComponent<SingleEntityReceptacle>();
            originalFish = receptacle.Occupant;
            originalFish.SetActive(false);
            var prefab = Assets.GetPrefab(FakeFishConfig.ID);
            fakeFish = Util.KInstantiate(prefab, transform.position);
            receptacle.ForceDepositPickupable(fakeFish.AddOrGet<Pickupable>());
            // Hold fake fish in place
            fakeFish.Trigger((int) GameHashes.OnStore, storage);
            fakeFish.SetActive(true);
        }

        // TODO: First removal doesn't work
        public void RemoveFish()
        {
            var receptacle = GetComponent<SingleEntityReceptacle>();
            receptacle.OrderRemoveOccupant();
            Object.Destroy(fakeFish);
            storage.Drop(originalFish);
            originalFish.transform.SetPosition(transform.position + receptacle.occupyingObjectRelativePosition);
            originalFish.SetActive(true);
            var vec = UnityEngine.Random.insideUnitCircle.normalized;
            vec.y = Mathf.Abs(vec.y);
            vec += new Vector2(0f, UnityEngine.Random.Range(0f, 1f));
            vec *= UnityEngine.Random.Range(MIN_YEET_DISTANCE, MAX_YEET_DISTANCE);
            GameComps.Fallers.Add(originalFish, vec);
            originalFish.AddOrGet<Rotator>().direction = vec;
        }
    }
}
