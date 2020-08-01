using System.IO;
using System.Reflection;

namespace FUtility
{
    public class Utils
    {
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
