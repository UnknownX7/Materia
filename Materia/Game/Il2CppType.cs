using System.Collections.Concurrent;
using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

#pragma warning disable CS0660, CS0661
public unsafe class Il2CppType
{
    private static readonly ConcurrentDictionary<nint, Il2CppType> cache = new();
    public static Il2CppType WrapPointer(Il2CppClass* @class)
    {
        var address = (nint)@class;
        if (cache.TryGetValue(address, out var type)) return type;
        type = new Il2CppType(@class);
        cache[address] = type;
        return type;
    }

    public static Il2CppType WrapPointer(ref Il2CppClass @class)
    {
        fixed (Il2CppClass* ptr = &@class)
            return WrapPointer(ptr);
    }

    public static Il2CppType WrapPointer<S,R,V>(ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged
    {
        fixed (void* ptr = &@class)
            return WrapPointer((Il2CppClass*)ptr);
    }

    public static Il2CppType WrapPointer(void* ptr) => WrapPointer(*(Il2CppClass**)ptr);

    public Il2CppClass* NativePtr { get; }

    private string? name;
    public string Name => name ??= Util.ReadCString(NativePtr->name);

    private string? @namespace;
    public string Namespace => @namespace ??= NativePtr->declaringType == null ? Util.ReadCString(NativePtr->@namespace) : new Il2CppType(NativePtr->declaringType).FullName;

    private string? fullName;
    public string FullName => fullName ??= !string.IsNullOrEmpty(Namespace) ? $"{Namespace}.{Name}" : Name;

    private Il2CppType? genericType;
    public Il2CppType? GenericType => NativePtr->genericClass != null ? genericType ??= WrapPointer(GameInterop.GetTypeInfo((*NativePtr->genericClass->typeDefinition)->byValTypeIndex)) : null;

    private Il2CppType(Il2CppClass* ptr) => NativePtr = ptr;
    public static bool operator ==(Il2CppType t, void* ptr) => ptr != null && *(void**)ptr == t.NativePtr;
    public static bool operator ==(void* ptr, Il2CppType t) => ptr != null && *(void**)ptr == t.NativePtr;
    public static bool operator !=(Il2CppType t, void* ptr) => ptr == null || *(void**)ptr != t.NativePtr;
    public static bool operator !=(void* ptr, Il2CppType t) => ptr == null || *(void**)ptr != t.NativePtr;
    public static implicit operator Il2CppClass*(Il2CppType t) => t.NativePtr;
    public bool IsSuperclassOf(void* ptr) => ptr != null && GameInterop.IsIl2CppClassSubclassOf(*(Il2CppClass**)ptr, NativePtr);
    public bool IsSubclassOf(void* ptr) => ptr != null && GameInterop.IsIl2CppClassSubclassOf(NativePtr, *(Il2CppClass**)ptr);
    public bool IsAssignableTo(void* ptr) => ptr != null && GameInterop.IsIl2CppClassAssignableFrom(*(Il2CppClass**)ptr, NativePtr);
    public bool IsAssignableFrom(void* ptr) => ptr != null && GameInterop.IsIl2CppClassAssignableFrom(NativePtr, *(Il2CppClass**)ptr);
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
    public static Il2CppClass* NativePtr => Instance.NativePtr;

    public static bool IsSuperclassOf(void* ptr) => Instance.IsSuperclassOf(ptr);
    public static bool IsSubclassOf(void* ptr) => Instance.IsSubclassOf(ptr);
    public static bool IsAssignableTo(void* ptr) => Instance.IsAssignableTo(ptr);
    public static bool IsAssignableFrom(void* ptr) => Instance.IsAssignableFrom(ptr);
    public static T* As(void* ptr) => Instance.IsAssignableFrom(ptr) ? (T*)ptr : null;
    public static bool Is(void* ptr, out T* cast) => (cast = As(ptr)) != null;
}