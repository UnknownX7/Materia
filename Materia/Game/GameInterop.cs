using ECGen.Generated;
using ECGen.Generated.Command;
using PInvoke;
using Materia.Attributes;

namespace Materia.Game;

[Injection]
public static unsafe class GameInterop
{
    private static readonly Dictionary<Type, (string, string)> cachedTypeNames = new();
    private static readonly Dictionary<string, nint> cachedInstanceStaticFields = new();

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

        var klass = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)((Il2CppMethodInfo*)address)->klass;
        if ((klass->bitflags1 & 1) == 0) return nint.Zero;

        klass = (Il2CppClass<SharedMonoBehaviour<nint>.StaticFields, SharedMonoBehaviour<nint>.RGCTXs, SharedMonoBehaviour<nint>.VirtualTable>*)klass->rgctx_data->Command_SharedMonoBehaviour_T_;
        if ((klass->bitflags1 & 1) == 0) return nint.Zero;

        staticFields = (nint)klass->static_fields;
        cachedInstanceStaticFields[symbolIndex > 0 ? $"{name}_{symbolIndex}" : name] = staticFields;
        return staticFields != nint.Zero ? (nint)klass->static_fields->instance : nint.Zero;
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
    public static T* GetSingletonInstance<T>(int symbolIndex = 0) where T : unmanaged => (T*)GetSingletonInstance(GetCachedTypeName<T>().name, symbolIndex);

    public static Il2CppClass* GetClass(string name) // TODO: Rework
    {
        GameData.TryGetSymbolAddress($"{name}_TypeInfo", out var address);
        return address != nint.Zero ? *(Il2CppClass**)address : null;
    }

    public static Il2CppClass* GetClass<T>() where T : unmanaged => GetClass(GetCachedTypeName<T>().fullName);

    public static string GetTypeName<T>() where T : unmanaged => GetCachedTypeName<T>().fullName;

    private static (string fullName, string name) GetCachedTypeName<T>() where T : unmanaged
    {
        var type = typeof(T);
        if (cachedTypeNames.TryGetValue(type, out var t)) return t;
        var fullName = type.Name.Replace('_', '.');
        t = (fullName, fullName.Split('.').Last());
        cachedTypeNames[type] = t;
        return t;
    }

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