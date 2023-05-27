using Bestagons.Content.Map;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Bestagons.Content.ModDb
{
    public class HexTile : Resource
    {
        public string templateId;
        public string icon;
        public ZoneType zoneType;
        public List<PurchasableHex.Price> price;
        public HashSet<string> tags;
        public HashSet<string> providedResources;

        public HexTile(string id, string templateId, string icon, ZoneType zoneType, List<PurchasableHex.Price> price, HashSet<string> tags, HashSet<string> providedResources) : base(id)
        {
            this.templateId = templateId;
            this.icon = icon;
            this.zoneType = zoneType;
            this.price = price;
            this.tags = tags ?? new HashSet<string>();
            this.providedResources = providedResources;
        }

        public bool HasTag(string tag) => tags == null || tags.Contains(tag);
    }
}
