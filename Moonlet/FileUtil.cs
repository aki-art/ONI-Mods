using FUtility;
using Klei;
using System.IO;

namespace Moonlet
{
	public class FileUtil
	{
		public static T Read<T>(string path, bool warnIfFailed = false) where T : class
		{
			if (!File.Exists(path))
			{
				if (warnIfFailed)
					Log.Warning($"File does not exist {path}");

				return null;
			}

			var result = YamlIO.LoadFile<T>(path);

			return result;
		}
	}
}
