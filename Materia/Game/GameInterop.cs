using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using ECGen.Generated;
using ECGen.Generated.Command;
using ECGen.Generated.Command.KeyInput;
using ECGen.Generated.Command.UI;
using ECGen.Generated.UnityEngine;
using PInvoke;
using Materia.Attributes;

namespace Materia.Game;

[Injection]
public static unsafe class GameInterop
{
    public static bool IsInUpdateThread => Environment.CurrentManagedThreadId == Materia.CurrentUpdateThreadId;
    private static readonly ConcurrentDictionary<Type, nint> cachedTypeInfos = new();
    private static readonly ConcurrentDictionary<string, nint> cachedInstanceStaticFields = new();
    private static readonly ConcurrentDictionary<(nint, nint), long> lastPressedButtons = new();
    private static readonly ConcurrentQueue<Action> onUpdate = new();
    private static readonly List<(Action, Stopwatch, TimeSpan)> delayedOnUpdate = new();

    //[Signature("E8 ?? ?? ?? ?? 48 63 CB 48 8D 55 08", ScanType = ScanType.Text)]
    //private static delegate* unmanaged<int, Il2CppClass*> getTypeInfo;

    [Signature("E8 ?? ?? ?? ?? 48 8B C8 48 89 07", ScanType = ScanType.Text, Required = true)] // 8B CA B2 01 E9
    private static delegate* unmanaged<nint, int, Il2CppClass*> getTypeInfoFromInstance;
    public static Il2CppClass* GetTypeInfo(int id) => getTypeInfoFromInstance(0, id);
    public static Il2CppClass* GetTypeInfo<T>() where T : unmanaged
    {
        var type = typeof(T);
        if (cachedTypeInfos.TryGetValue(type, out var typeInfo)) return (Il2CppClass*)typeInfo;
        var attribute = type.GetCustomAttribute<Il2CppTypeAttribute>();
        if (attribute != null)
            typeInfo = (nint)getTypeInfoFromInstance(0, attribute.InstanceId);
        cachedTypeInfos[type] = typeInfo;
        return (Il2CppClass*)typeInfo;
    }

    public static Il2CppClass* GetGenericTypeInfo(Il2CppClass* @class) => @class->genericClass != null ? GetTypeInfo((*@class->genericClass->typeDefinition)->byValTypeIndex) : null; // TODO: Causes an access violation on some types occasionally

    [Signature("E8 ?? ?? ?? ?? 48 8B 4C 24 ?? 48 8B D8 48 8B 51 20", ScanType = ScanType.Text, Required = true)] // E8 ?? ?? ?? ?? 48 8B D7 48 8B C8 48 8B D8 E8 ?? ?? ?? ??
    private static delegate* unmanaged<Il2CppClass*, void*> il2cppObjectNew;
    internal static void* NewIl2CppObject(Il2CppClass* @class) => il2cppObjectNew(@class);

    [Signature("0F B6 C2 4C 8B C9 45 33 C0", Required = true)]
    private static delegate* unmanaged<void*, CBool, uint> il2cppGCHandleNew;
    internal static uint NewIl2CppGCHandle(void* ptr, bool pinned) => il2cppGCHandleNew(ptr, pinned);

    [GameSymbol("System.Runtime.InteropServices.GCHandle$$FreeHandle", Required = true)]
    private static delegate* unmanaged<nint, nint, void> il2cppGCHandleFree;
    internal static void FreeIl2CppGCHandle(uint handle) => il2cppGCHandleFree((nint)handle, 0);

    [Signature("E8 ?? ?? ?? ?? 0F B6 F8 EB 0B", ScanType = ScanType.Text, Required = true)] // 48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 41 0F B6 D8 48 8B F2 48 8B F9 E8
    private static delegate* unmanaged<Il2CppClass*, Il2CppClass*, CBool, CBool> il2cppClassIsSubclassOf;
    internal static bool IsIl2CppClassSubclassOf(Il2CppClass* @class, Il2CppClass* otherClass) => il2cppClassIsSubclassOf(@class, otherClass, false);

    [Signature("E8 ?? ?? ?? ?? 84 C0 75 2D 4C 8D 4C 24", ScanType = ScanType.Text, Required = true)]
    private static delegate* unmanaged<Il2CppClass*, Il2CppClass*, CBool> il2cppClassIsAssignableFrom;
    internal static bool IsIl2CppClassAssignableFrom(Il2CppClass* @class, Il2CppClass* otherClass) => il2cppClassIsAssignableFrom(@class, otherClass);

    [GameSymbol("System.String$$Ctor_3", Required = true)]
    private static delegate* unmanaged<char*, int, int, nint, Unmanaged_String*> stringCtor;
    public static Unmanaged_String* CreateString(string str)
    {
        var array = str.ToCharArray();
        fixed (char* ptr = array)
            return stringCtor(ptr, 0, array.Length, 0);
    }

    public static nint GetSharedMonoBehaviourInstance(string name, int symbolIndex = 0)
    {
        var key = symbolIndex > 0 ? $"{name}_{symbolIndex}" : name;
        if (cachedInstanceStaticFields.TryGetValue(key, out var staticFields))
            return staticFields != nint.Zero ? (nint)((SharedMonoBehaviour<nint>.StaticFields*)staticFields)->instance : nint.Zero;

        if (!GameData.TryGetSymbolAddress(symbolIndex > 0 ? $"Method$Command.SharedMonoBehaviour<{name}>.get_Instance()_{symbolIndex}" : $"Method$Command.SharedMonoBehaviour<{name}>.get_Instance()", out var address))
        {
            cachedInstanceStaticFields[key] = nint.Zero;
            return nint.Zero;
        }

        address = *(nint*)address;
        if ((nuint)address <= uint.MaxValue) return nint.Zero;

        var @class = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)((Il2CppMethodInfo*)address)->parent;
        if ((@class->bitflags1 & 1) == 0) return nint.Zero;

        @class = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)@class->rgctx->Command_SharedMonoBehaviour_T_;
        if ((@class->bitflags1 & 1) == 0) return nint.Zero;

        staticFields = (nint)@class->staticFields;
        cachedInstanceStaticFields[key] = staticFields;
        return staticFields != nint.Zero ? (nint)@class->staticFields->instance : nint.Zero;
    }

    public static T* GetSharedMonoBehaviourInstance<T>(string name, int symbolIndex = 0) where T : unmanaged => (T*)GetSharedMonoBehaviourInstance(name, symbolIndex);
    public static T* GetSharedMonoBehaviourInstance<T>(int symbolIndex = 0) where T : unmanaged => (T*)GetSharedMonoBehaviourInstance(typeof(T).Name, symbolIndex);

    [GameSymbol("Singleton<object>$$get_Instance", Required = true)]
    private static delegate* unmanaged<nint, nint> singletonGetInstance;
    public static nint GetSingletonInstance(string name, int symbolIndex = 0)
    {
        if (!GameData.TryGetSymbolAddress(symbolIndex > 0 ? $"Method$Singleton<{name}>.get_Instance()_{symbolIndex}" : $"Method$Singleton<{name}>.get_Instance()", out var address)) return nint.Zero;
        address = *(nint*)address;
        return (nuint)address <= uint.MaxValue ? nint.Zero : singletonGetInstance(address);
    }

    public static T* GetSingletonInstance<T>(string name, int symbolIndex = 0) where T : unmanaged => (T*)GetSingletonInstance(name, symbolIndex);
    public static T* GetSingletonInstance<T>(int symbolIndex = 0) where T : unmanaged => (T*)GetSingletonInstance(typeof(T).Name, symbolIndex);

    [GameSymbol("SingletonMonoBehaviour<object>$$get_Instance", Required = true)]
    private static delegate* unmanaged<nint, nint> singletonMonoBehaviourGetInstance;
    public static nint GetSingletonMonoBehaviourInstance(string name, int symbolIndex = 0)
    {
        if (!GameData.TryGetSymbolAddress(symbolIndex > 0 ? $"Method$SingletonMonoBehaviour<{name}>.get_Instance()_{symbolIndex}" : $"Method$SingletonMonoBehaviour<{name}>.get_Instance()", out var address)) return nint.Zero;
        address = *(nint*)address;
        return (nuint)address <= uint.MaxValue ? nint.Zero : singletonMonoBehaviourGetInstance(address);
    }

    public static T* GetSingletonMonoBehaviourInstance<T>(string name, int symbolIndex = 0) where T : unmanaged => (T*)GetSingletonMonoBehaviourInstance(name, symbolIndex);
    public static T* GetSingletonMonoBehaviourInstance<T>(int symbolIndex = 0) where T : unmanaged => (T*)GetSingletonMonoBehaviourInstance(typeof(T).Name, symbolIndex);

    [GameSymbol("UnityEngine.GameObject$$get_activeSelf")]
    private static delegate* unmanaged<GameObject*, nint, CBool> gameObjectGetActive;
    [GameSymbol("UnityEngine.GameObject$$get_activeInHierarchy")]
    private static delegate* unmanaged<GameObject*, nint, CBool> gameObjectGetActiveInHierarchy;
    [GameSymbol("UnityEngine.Component$$get_gameObject")]
    private static delegate* unmanaged<Component*, nint, GameObject*> componentGetGameObject;
    public static bool IsGameObjectActive(void* obj, bool inHierarchy = false)
    {
        if (obj == null || ((GameObject*)obj)->m_CachedPtr == nint.Zero) return false;
        if (Il2CppType<GameObject>.IsAssignableFrom(obj))
            return inHierarchy ? gameObjectGetActiveInHierarchy((GameObject*)obj, 0) : gameObjectGetActive((GameObject*)obj, 0);
        if (Il2CppType<Component>.IsAssignableFrom(obj))
            return inHierarchy ? gameObjectGetActiveInHierarchy(componentGetGameObject((Component*)obj, 0), 0) : gameObjectGetActive(componentGetGameObject((Component*)obj, 0), 0);
        return false;
    }

    [GameSymbol("UnityEngine.GameObject$$SetActive")]
    private static delegate* unmanaged<GameObject*, CBool, nint, void> gameObjectSetActive;
    [GameSymbol("Command.Extensions$$SetActive")]
    private static delegate* unmanaged<Component*, CBool, nint, void> componentSetActive;
    public static void SetGameObjectActive(void* obj, bool active)
    {
        if (obj == null || ((GameObject*)obj)->m_CachedPtr == nint.Zero) return;
        if (Il2CppType<GameObject>.IsAssignableFrom(obj))
            RunOnUpdate(() => gameObjectSetActive((GameObject*)obj, active, 0));
        else if (Il2CppType<Component>.IsAssignableFrom(obj))
            RunOnUpdate(() => componentSetActive((Component*)obj, active, 0));
    }

    public static void ChangeGameObjectActive(void* obj, bool active)
    {
        if (obj == null || ((GameObject*)obj)->m_CachedPtr == nint.Zero) return;
        if (Il2CppType<GameObject>.IsAssignableFrom(obj))
        {
            RunOnUpdate(() =>
            {
                if (gameObjectGetActive((GameObject*)obj, 0) != active)
                    gameObjectSetActive((GameObject*)obj, active, 0);
            });
        }
        else if (Il2CppType<Component>.IsAssignableFrom(obj))
        {
            RunOnUpdate(() =>
            {
                var gameObject = componentGetGameObject((Component*)obj, 0);
                if (gameObjectGetActive(gameObject, 0) != active)
                    gameObjectSetActive(gameObject, active, 0);
            });
        }
    }

    [GameSymbol("UnityEngine.GameObject$$get_tag")]
    private static delegate* unmanaged<GameObject*, nint, Unmanaged_String*> gameObjectGetTag;
    public static string? GetGameObjectTag(void* obj)
    {
        if (obj == null || ((GameObject*)obj)->m_CachedPtr == nint.Zero) return null;
        if (Il2CppType<GameObject>.IsAssignableFrom(obj))
            return gameObjectGetTag((GameObject*)obj, 0)->ToString();
        if (Il2CppType<Component>.IsAssignableFrom(obj))
            return gameObjectGetTag(componentGetGameObject((Component*)obj, 0), 0)->ToString();
        return null;
    }

    [GameSymbol("UnityEngine.GameObject$$get_transform")]
    private static delegate* unmanaged<GameObject*, nint, Transform*> gameObjectGetTransform;
    public static Transform* GetGameObjectTransform(void* obj)
    {
        if (obj == null || ((GameObject*)obj)->m_CachedPtr == nint.Zero) return null;
        if (Il2CppType<GameObject>.IsAssignableFrom(obj))
            return gameObjectGetTransform((GameObject*)obj, 0);
        if (Il2CppType<Component>.IsAssignableFrom(obj))
            return gameObjectGetTransform(componentGetGameObject((Component*)obj, 0), 0);
        return null;
    }

    public static void SendKey(ushort vKey, int ms = 100) => GameInteropHelpers.RunSendKeyTask(SendKeyInternal, vKey, ms);
    public static void SendKey(VirtualKey vKey, int ms = 100) => GameInteropHelpers.RunSendKeyTask(SendKeyInternal, (ushort)vKey, ms);

    [Signature("E8 ?? ?? ?? ?? 8B 47 04 48 83 C7 07", ScanType = ScanType.Text, Scanner = ScannerType.UnityPlayer)]
    private static delegate* unmanaged<void*, void*, CBool, void> processRawInput;
    private static void SendKeyInternal(ushort vKey, bool down)
    {
        var data = new RawInput.RawKeyboardData
        {
            makeCode = (ushort)User32.MapVirtualKey(vKey, User32.MapVirtualKeyTranslation.MAPVK_VK_TO_VSC),
            flags = (ushort)(down ? 0 : 1),
            vKey = vKey
        };
        var type = 1u;
        processRawInput(&type, &data, true);
    }

    [GameSymbol("Command.UI.SingleTapButton$$IsInputBlocked")]
    private static delegate* unmanaged<SingleTapButton*, nint, CBool> isInputBlocked;
    public static bool CanTapButton(SingleTapButton* singleTapButton, bool checkActive) => singleTapButton->canTap
        && (!checkActive || IsGameObjectActive(singleTapButton))
        && !isInputBlocked(singleTapButton, 0)
        && ScreenManager.Instance is not { IsBlocking: true };
    public static bool CanTapButton(TintButton* tintButton, bool checkActive) => CanTapButton((SingleTapButton*)tintButton, checkActive);

    [GameSymbol("Command.UI.SingleTapButton$$ForceTapSteamUICursor")]
    private static delegate* unmanaged<SingleTapButton*, nint, void> forceTapSteamUICursor;
    public static bool TapButton(SingleTapButton* singleTapButton, bool checkActive = true, uint lockoutMs = 2000, uint delayMS = 0) // TODO: Ensure the button still exists when delayed
    {
        if (singleTapButton == null
            || (lastPressedButtons.TryGetValue(((nint)singleTapButton, (nint)singleTapButton->steamUICursorTapSubject), out var timestampMs)
                && timestampMs > DateTimeOffset.Now.ToUnixTimeMilliseconds())
            || !CanTapButton(singleTapButton, checkActive))
            return false;

        if (lockoutMs > 0 || delayMS > 0)
            lastPressedButtons[((nint)singleTapButton, (nint)singleTapButton->steamUICursorTapSubject)] = DateTimeOffset.Now.ToUnixTimeMilliseconds() + lockoutMs + delayMS;

        if (delayMS > 0)
            RunOnUpdate(() =>
            {
                if (CanTapButton(singleTapButton, checkActive))
                    forceTapSteamUICursor(singleTapButton, 0);
            }, delayMS);
        else
            RunOnUpdate(() => forceTapSteamUICursor(singleTapButton, 0));
        return true;
    }

    public static bool TapButton(TintButton* tintButton, bool checkActive = true, uint lockoutMs = 2000, uint delayMS = 0) => TapButton((SingleTapButton*)tintButton, checkActive, lockoutMs, delayMS);

    public static bool TapKeyAction(KeyAction keyAction, bool checkActive = true, uint lockoutMs = 2000, uint delayMS = 0)
    {
        var ret = false;
        if (GetSingletonInstance<KeyMapManager>() is var keyMapManager && (keyMapManager == null || keyMapManager->keyMaps->size == 0)) return ret;

        var keyMap = keyMapManager->keyMaps->GetPtr(keyMapManager->keyMaps->size - 1);
        for (int i = 0; i < keyMap->keyHandlers->size; i++)
        {
            if (!Il2CppType<SingleTapButton>.Is(keyMap->keyHandlers->GetPtr(i), out var singleTapButton)) continue;
            var buttonKeyAction = singleTapButton->steamKeyAction != KeyAction.None ? singleTapButton->steamKeyAction : singleTapButton->steamKeyActionDefault;
            if (buttonKeyAction == keyAction || buttonKeyAction == KeyAction.Any)
                ret |= TapButton(singleTapButton, checkActive, lockoutMs, delayMS);
        }
        return ret;
    }

    private static readonly Dictionary<long, string> localizationCache = [];
    [GameSymbol("Command.UI.LocalizeExtensions$$Get", Required = true)]
    private static delegate* unmanaged<LocalizeTextCategory, long, nint, Unmanaged_String*> getLocalizedText;
    public static string GetLocalizedText(LocalizeTextCategory category, long id)
    {
        if (localizationCache.TryGetValue(id, out var loc)) return loc;
        loc = getLocalizedText(category, id, 0)->ToString();
        if (loc != string.Empty)
            localizationCache[id] = loc;
        return loc;
    }

    public static string GetLocalizedText(long id) => localizationCache.TryGetValue(id, out var loc)
        ? loc
        : Enum.GetValues<LocalizeTextCategory>().Skip(1).Select(category => GetLocalizedText(category, id)).FirstOrDefault(str => str != string.Empty) ?? string.Empty;

    public static void RunOnUpdate(Action action)
    {
        if (IsInUpdateThread)
            action.Invoke();
        else
            onUpdate.Enqueue(action);
    }

    public static void RunOnUpdate(Action action, TimeSpan delay)
    {
        lock (delayedOnUpdate)
            delayedOnUpdate.Add((action, Stopwatch.StartNew(), delay));
    }

    public static void RunOnUpdate(Action action, uint delayMs) => RunOnUpdate(action, TimeSpan.FromMilliseconds(delayMs));

    internal static void Update()
    {
        while (onUpdate.TryDequeue(out var action))
            action.Invoke();

        lock (delayedOnUpdate)
        {
            for (int i = 0; i < delayedOnUpdate.Count; i++)
            {
                var (action, stopwatch, delay) = delayedOnUpdate[i];
                if (stopwatch.Elapsed < delay) continue;
                action.Invoke();
                delayedOnUpdate.RemoveAt(i--);
            }
        }
    }
}

internal static class GameInteropHelpers
{
    private static readonly Dictionary<ushort, Task> runningTasks = new();
    public static void RunSendKeyTask(Action<ushort, bool> sendKey, ushort vKey, int ms)
    {
        if (runningTasks.TryGetValue(vKey, out var task) && !task.IsCompleted) return;
        runningTasks[vKey] = Task.Run(async () =>
        {
            sendKey(vKey, true);
            await Task.Delay(ms);
            sendKey(vKey, false);
            await Task.Delay(Math.Min(ms, 100));
        });
    }
}