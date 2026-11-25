using System;

namespace SPFrightfulLands
{
    public class DependencyMissingException : Exception
    {
        public DependencyMissingException(string name) : base(GetMessage(name))
        {
        }

        private static string GetMessage(string name)
        {
            return string.Format("Required Mod missing: \"{0}\". Please install all dependencies and enable them in the Mod menu.", name);
        }
    }
}
