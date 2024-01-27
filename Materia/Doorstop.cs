using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Doorstop;

internal partial class Entrypoint
{
#if DEBUG
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();
#endif

    public static void Start()
    {
        StopCrashHandler();
#if DEBUG
        AllocConsole();
#endif
        Materia.Materia.Initialize();
    }

    public static void StopCrashHandler()
    {
        const string crashHandlerName = "UnityCrashHandler64";
        var path = new FileInfo($"{crashHandlerName}.exe").FullName;
        var process = Process.GetProcessesByName(crashHandlerName).First(p => p.MainModule?.FileName == path);
        process.Kill(true);
    }
}