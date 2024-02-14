namespace Materia.Game;

internal sealed unsafe class Il2CppGCHandle : IDisposable
{
    private readonly uint gcHandle;
    public Il2CppGCHandle(void* ptr) => gcHandle = GameInterop.il2cppGCHandleNew(ptr, 0);
    public void Dispose() => GameInterop.il2cppGCHandleFree(gcHandle);
}