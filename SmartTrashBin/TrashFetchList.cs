using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTrashBin
{
    class TrashFetchList : IFetchList
    {
        public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();
        public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();
        private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();
        public Storage Destination { get; private set; }
        public int PriorityMod { get; private set; }

        public TrashFetchList(Storage destination)
        {
            Destination = destination;
        }

        public void Add(Tag tag, float amount = 1f)
        {
            if (!MinimumAmount.ContainsKey(tag))
            {
                MinimumAmount[tag] = amount;
            }

            FetchOrder2 item = new FetchOrder2(
                Db.Get().ChoreTypes.Fetch,
                new Tag[] { tag },
                null,
                null,
                Destination,
                amount,
                FetchOrder2.OperationalRequirement.None,
                PriorityMod);

            FetchOrders.Add(item);
        }

        public void Remove(Tag tag)
        {
            for (int i = 0; i < FetchOrders.Count; i++)
            {
                if(FetchOrders[i].Tags.Contains(tag))
                {
                    FetchOrders[i].Cancel("");
                    FetchOrders[i] = null;
                }
            }
        }

        public void SetPriorityMod(int priorityMod)
        {
            PriorityMod = priorityMod;
            FetchOrders.ForEach(f => f.SetPriorityMod(priorityMod));
        }

        public float GetMinimumAmount(Tag tag)
        {
            MinimumAmount.TryGetValue(tag, out float result);
            return result;
        }

        public Dictionary<Tag, float> GetRemaining()
        {
            return Remaining;
        }

        public Dictionary<Tag, float> GetRemainingMinimum()
        {
            return default;
        }
    }
}
