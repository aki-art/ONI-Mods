using System;
using System.Collections.Generic;

namespace Bestagons.Content.ModDb
{
    [Serializable]
    public class HexTileCollection
    {
        public List<Data> HexTiles {  get; set; }

        public List<string> TagAll {  get; set; }

        [Serializable]
        public class Data
        {
            public string Id { get; set; }

            public string TemplateId { get; set; }

            public string Icon { get; set; }

            public string ZoneType { get; set; }

            public List<Price> Price { get; set; }

            public string[] DlcIds { get; set; }

            public string[] RequiredModIds { get; set; }

            public string[] Provides { get; set; }

            public string[] Tags { get; set; }
        }

        [Serializable]
        public class Price
        {
            public string Currency { get; set; }

            public int Amount { get; set; }
        }
    }
}
