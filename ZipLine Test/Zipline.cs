using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiplineTest
{
    internal class Zipline : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText => "Connect";

        public string SidescreenButtonTooltip => "";

        public int ButtonSideScreenSortOrder() => 0;

        public void OnSidescreenButtonPressed()
        {
            ZiplineConnectorTool.Instance.Activate();
        }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => true;
    }
}
