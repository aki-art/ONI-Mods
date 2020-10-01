using Klei.AI;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    public class Aquarium : KMonoBehaviour
    {
        public const int MIN_YEET_DISTANCE = 3;
        public const int MAX_YEET_DISTANCE = 6;

        private AquariumStages.Instance _smi;
        public GameObject originalFish;

        [MyCmpGet] private SingleEntityReceptacle receptacle;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _smi = new AquariumStages.Instance(this);
            _smi.StartSM();
        }

        private readonly AttributeModifier AgeCancelModifier = new AttributeModifier(
            Db.Get().Amounts.Age.deltaAttribute.Id,
            -1f / 600f,
            "Unaging"
        );

        public void AddFish()
        {
            originalFish = receptacle.Occupant;
            originalFish.GetAttributes().Add(AgeCancelModifier);
        }

        public void RemoveFish()
        {
            originalFish.transform.SetPosition(transform.position + receptacle.occupyingObjectRelativePosition);
            var vec = Random.insideUnitCircle.normalized;
            vec.y = Mathf.Abs(vec.y);
            vec += new Vector2(0f, Random.Range(0f, 1f));
            vec *= Random.Range(MIN_YEET_DISTANCE, MAX_YEET_DISTANCE);
            GameComps.Fallers.Add(originalFish, vec);
            originalFish.AddOrGet<Rotator>().SetVec(vec);

            originalFish.GetAttributes().Remove(AgeCancelModifier);
        }
    }
}
