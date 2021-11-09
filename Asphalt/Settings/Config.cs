using FUtility.SaveData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asphalt.Settings
{
    public class Config : IUserSetting
    {
        public string Test { get; set; } = "TestValue";
    }
}
