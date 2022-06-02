using System;

namespace PrintingPodRecharge
{
    [Serializable]
    public class PackageData
    {
        public PackageData(string prefabID, float amount = 1f)
        {
            PrefabID = prefabID;
            Amount = amount;
        }

        public string PrefabID { get; set; }

        public float Amount { get; set; }

        public float ChanceModifier { get; set; } = 1f;

        public float MinCycle { get; set; }

        public float MaxCycle { get; set; }

        public bool HasToBeDicovered { get; set; }

        public bool DLCRequired { get; set; }

        public string[] ModsRequired { get; set; }
    }
}
