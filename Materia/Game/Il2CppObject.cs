namespace Materia.Game;

public unsafe class Il2CppObject<T> : IDisposable where T : unmanaged
{
    private readonly uint gcHandle;
    public T* Ptr { get; }

    public Il2CppObject(bool managed = false)
    {
        Ptr = (T*)GameInterop.il2cppObjectNew(Il2CppType<T>.NativePtr);
        if (managed)
            gcHandle = GameInterop.il2cppGCHandleNew(Ptr, 0);
    }

    public override string ToString() => $"{(nint)Ptr:X}<{typeof(T)}>";

    public void Dispose()
    {
        if (gcHandle != 0)
            GameInterop.il2cppGCHandleFree(gcHandle);
        GC.SuppressFinalize(this);
    }
}