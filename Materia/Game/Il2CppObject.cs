namespace Materia.Game;

public unsafe class Il2CppObject<T> : IDisposable where T : unmanaged
{
    private readonly uint gcHandle;
    public T* Ptr { get; }

    public Il2CppObject()
    {
        Ptr = (T*)GameInterop.il2cppObjectNew(Il2CppType<T>.NativePtr);
        gcHandle = GameInterop.il2cppGCHandleNew(Ptr, 1);
    }

    public override string ToString() => $"{(nint)Ptr:X}<{typeof(T)}>";

    public void Dispose()
    {
        if (gcHandle != 0)
            GameInterop.il2cppGCHandleFree(gcHandle);
        GC.SuppressFinalize(this);
    }
}