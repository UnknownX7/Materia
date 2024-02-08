using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

#pragma warning disable CS0660, CS0661
public unsafe class Il2CppType
{
    private static readonly Dictionary<nint, Il2CppType> cache = new();
    internal static Il2CppType WrapPointer(Il2CppClass* ptr)
    {
        var address = (nint)ptr;
        if (cache.TryGetValue(address, out var type)) return type;
        type = new Il2CppType(ptr);
        cache[address] = type;
        return type;
    }

    internal static Il2CppType WrapPointer(void* ptr) => WrapPointer(*(Il2CppClass**)ptr);

    public Il2CppClass* NativePtr { get; }

    private string? name;
    public string Name => name ??= Util.ReadCString(NativePtr->name);

    private string? @namespace;
    public string Namespace => @namespace ??= NativePtr->declaringType == null ? Util.ReadCString(NativePtr->@namespace) : new Il2CppType(NativePtr->declaringType).FullName;

    private string? fullName;
    public string FullName => fullName ??= !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;

    private Il2CppType(Il2CppClass* ptr) => NativePtr = ptr;
    public static bool operator ==(Il2CppType t, void* p) => t.Is(p);
    public static bool operator ==(void* p, Il2CppType t) => t.Is(p);
    public static bool operator !=(Il2CppType t, void* p) => t.Is(p);
    public static bool operator !=(void* p, Il2CppType t) => t.Is(p);
    public bool Is(void* ptr) => ptr != null && *(void**)ptr == NativePtr;
    public T* As<T>(void* ptr) where T : unmanaged => Is(ptr) ? (T*)ptr : null; // TODO: Inheritance
}

public static unsafe class Il2CppType<T> where T : unmanaged
{
    private static Il2CppType? instance;
    public static Il2CppType Instance
    {
        get
        {
            if (instance is not null) return instance;
            var ptr = GameInterop.GetTypeInfo<T>();
            if (ptr == null) throw new ArgumentException($"{typeof(T)} is not an Il2Cpp Type");
            return instance = Il2CppType.WrapPointer(ptr);
        }
    }

    public static bool Is(void* ptr) => Instance.Is(ptr);
    public static T* As(void* ptr) => Instance.As<T>(ptr);
}