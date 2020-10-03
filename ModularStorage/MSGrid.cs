using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularStorage
{
    public class MSGrid
    {
        public static int[] networks;

        public static bool GetController(int cell, out MSController controller)
        {
            return ModularStorageManager.Instance.controllers.TryGetValue(networks[cell], out controller);
        }
    }
}
