using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FUtility
{
    public class AlternativeMaterial
    {
        public string def;
        public string parentDef;
        public Tag element;

        public AlternativeMaterial(string def, string parentDef, Tag element)
        {
            this.def = def ?? throw new ArgumentNullException(nameof(def));
            this.parentDef = parentDef ?? throw new ArgumentNullException(nameof(parentDef));
            this.element = element;
        }
    }
    class AltMat
    {
        //public static List<AlternativeMaterial> alternativeMats;

        
    }
}
