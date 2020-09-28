
using System;

namespace FUtility
{
    public interface IModdedBuilding
    {
        MBInfo Info { get; }
    }

    public class MBInfo
    {
        public MBInfo(string ID, string buildMenu, string research = null, string following = null)
        {
            this.ID = ID;
            Research = research;
            BuildMenu = buildMenu;
            Following = following;
        }

        public string Research { get; }
        public string BuildMenu { get; }
        public string Following { get; }
        public string ID { get; }
    }
}
