namespace Materia.Game;

internal sealed unsafe class Il2CppGCHandle : IDisposable
{
    private readonly uint gcHandle;
    public Il2CppGCHandle(void* ptr) => gcHandle = GameInterop.NewIl2CppGCHandle(ptr, false);
    public void Dispose() => GameInterop.FreeIl2CppGCHandle(gcHandle);
}