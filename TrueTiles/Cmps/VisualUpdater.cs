using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueTiles.Cmps
{
    internal class VisualUpdater : KMonoBehaviour
    {
        [MyCmpGet] 
        private SimCellOccupier sco;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            if(sco != null)
            {
            }
        }
    }
}
