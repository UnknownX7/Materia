namespace Materia.Game;

internal sealed unsafe class Il2CppGCHandle(void* ptr) : IDisposable
{
    private uint gcHandle = GameInterop.NewIl2CppGCHandle(ptr, false);
    public void Dispose()
    {
        if (gcHandle == 0) return;
        GameInterop.FreeIl2CppGCHandle(gcHandle);
        gcHandle = 0;
    }
}