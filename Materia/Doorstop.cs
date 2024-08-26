namespace Doorstop;

internal class Entrypoint
{
    public static void Start()
    {
#if DEBUG
        Materia.Utilities.Util.AllocConsole();
#endif
        Materia.Materia.Initialize();
    }
}