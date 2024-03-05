using ECGen.Generated;
using ECGen.Generated.Command.UI;
using ECGen.Generated.UnityEngine;

namespace Materia.Game;

public static unsafe class GameExtensions
{
    public static Il2CppType GetIl2CppType(this ref Il2CppClass @class) => Il2CppType.WrapPointer(ref @class);
    public static string GetName(this ref Il2CppClass @class) => Il2CppType.WrapPointer(ref @class).Name;
    public static string GetNamespace(this ref Il2CppClass @class) => Il2CppType.WrapPointer(ref @class).Namespace;
    public static string GetFullName(this ref Il2CppClass @class) => Il2CppType.WrapPointer(ref @class).FullName;
    public static Il2CppClass* GetGenericClass(this ref Il2CppClass @class) => Il2CppType.WrapPointer(ref @class).GenericType is { } t ? t.NativePtr : null;

    public static Il2CppType GetIl2CppType<S,R,V>(this ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged => Il2CppType.WrapPointer(ref @class);
    public static string GetName<S,R,V>(this ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged => Il2CppType.WrapPointer(ref @class).Name;
    public static string GetNamespace<S,R,V>(this ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged => Il2CppType.WrapPointer(ref @class).Namespace;
    public static string GetFullName<S,R,V>(this ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged => Il2CppType.WrapPointer(ref @class).FullName;
    public static Il2CppClass* GetGenericClass<S,R,V>(this ref Il2CppClass<S,R,V> @class) where S : unmanaged where R : unmanaged where V : unmanaged => Il2CppType.WrapPointer(ref @class).GenericType is { } t ? t.NativePtr : null;

    public static bool IsActive(this ref GameObject obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr);
    }

    public static void SetActive(this ref GameObject obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static bool IsActive(this ref Component obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr);
    }

    public static void SetActive(this ref Component obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static bool IsActive(this ref Canvas obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr);
    }

    public static void SetActive(this ref Canvas obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static bool IsActive(this ref SingleTapButton obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr);
    }

    public static void SetActive(this ref SingleTapButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static bool CanTapButton(this ref SingleTapButton singleTapButton, bool checkActive)
    {
        fixed (SingleTapButton* ptr = &singleTapButton)
            return GameInterop.CanTapButton(ptr, checkActive);
    }

    public static bool TapButton(this ref SingleTapButton singleTapButton, bool checkActive = true, uint lockoutMs = 2000, uint delayMS = 0)
    {
        fixed (SingleTapButton* ptr = &singleTapButton)
            return GameInterop.TapButton(ptr, checkActive, lockoutMs, delayMS);
    }

    public static bool IsActive(this ref TintButton obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr);
    }

    public static void SetActive(this ref TintButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static bool CanTapButton(this ref TintButton tintButton, bool checkActive)
    {
        fixed (TintButton* ptr = &tintButton)
            return GameInterop.CanTapButton(ptr, checkActive);
    }

    public static bool TapButton(this ref TintButton tintButton, bool checkActive = true, uint lockoutMs = 2000, uint delayMS = 0)
    {
        fixed (TintButton* ptr = &tintButton)
            return GameInterop.TapButton(ptr, checkActive, lockoutMs, delayMS);
    }
}