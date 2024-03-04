namespace Materia.Game;

internal sealed unsafe class Il2CppGCHandle : IDisposable
{
    private uint gcHandle;
    public Il2CppGCHandle(void* ptr) => gcHandle = GameInterop.NewIl2CppGCHandle(ptr, false);
    public void Dispose()
    {
        if (gcHandle == 0) return;
        GameInterop.FreeIl2CppGCHandle(gcHandle);
        gcHandle = 0;
    }
}