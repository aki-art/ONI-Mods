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
                return true;
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
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not create directory: " + e.Message);
                return null;
            }
        }

        public static bool AreThesePathsEqual(string patha, string pathb)
        {
            return NormalizePath(patha) == NormalizePath(pathb);
        }

        // https://stackoverflow.com/a/21058152
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}
