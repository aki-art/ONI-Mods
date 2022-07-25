using System.Collections.Generic;

namespace Backwalls.Integration.Blueprints
{
    public class AdditionalData
    {
        public string Name { get; set; }

        public Vector2I Size { get; set; }

        public Dictionary<Vector2I, BackwallData> Data { get; set; }

        public class BackwallData
        {
            public string Pattern { get; set; }

            public int ColorIdx { get; set; }
        }
    }
}
