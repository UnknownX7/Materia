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
#if DEBUG
        AllocConsole();
#endif
        Materia.Materia.Initialize();
    }
}