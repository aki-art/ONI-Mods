using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;

/// <summary>
/// Helper function to dynamically load DLL contained functions on Windows only
/// </summary>
class FunctionLoader
{
    [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr LoadLibrary(string path);

    [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    /// <summary>
    /// Map String (library name) to IntPtr (reference from LoadLibrary)
    /// </summary>
    private static ConcurrentDictionary<String, IntPtr> LoadedLibraries { get; } = new ConcurrentDictionary<string, IntPtr>();

    /// <summary>
    /// Load function (by name) from DLL (by name) and return its delegate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dllPath"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    public static T LoadFunction<T>(string dllPath, string functionName)
    {
        // Get preloaded or load the library on-demand
        IntPtr hModule = LoadedLibraries.GetOrAdd(
            dllPath,
            valueFactory: (string dllPath) => {
                IntPtr loaded = LoadLibrary(dllPath);
                if (loaded == IntPtr.Zero)
                {
                    throw new DllNotFoundException(String.Format("Library not found in path {0}", dllPath));
                }
                return loaded;
            }
        );
        // Load function
        var functionAddress = GetProcAddress(hModule, functionName);
        if (functionAddress == IntPtr.Zero)
        {
            throw new EntryPointNotFoundException(String.Format("Function {0} not found in {1}", functionName, dllPath));
        }
        // Return delegate, casting is hack-ish, but simplifies usage
        return (T)(object)(Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(T)));
    }
}