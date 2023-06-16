/*using FUtility;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LongWormy : KMonoBehaviour
    {
        [Serialize]
        public bool isLongBoi;

        [SerializeField]
        public bool hasBeenStretched;

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (isLongBoi && !hasBeenStretched)
            {
                GameScheduler.Instance.ScheduleNextFrame("lengteh longboi", Replace);
            }
        }

        private void Replace(object obj)
        {
            Log.Debuglog("replacing worm");

            var newWorm = SpawnLongWorm(Grid.PosToCell(this));
            var thisAttributes = GetComponent<Modifiers>();
            var newAttributes = newWorm.GetComponent<Modifiers>();

            foreach (var amount in thisAttributes.amounts)
            {
                if (amount != null)
                {
                    if(newAttributes.amounts.Get(amount.modifier.Id) == null)
                    {
                        newAttributes.amounts.Add(new AmountInstance(amount.amount, newWorm));
                    }

                    newAttributes.amounts.SetValue(amount.modifier.Id, amount.value);
                    newAttributes.amounts.SetValue(amount.modifier.minAttribute.Id, amount.minAttribute.GetBaseValue());
                    newAttributes.amounts.SetValue(amount.modifier.maxAttribute.Id, amount.maxAttribute.GetBaseValue());
                    newAttributes.amounts.SetValue(amount.modifier.deltaAttribute.Id, amount.deltaAttribute.GetBaseValue());
                }
            }

            if (thisAttributes.sicknesses != null)
            {
                foreach (var sickness in thisAttributes.sicknesses)
                {
                    if (sickness != null)
                    {
                        newAttributes.sicknesses.CreateInstance(sickness.Sickness);
                    }
                }
            }

            Util.KDestroyGameObject(gameObject);
        }

        public static GameObject SpawnLongWorm(int cell)
        {
            Log.Debuglog("spawning worm");

            var prefab = Assets.GetPrefab(DivergentWormConfig.ID);

            var go = GameUtil.KInstantiate(prefab, Grid.CellToPos(cell), Grid.SceneLayer.Creatures);
            go.GetComponent<KPrefabID>().AddTag(TTags.longBoi, true);

            if (go.TryGetComponent(out LongWormy wormy))
            {
                wormy.isLongBoi = true;
                wormy.hasBeenStretched = true;
            }

            go.SetActive(true);
            return go;
        }
    }
}
*/