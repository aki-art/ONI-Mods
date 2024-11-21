using FUtility;
using System;
using System.IO;

namespace TrueTiles
{
	public class FileUtil
	{
		public static bool TryReadFile(string path, out string result)
		{
			try
			{
				result = File.ReadAllText(path);
				return result != null;
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warning($"Tried to read file at {path}, but could not be read: ", e.Message);
				result = null;
				return false;
			}
		}

		public static string GetOrCreateDirectory(string path)
		{
			try
			{
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				return path;
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
			{
				Log.Warning("Could not create directory: " + e.Message);
				return null;
			}
		}
	}
}
