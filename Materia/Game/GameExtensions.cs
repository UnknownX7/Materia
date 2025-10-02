using ECGen.Generated;
using ECGen.Generated.Command.UI;
using ECGen.Generated.Cysharp.Threading.Tasks;
using ECGen.Generated.UnityEngine;
using ImGuiNET;
using Materia.Attributes;

namespace Materia.Game;

[Injection]
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

    public static bool IsActive(this ref GameObject obj, bool inHierarchy = false)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr, inHierarchy);
    }

    public static void SetActive(this ref GameObject obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static void ChangeActive(this ref GameObject obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.ChangeGameObjectActive(ptr, active);
    }

    public static string GetTag(this ref GameObject obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.GetGameObjectTag(ptr)!;
    }

    public static Transform* GetTransform(this ref GameObject obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.GetGameObjectTransform(ptr);
    }

    public static bool IsActive(this ref Component obj, bool inHierarchy = false)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr, inHierarchy);
    }

    public static void SetActive(this ref Component obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static void ChangeActive(this ref Component obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.ChangeGameObjectActive(ptr, active);
    }

    public static Transform* GetTransform(this ref Component obj)
    {
        fixed (void* ptr = &obj)
            return GameInterop.GetGameObjectTransform(ptr);
    }

    [GameSymbol("UnityEngine.Transform$$get_position_Injected")]
    private static delegate* unmanaged<Transform*, System.Numerics.Vector3*, nint, void> transformGetPosition;
    [GameSymbol("UnityEngine.Transform$$get_rotation_Injected")]
    private static delegate* unmanaged<Transform*, System.Numerics.Quaternion*, nint, void> transformGetRotation;
    public static System.Numerics.Vector3 GetPosition(this ref Transform transform)
    {
        var v = new System.Numerics.Vector3();
        fixed (Transform* ptr = &transform)
            transformGetPosition(ptr, &v, 0);
        return v;
    }

    public static System.Numerics.Quaternion GetRotation(this ref Transform transform)
    {
        var q = new System.Numerics.Quaternion();
        fixed (Transform* ptr = &transform)
            transformGetRotation(ptr, &q, 0);
        return q;
    }

    [GameSymbol("UnityEngine.RectTransform$$get_rect")]
    private static delegate* unmanaged<RectTransform*, nint, Rect> getRect;
    [GameSymbol("Command.RectTransformExtensions$$GetRect", ReturnPointer = true)]
    private static delegate* unmanaged<System.Numerics.Vector4*, RectTransform*, nint, System.Numerics.Vector4*> rectTransformGetRect;
    public static System.Numerics.Vector3 GetPosition(this ref RectTransform transform)
    {
        var v = new System.Numerics.Vector3();
        fixed (RectTransform* ptr = &transform)
            transformGetPosition((Transform*)ptr, &v, 0);
        return v;
    }

    public static System.Numerics.Quaternion GetRotation(this ref RectTransform transform)
    {
        var q = new System.Numerics.Quaternion();
        fixed (RectTransform* ptr = &transform)
            transformGetRotation((Transform*)ptr, &q, 0);
        return q;
    }

    public static System.Numerics.Vector4 GetRect(this ref RectTransform transform)
    {
        var v = new System.Numerics.Vector4();
        fixed (RectTransform* ptr = &transform)
            rectTransformGetRect(&v, ptr, 0);
        return v;
    }

    public static System.Numerics.Vector2 GetSize(this ref RectTransform transform)
    {
        var v = new System.Numerics.Vector4();
        fixed (RectTransform* ptr = &transform)
            rectTransformGetRect(&v, ptr, 0);
        return new System.Numerics.Vector2(v.Z, v.W);
    }

    public static (System.Numerics.Vector2 pos, System.Numerics.Vector2 size) GetUIRect(this ref RectTransform transform)
    {
        var v = new System.Numerics.Vector4();
        fixed (RectTransform* ptr = &transform)
            rectTransformGetRect(&v, ptr, 0);
        return (new System.Numerics.Vector2(MathF.Round(v.X), MathF.Round(ImGui.GetMainViewport().Size.Y - v.Y - v.W)), new System.Numerics.Vector2(MathF.Round(v.Z), MathF.Round(v.W)));
    }

    public static (System.Numerics.Vector2 min, System.Numerics.Vector2 max) GetUIRectMinMax(this ref RectTransform transform)
    {
        var (pos, size) = transform.GetUIRect();
        return (pos, pos + size);
    }

    public static bool IsActive(this ref Canvas obj, bool inHierarchy = false)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr, inHierarchy);
    }

    public static void SetActive(this ref Canvas obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static void ChangeActive(this ref Canvas obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.ChangeGameObjectActive(ptr, active);
    }

    public static RectTransform* GetTransform(this ref Canvas obj)
    {
        fixed (void* ptr = &obj)
            return (RectTransform*)GameInterop.GetGameObjectTransform(ptr);
    }

    public static bool IsActive(this ref SingleTapButton obj, bool inHierarchy = false)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr, inHierarchy);
    }

    public static void SetActive(this ref SingleTapButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static void ChangeActive(this ref SingleTapButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.ChangeGameObjectActive(ptr, active);
    }

    public static RectTransform* GetTransform(this ref SingleTapButton obj)
    {
        fixed (void* ptr = &obj)
            return (RectTransform*)GameInterop.GetGameObjectTransform(ptr);
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

    public static bool IsActive(this ref TintButton obj, bool inHierarchy = false)
    {
        fixed (void* ptr = &obj)
            return GameInterop.IsGameObjectActive(ptr, inHierarchy);
    }

    public static void SetActive(this ref TintButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.SetGameObjectActive(ptr, active);
    }

    public static void ChangeActive(this ref TintButton obj, bool active)
    {
        fixed (void* ptr = &obj)
            GameInterop.ChangeGameObjectActive(ptr, active);
    }

    public static RectTransform* GetTransform(this ref TintButton obj)
    {
        fixed (void* ptr = &obj)
            return (RectTransform*)GameInterop.GetGameObjectTransform(ptr);
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

    public static Unmanaged_String* ToUnmanagedString(this string str) => GameInterop.CreateString(str);

    public static short GetToken(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask uniTask) => uniTask.token;

    public static UniTaskStatus GetStatus(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask uniTask)
    {
        if (uniTask.source == null) return UniTaskStatus.Succeeded;
        var invokeData = GameInterop.GetVirtualFunctionByInterface<IUniTaskSource>(uniTask.source, 0);
        return ((delegate* unmanaged<IUniTaskSource*, short, Il2CppMethodInfo*, UniTaskStatus>)invokeData->methodPtr)(uniTask.source, uniTask.GetToken(), invokeData->method);
    }

    public static void ContinueWith(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask uniTask, Action a)
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask* ptr = &uniTask)
            GameInterop.ContinueUniTaskWith(ptr, a);
    }

    internal static UniTask ToManagedUniTask(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask uniTask)
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask* ptr = &uniTask)
            return new UniTask(ptr);
    }

    public static short GetToken<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask) where T : unmanaged
    {
        var type = typeof(T);
        if (type.GetField(nameof(Il2CppObject.@class)) != null) return uniTask.token;
        var size = sizeof(T);
        var remainder = size % 2;
        if (remainder > 0)
            size += 2 - remainder;
        var offset = 8 + size;
        fixed (void* ptr = &uniTask)
            return *(short*)((nint)ptr + offset);
    }

    public static UniTaskStatus GetStatus<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask) where T : unmanaged
    {
        if (uniTask.source == null) return UniTaskStatus.Succeeded;
        var invokeData = GameInterop.GetVirtualFunctionByInterface<IUniTaskSource>(uniTask.source, 0);
        return ((delegate* unmanaged<IUniTaskSource*, short, Il2CppMethodInfo*, UniTaskStatus>)invokeData->methodPtr)((IUniTaskSource*)uniTask.source, uniTask.GetToken(), invokeData->method);
    }

    public static T GetResult<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask) where T : unmanaged
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>* ptr = &uniTask)
        {
            if (uniTask.source == null) return *(T*)&ptr->result;
            var invokeData = GameInterop.GetVirtualFunctionByName(uniTask.source, "GetResult");
            var result = ((delegate* unmanaged<void*, short, Il2CppMethodInfo*, nint>)invokeData->methodPtr)(uniTask.source, uniTask.GetToken(), invokeData->method);
            return *(T*)&result;
        }
    }

    public static T* GetResultPtr<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask) where T : unmanaged
    {
        if (uniTask.source == null) return uniTask.result;
        var invokeData = GameInterop.GetVirtualFunctionByName(uniTask.source, "GetResult");
        return (T*)((delegate* unmanaged<void*, short, Il2CppMethodInfo*, nint>)invokeData->methodPtr)(uniTask.source, uniTask.GetToken(), invokeData->method);
    }

    public static void ContinueWith<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask, Action<T> a) where T : unmanaged
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>* ptr = &uniTask)
            GameInterop.ContinueUniTaskWith(ptr, a);
    }

    public static void ContinueWithPtr<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask, Action<Ptr<T>> a) where T : unmanaged
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>* ptr = &uniTask)
            GameInterop.ContinuePtrUniTaskWith(ptr, a);
    }

    internal static UniTask<T> ToManagedUniTask<T>(this ref ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T> uniTask) where T : unmanaged
    {
        fixed (ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>* ptr = &uniTask)
            return new UniTask<T>(ptr);
    }
}