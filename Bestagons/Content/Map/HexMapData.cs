using System.Collections.Generic;

namespace Bestagons.Content.Map
{
    public class HexMapData
    {
        public string StartTile {  get; set; }

        public float StartHorizontal { get; set; }

        public float StartVertical { get; set; }

        public List<string> GlobalRequiredTags { get; set; } = new List<string>();

        public List<string> GlobalForbiddenTags { get; set; } = new List<string>();

        public RingData[] Rings { get; set; }

        public class RingData
        {
            public List<string> RequiredTags { get; set; } = new List<string>();

            public List<string> ForbiddenTags { get; set; } = new List<string>();
        }
    }
}
