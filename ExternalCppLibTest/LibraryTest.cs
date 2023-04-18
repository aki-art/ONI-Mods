using System.IO;
using System;
using FUtility;

public class Library
{
    // This is the only file that is needed in base distribution of the application, all other copies/instances will be created on-demand by Library constructor
    public string DefaultLibrarySource { get; } = "CppDllTest.dll";

    public delegate void T_fibonacci_init(long a, long b);
    public T_fibonacci_init fibonacciInit;

    public delegate bool T_fibonacci_next();
    public T_fibonacci_next fibonacciNext;

    public delegate long T_fibonacci_current();
    public T_fibonacci_current fibonacciCurrent;

    public delegate int T_fibonacci_index();
    public T_fibonacci_index fibonacciIndex;

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

        fibonacciInit = FunctionLoader.LoadFunction<T_fibonacci_init>(path, "fibonacci_init");
        fibonacciNext = FunctionLoader.LoadFunction<T_fibonacci_next>(path, "fibonacci_next");
        fibonacciCurrent = FunctionLoader.LoadFunction<T_fibonacci_current>(path, "fibonacci_current");
        fibonacciIndex = FunctionLoader.LoadFunction<T_fibonacci_index>(path, "fibonacci_index");

        fibonacciInit(1, 1);
        while(fibonacciNext())
        {
            Debug.Log($"{fibonacciIndex()} : {fibonacciCurrent()}");
        }
    }
}