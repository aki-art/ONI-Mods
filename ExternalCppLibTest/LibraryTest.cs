using System.IO;
using System;
using FUtility;

public class Library
{
    // This is the only file that is needed in base distribution of the application, all other copies/instances will be created on-demand by Library constructor
    public string DefaultLibrarySource { get; } = "FastNoise.dll";

    public delegate int T_externalFunction(int numArgument, out uint outNumArgument);
    public T_externalFunction fnNewFromMetadata;

    public Library()
    {
        // reference correct DLL file per instance ID
        var LibraryName = DefaultLibrarySource;
        var path = Path.Combine(Utils.ModPath, "lib", DefaultLibrarySource);

        // ensure the library itself is in place
        if (!File.Exists(path))
        {
            Log.Warning("No file found at " + path);
        }

        // load each individual function that needs to be available later
        fnNewFromMetadata = FunctionLoader.LoadFunction<T_externalFunction>(path, "fnNewFromMetadata");
    }
}