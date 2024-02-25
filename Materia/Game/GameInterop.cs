using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using ECGen.Generated;
using ECGen.Generated.Command;
using ECGen.Generated.Command.KeyInput;
using ECGen.Generated.Command.UI;
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
    internal static Il2CppClass* GetTypeInfo<T>() where T : unmanaged
    {
        var type = typeof(T);
        if (cachedTypeInfos.TryGetValue(type, out var typeInfo)) return (Il2CppClass*)typeInfo;
        var attribute = type.GetCustomAttribute<Il2CppTypeAttribute>();
        if (attribute != null)
            typeInfo = (nint)getTypeInfoFromInstance(0, attribute.InstanceId);
        cachedTypeInfos[type] = typeInfo;
        return (Il2CppClass*)typeInfo;
    }

    [Signature("E8 ?? ?? ?? ?? 48 8B 4C 24 ?? 48 8B D8 48 8B 51 20", ScanType = ScanType.Text, Required = true)] // E8 ?? ?? ?? ?? 48 8B D7 48 8B C8 48 8B D8 E8 ?? ?? ?? ??
    private static delegate* unmanaged<Il2CppClass*, void*> il2cppObjectNew;
    internal static void* NewIl2CppObject(Il2CppClass* @class) => il2cppObjectNew(@class);

    [Signature("0F B6 C2 4C 8B C9 45 33 C0", Required = true)]
    private static delegate* unmanaged<void*, byte, uint> il2cppGCHandleNew;
    internal static uint NewIl2CppGCHandle(void* ptr, bool pinned) => il2cppGCHandleNew(ptr, (byte)(pinned ? 1 : 0));

    [GameSymbol("System.Runtime.InteropServices.GCHandle$$FreeHandle", Required = true)]
    private static delegate* unmanaged<nint, nint, void> il2cppGCHandleFree;
    internal static void FreeIl2CppGCHandle(uint handle) => il2cppGCHandleFree((nint)handle, 0);

    [Signature("E8 ?? ?? ?? ?? 0F B6 F8 EB 0B", ScanType = ScanType.Text, Required = true)] // 48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 41 0F B6 D8 48 8B F2 48 8B F9 E8
    private static delegate* unmanaged<Il2CppClass*, Il2CppClass*, byte, byte> il2cppClassIsSubclassOf;
    internal static bool IsIl2CppClassSubclassOf(Il2CppClass* @class, Il2CppClass* otherClass) => il2cppClassIsSubclassOf(@class, otherClass, 0) != 0;

    [Signature("E8 ?? ?? ?? ?? 84 C0 75 2D 4C 8D 4C 24", ScanType = ScanType.Text, Required = true)]
    private static delegate* unmanaged<Il2CppClass*, Il2CppClass*, byte> il2cppClassIsAssignableFrom;
    internal static bool IsIl2CppClassAssignableFrom(Il2CppClass* @class, Il2CppClass* otherClass) => il2cppClassIsAssignableFrom(@class, otherClass) != 0;

    public static nint GetSharedMonoBehaviourInstance(string name, int symbolIndex = 0)
    {
        if (cachedInstanceStaticFields.TryGetValue(symbolIndex > 0 ? $"{name}_{symbolIndex}" : name, out var staticFields))
            return staticFields != nint.Zero ? (nint)((SharedMonoBehaviour<nint>.StaticFields*)staticFields)->instance : nint.Zero;

        if (!GameData.TryGetSymbolAddress(symbolIndex > 0 ? $"Method$Command.SharedMonoBehaviour<{name}>.get_Instance()_{symbolIndex}" : $"Method$Command.SharedMonoBehaviour<{name}>.get_Instance()", out var address))
        {
            cachedInstanceStaticFields[name] = nint.Zero;
            return nint.Zero;
        }

        address = *(nint*)address;
        if ((nuint)address <= uint.MaxValue) return nint.Zero;

        var @class = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)((Il2CppMethodInfo*)address)->parent;
        if ((@class->bitflags1 & 1) == 0) return nint.Zero;

        @class = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)@class->rgctx->Command_SharedMonoBehaviour_T_;
        if ((@class->bitflags1 & 1) == 0) return nint.Zero;

        staticFields = (nint)@class->staticFields;
        cachedInstanceStaticFields[symbolIndex > 0 ? $"{name}_{symbolIndex}" : name] = staticFields;
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

    public static void SendKey(ushort vKey, int ms = 100) => GameInteropHelpers.RunSendKeyTask(SendKeyInternal, vKey, ms);
    public static void SendKey(VirtualKey vKey, int ms = 100) => GameInteropHelpers.RunSendKeyTask(SendKeyInternal, (ushort)vKey, ms);

    [Signature("E8 ?? ?? ?? ?? 8B 47 04 48 83 C7 07", ScanType = ScanType.Text, Scanner = ScannerType.UnityPlayer)]
    private static delegate* unmanaged<void*, void*, byte, void> processRawInput;
    private static void SendKeyInternal(ushort vKey, bool down)
    {
        var data = new RawInput.RawKeyboardData
        {
            makeCode = (ushort)User32.MapVirtualKey(vKey, User32.MapVirtualKeyTranslation.MAPVK_VK_TO_VSC),
            flags = (ushort)(down ? 0 : 1),
            vKey = vKey
        };
        var type = 1u;
        processRawInput(&type, &data, 1);
    }

    [GameSymbol("Command.UI.SingleTapButton$$IsInputBlocked")]
    private static delegate* unmanaged<SingleTapButton*, nint, bool> isInputBlocked;
    public static bool CanTapButton(SingleTapButton* singleTapButton) => singleTapButton->steamKeyMapIsActive && !isInputBlocked(singleTapButton, 0) && ScreenManager.Instance is not { IsBlocking: true };
    public static bool CanTapButton(TintButton* button) => CanTapButton((SingleTapButton*)button);

    [GameSymbol("Command.UI.SingleTapButton$$ForceTapSteamUICursor")]
    private static delegate* unmanaged<SingleTapButton*, nint, void> forceTapSteamUICursor;
    public static bool TapButton(SingleTapButton* singleTapButton, uint lockoutMs = 2000, uint delayMS = 0) // TODO: Ensure the button still exists when delayed
    {
        if (singleTapButton == null
            || (lastPressedButtons.TryGetValue(((nint)singleTapButton, (nint)singleTapButton->steamUICursorTapSubject), out var timestampMs)
                && timestampMs > DateTimeOffset.Now.ToUnixTimeMilliseconds())
            || !CanTapButton(singleTapButton))
            return false;

        if (lockoutMs > 0 || delayMS > 0)
            lastPressedButtons[((nint)singleTapButton, (nint)singleTapButton->steamUICursorTapSubject)] = DateTimeOffset.Now.ToUnixTimeMilliseconds() + lockoutMs + delayMS;

        if (delayMS > 0)
            RunOnUpdate(() => forceTapSteamUICursor(singleTapButton, 0), delayMS);
        else
            RunOnUpdate(() => forceTapSteamUICursor(singleTapButton, 0));
        return true;
    }

    public static bool TapButton(TintButton* button, uint lockoutMs = 2000, uint delayMS = 0) => TapButton((SingleTapButton*)button, lockoutMs, delayMS);

    public static bool TapKeyAction(KeyAction keyAction, uint lockoutMs = 2000, uint delayMS = 0)
    {
        var ret = false;
        if (GetSingletonInstance<KeyMapManager>() is var keyMapManager && (keyMapManager == null || keyMapManager->keyMaps->size == 0)) return ret;

        var keyMap = keyMapManager->keyMaps->GetPtr(keyMapManager->keyMaps->size - 1);
        for (int i = 0; i < keyMap->keyHandlers->size; i++)
        {
            if (!Il2CppType<SingleTapButton>.Is(keyMap->keyHandlers->GetPtr(i), out var singleTapButton)) continue;
            var buttonKeyAction = singleTapButton->steamKeyAction != KeyAction.None ? singleTapButton->steamKeyAction : singleTapButton->steamKeyActionDefault;
            if (buttonKeyAction == keyAction || buttonKeyAction == KeyAction.Any)
                ret |= TapButton(singleTapButton, lockoutMs, delayMS);
        }
        return ret;
    }

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