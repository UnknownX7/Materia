namespace Materia.Game;

public unsafe class Il2CppObject<T> : IDisposable where T : unmanaged
{
    private readonly Il2CppGCHandle? gcHandle;
    public T* Ptr { get; }

    public Il2CppObject(bool managed = false)
    {
        Ptr = (T*)GameInterop.NewIl2CppObject(Il2CppType<T>.NativePtr);
        if (managed)
            gcHandle = new Il2CppGCHandle(Ptr);
    }

    public Il2CppObject(T* ptr)
    {
        Ptr = ptr;
        gcHandle = new Il2CppGCHandle(ptr);
    }

    public static implicit operator T*(Il2CppObject<T> p) => p.Ptr;

    public override string ToString() => $"{(nint)Ptr:X}<{typeof(T)}>";

    public void Dispose()
    {
        gcHandle?.Dispose();
        GC.SuppressFinalize(this);
    }
}