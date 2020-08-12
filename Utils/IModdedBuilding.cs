
using System;

namespace FUtility
{
    public interface IModdedBuilding
    {
        MBInfo Info { get; }
    }

    public class MBInfo
    {
        public MBInfo(string iD, string buildMenu, string research = null, string following = null)
        {
            ID = iD;
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
