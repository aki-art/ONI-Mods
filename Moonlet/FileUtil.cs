using FUtility;
using Newtonsoft.Json;
using System;
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

			var content = TryReadFile(path);

			if (content == null)
				return null;

			var result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
			{
				ObjectCreationHandling = ObjectCreationHandling.Replace
			});

			return result;
		}

		private static string TryReadFile(string path)
		{
			try
			{
				return File.ReadAllText(path);
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warning("Config file found, but could not be read: ", e.Message);
				return null;
			}
		}
	}
}
