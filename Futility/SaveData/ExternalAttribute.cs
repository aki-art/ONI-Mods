using System;
using System.IO;

namespace FUtility.SaveData
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ExternalAttribute : Attribute
    {
        public string path;

        public ExternalAttribute()
        {
            path = Path.Combine(Util.RootFolder(), "mods", "settings", "akismods", Log.modName.ToLowerInvariant());
        }

        public ExternalAttribute(params string[] path)
        {
            var subPath = Path.Combine(path);
            this.path = Path.Combine(Util.RootFolder(), subPath);
        }
    }
}
