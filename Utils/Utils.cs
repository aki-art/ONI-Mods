using System.IO;
using System.Reflection;

namespace FUtility
{
    // pointless to be a singleton at the moment, but it will be expanded later
    public class Utils
    {
        public string ModPath { get; set; }
        private static Utils instance = null;
        private static readonly object padlock = new object();

        Utils()
        {
            ModPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static Utils Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new Utils();
                    return instance;
                }
            }
        }
    }
}
