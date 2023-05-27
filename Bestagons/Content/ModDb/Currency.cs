using System.Collections.Generic;

namespace Bestagons.Content.ModDb
{
    public class Currency : Resource
    {
        public string sprite;
        public int value;

        public Currency(string id, string sprite, int value) : base(id)
        {
            this.sprite = sprite;
            this.value = value;
            Name = Strings.Get($"STRINGS.BESTAGONS.CURRENCY.{id.ToUpperInvariant()}");
        }
    }
}
