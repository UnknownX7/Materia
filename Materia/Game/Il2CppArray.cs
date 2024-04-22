using ECGen.Generated;
using ECGen.Generated.System.Collections.Generic;

namespace Materia.Game;

public unsafe class Il2CppArray<T> : IDisposable where T : unmanaged
{
    private readonly Il2CppGCHandle? gcHandle;
    public Unmanaged_Array<T>* Ptr { get; private set; }
    public int Size => Ptr->size;

    public Il2CppArray(int size, bool managed = false)
    {
        Ptr = (Unmanaged_Array<T>*)GameInterop.NewIl2CppArray(Il2CppType<T>.NativePtr, size);
        if (managed)
            gcHandle = new Il2CppGCHandle(Ptr);
    }

    public Il2CppArray(IReadOnlyList<T> array, bool managed = false)
    {
        Ptr = (Unmanaged_Array<T>*)GameInterop.NewIl2CppArray(Il2CppType<T>.NativePtr, array.Count);
        for (int i = 0; i < array.Count; i++)
            ((T*)Ptr->items)[i] = array[i];
        if (managed)
            gcHandle = new Il2CppGCHandle(Ptr);
    }

    public Il2CppArray(T*[] array, bool managed = false)
    {
        Ptr = (Unmanaged_Array<T>*)GameInterop.NewIl2CppArray(Il2CppType<T>.NativePtr, array.Length);
        for (int i = 0; i < array.Length; i++)
            Ptr->items[i] = (long)array[i];
        if (managed)
            gcHandle = new Il2CppGCHandle(Ptr);
    }

    public Il2CppArray(IReadOnlyList<nint> array, bool managed = false)
    {
        Ptr = (Unmanaged_Array<T>*)GameInterop.NewIl2CppArray(Il2CppType<T>.NativePtr, array.Count);
        for (int i = 0; i < array.Count; i++)
            Ptr->items[i] = array[i];
        if (managed)
            gcHandle = new Il2CppGCHandle(Ptr);
    }

    public Il2CppArray(Unmanaged_Array<T>* ptr)
    {
        Ptr = ptr;
        gcHandle = new Il2CppGCHandle(ptr);
    }

    public Il2CppArray(void* ptr)
    {
        Ptr = (Unmanaged_Array<T>*)ptr;
        gcHandle = new Il2CppGCHandle(ptr);
    }

    public static implicit operator Unmanaged_Array<T>*(Il2CppArray<T> p) => p.Ptr;
    public static implicit operator Unmanaged_IReadOnlyList<T>*(Il2CppArray<T> p) => (Unmanaged_IReadOnlyList<T>*)p.Ptr;

    public override string ToString() => $"{(nint)Ptr:X}<{typeof(T)}[]>";

    public void Dispose()
    {
        gcHandle?.Dispose();
        Ptr = null;
        GC.SuppressFinalize(this);
    }
}