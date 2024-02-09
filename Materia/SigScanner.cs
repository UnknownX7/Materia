using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Iced.Intel;
using Materia.Attributes;
using Materia.Game;
using Materia.Utilities;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Definitions;

namespace Materia;

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
internal unsafe struct SectionHeader
{
    public fixed byte name[8];
    public int virtualSize;
    public int virtualOffset;
    public int rawDataSize;
    public int rawDataOffset;
    public int relocationOffset;
    public int lineNumbersOffset;
    public short relocationCount;
    public short lineNumberCount;
    public uint characteristics;
}

public sealed unsafe class SigScanner : IDisposable
{
    public static SigScanner Exe { get; } = new(Process.GetCurrentProcess().MainModule!);
    public static SigScanner GameAssembly { get; } = new("GameAssembly.dll");
    public static SigScanner NtDll { get; } = new("ntdll.dll");
    public static SigScanner UnityPlayer { get; } = new("UnityPlayer.dll");

    private readonly Scanner scanner;
    private readonly nint srcAddress;
    private readonly nint copyAddress;

    public nint BaseAddress { get; }

    public SigScanner(ProcessModule module, bool copy = true)
    {
        var baseAddress = module.BaseAddress;
        var size = 0;
        if (module.ModuleName == "GameAssembly.dll")
        {
            var text = GetSectionHeader(module, ".text");
            var il2cpp = GetSectionHeader(module, "il2cpp");
            if (text != null && il2cpp != null)
            {
                size = il2cpp->virtualSize + il2cpp->virtualOffset - text->virtualOffset;
                srcAddress = baseAddress + text->virtualOffset;
            }
        }
        else
        {
            var text = GetSectionHeader(module, ".text");
            if (text != null)
            {
                size = text->virtualSize;
                srcAddress = baseAddress + text->virtualOffset;
            }
        }

        if (srcAddress == nint.Zero)
            throw new ApplicationException($"Could not find {module.ModuleName} .text section");

        if (copy)
        {
            copyAddress = Marshal.AllocHGlobal(size);
            Buffer.MemoryCopy(srcAddress.ToPointer(), copyAddress.ToPointer(), size, size);
            scanner = new Scanner((byte*)copyAddress, size);
        }
        else
        {
            scanner = new Scanner((byte*)srcAddress, size);
        }

        BaseAddress = baseAddress;
    }

    public SigScanner(string moduleName, bool copy = true) : this(Util.GetProcessModule(moduleName)!) { }

    private static SectionHeader* GetSectionHeader(ProcessModule module, string name)
    {
        var baseAddress = module.BaseAddress;
        var ntHeader = baseAddress + *(int*)(baseAddress + 0x3C);
        var sectionCount = *(short*)(ntHeader + 0x6);
        var firstSectionHeader = (SectionHeader*)(ntHeader + 0x18 + 0xF0);

        for (int i = 0; i < sectionCount; i++)
        {
            var header = firstSectionHeader + i;
            if (name == Marshal.PtrToStringAnsi((nint)header->name)) return header;
        }

        return null;
    }

    private static int ScanOffset(IScanner scanner, string sig)
    {
        var offset = scanner.FindPattern(sig).Offset;
        if (offset < 0)
            throw new ApplicationException($"Signature \"{sig}\" was not found");
        return offset;
    }

    private static bool TryScanOffset(IScanner scanner, string sig, out int offset)
    {
        offset = scanner.FindPattern(sig).Offset;
        return offset >= 0;
    }

    public nint Scan(string sig) => srcAddress + ScanOffset(scanner, sig);

    public static nint Scan(nint address, int length, string sig)
    {
        using var scanner = new Scanner((byte*)address, length);
        return address + ScanOffset(scanner, sig);
    }

    public bool TryScan(string sig, out nint result)
    {
        if (!TryScanOffset(scanner, sig, out var offset))
        {
            result = nint.Zero;
            return false;
        }

        result = srcAddress + offset;
        return true;
    }

    public static bool TryScan(nint address, int length, string sig, out nint result)
    {
        using var scanner = new Scanner((byte*)address, length);
        if (!TryScanOffset(scanner, sig, out var offset))
        {
            result = nint.Zero;
            return false;
        }

        result = address + offset;
        return true;
    }

    public nint ScanText(string sig)
    {
        var offset = ScanOffset(scanner, sig);
        return srcAddress + offset + GetJumpOffset((copyAddress != nint.Zero ? copyAddress : srcAddress) + offset);
    }

    public bool TryScanText(string sig, out nint result)
    {
        if (!TryScanOffset(scanner, sig, out var offset))
        {
            result = nint.Zero;
            return false;
        }

        result = srcAddress + offset + GetJumpOffset((copyAddress != nint.Zero ? copyAddress : srcAddress) + offset);
        return true;
    }

    private static int GetJumpOffset(nint address) => *(byte*)address switch
    {
        0xE8 => *(int*)(address + 1) + 5,
        0xE9 => *(int*)(address + 1) + 5,
        _ => 0
    };

    public static nint GetStaticAddress(nint address)
    {
        using var ms = new UnmanagedMemoryStream((byte*)address, 8);
        var reader = new StreamCodeReader(ms);
        var decoder = Decoder.Create(64, reader, (ulong)address, DecoderOptions.AMD);
        var instruction = decoder.Decode();
        if (instruction.IsInvalid || (instruction.Op0Kind != OpKind.Memory && instruction.Op1Kind != OpKind.Memory))
            throw new ApplicationException($"Unable to find static address at {address:X}");
        return (nint)instruction.MemoryDisplacement64;
    }

    public nint ScanStatic(string signature, int offset = 0) => GetStaticAddress(ScanText(signature) + offset);

    public bool TryScanStatic(string signature, int offset, out nint result)
    {
        try
        {
            result = GetStaticAddress(ScanText(signature) + offset);
            return true;
        }
        catch
        {
            result = nint.Zero;
            return false;
        }
    }

    public static void InjectAttributes(Assembly assembly, HookManager hookManager)
    {
        foreach (var (type, _) in assembly!.GetTypesWithAttribute<InjectionAttribute>())
            Inject(type, hookManager);
    }

    private static void Inject(Type type, HookManager hookManager)
    {
        foreach (var memberInfo in type.GetAllMembers().Where(memberInfo => memberInfo.MemberType is MemberTypes.Field or MemberTypes.Property))
            InjectMember(memberInfo, hookManager);
    }

    private static void InjectMember(MemberInfo memberInfo, HookManager hookManager)
    {
        var attribute = memberInfo.GetCustomAttribute<MemberInjectionAttribute>();
        if (attribute == null) return;

        switch (attribute)
        {
            case SignatureAttribute sigAttribute:
                InjectSignature(memberInfo, sigAttribute, hookManager);
                break;
            case GameSymbolAttribute symAttribute:
                InjectGameSymbol(memberInfo, symAttribute, hookManager);
                break;
        }
    }

    private static void InjectSignature(MemberInfo memberInfo, SignatureAttribute attribute, HookManager hookManager)
    {
        var scanner = attribute.Scanner switch
        {
            _ when attribute.CustomScanner != null => CreateCustomScanner(attribute.CustomScanner),
            ScannerType.Exe => Exe,
            ScannerType.GameAssembly => GameAssembly,
            ScannerType.NtDll => NtDll,
            ScannerType.UnityPlayer => UnityPlayer,
            _ => GameAssembly
        };

        if (scanner == null)
        {
            LogInjectError(memberInfo, $"CustomScanner \"{attribute.CustomScanner}\" was not found", attribute.Required);
            return;
        }

        var address = nint.Zero;
        var succeeded = attribute.ScanType switch
        {
            ScanType.Default => scanner.TryScan(attribute.Signature, out address),
            ScanType.Text => scanner.TryScanText(attribute.Signature, out address),
            ScanType.Static => scanner.TryScanStatic(attribute.Signature, attribute.StaticOffset, out address),
            _ => false
        };

        if (!succeeded)
        {
            LogInjectError(memberInfo, $"Injection failed for signature \"{attribute.Signature}\" (Type: {attribute.ScanType})", attribute.Required);
            return;
        }

        InjectAddress(memberInfo, address, attribute, hookManager);
    }

    private static readonly Dictionary<string, SigScanner?> cachedCustomScanners = new();
    private static SigScanner? CreateCustomScanner(string? name)
    {
        if (name == null) return null;
        if (cachedCustomScanners.TryGetValue(name, out var sigScanner)) return sigScanner;

        try
        {
            sigScanner = new(name);
        }
        catch
        {
            sigScanner = null;
        }

        cachedCustomScanners[name] = sigScanner;
        return sigScanner;
    }

    private static void InjectGameSymbol(MemberInfo memberInfo, GameSymbolAttribute attribute, HookManager hookManager)
    {
        var succeeded = GameData.TryGetSymbolAddress(attribute.Symbol, out var address);
        if (succeeded && attribute.ScanType == ScanType.Static)
        {
            try
            {
                address = GetStaticAddress(address + attribute.StaticOffset);
            }
            catch
            {
                succeeded = false;
            }
        }

        if (!succeeded)
        {
            LogInjectError(memberInfo, $"Injection failed for symbol \"{attribute.Symbol}\" (Type: {attribute.ScanType})", attribute.Required);
            return;
        }

        InjectAddress(memberInfo, address, attribute, hookManager);
    }

    private static void InjectAddress(MemberInfo memberInfo, nint address, MemberInjectionAttribute attribute, HookManager hookManager)
    {
        address += attribute.Offset;
        var type = memberInfo.GetObjectType()!;
        if (type == typeof(nint) || type.IsPointer)
            memberInfo.SetValue(null, address);
        else if (type.IsAssignableTo(typeof(Delegate)))
            memberInfo.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, type));
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMateriaHook<>))
            InjectHook(memberInfo, address, attribute, hookManager);
        else if (type.IsPrimitive)
            memberInfo.SetValue(null, Marshal.PtrToStructure(address, type));
        else
            LogInjectError(memberInfo, "Failed to determine how to inject member", attribute.Required);
    }

    private static void InjectHook(MemberInfo memberInfo, nint address, MemberInjectionAttribute attribute, HookManager hookManager)
    {
        var ownerType = memberInfo.ReflectedType!;
        var type = memberInfo.GetObjectType()!;
        var hookDelegateType = type.GenericTypeArguments[0];

        if (attribute is GameSymbolAttribute symAttribute && !ValidateDelegate(hookDelegateType, symAttribute.Symbol, symAttribute.ReturnPointer))
        {
            LogInjectError(memberInfo, $"Symbol \"{symAttribute.Symbol}\" ({GetSymbolTypeSignature(symAttribute.Symbol, symAttribute.ReturnPointer)}) does not match delegate \"{hookDelegateType.Name}\" ({GetTypeSignature(hookDelegateType)})", attribute.Required);
            return;
        }

        var detour = GetMethodDelegate(ownerType, hookDelegateType, null, memberInfo.Name.Replace("Hook", "Detour"));

        if (detour == null)
        {
            var detourName = attribute.DetourName;
            if (detourName != null)
            {
                detour = GetMethodDelegate(ownerType, hookDelegateType, null, detourName);
                if (detour == null)
                {
                    LogInjectError(memberInfo, $"Detour not found or was incompatible with delegate \"{detourName}\" {hookDelegateType.Name}", attribute.Required);
                    return;
                }
            }
            else
            {
                var matches = GetMethodDelegates(ownerType, hookDelegateType, null);
                if (matches.Length != 1)
                {
                    LogInjectError(memberInfo, $"Found {matches.Length} matching detours: specify a detour name", attribute.Required);
                    return;
                }

                detour = matches[0]!;
            }
        }

        var createHookMethod = typeof(HookManager).GetMethod(nameof(HookManager.CreateHook))!.MakeGenericMethod(hookDelegateType);
        var hook = (IMateriaHook)createHookMethod.Invoke(hookManager, new object?[] { address, detour })!;
        memberInfo.SetValue(null, hook);

        if (attribute.EnableHook)
            hook.Enable();
    }

    private static readonly Dictionary<Type, char> typeChars = new()
    {
        [typeof(void)] = 'v',
        [typeof(long)] = 'j',
        [typeof(ulong)] = 'j',
        [typeof(float)] = 'f',
        [typeof(double)] = 'd'
    };
    private static string? GetTypeSignature(MethodInfo? methodInfo)
    {
        if (methodInfo == null) return null;
        var types = methodInfo.GetParameters().Select(p => p.ParameterType).Prepend(methodInfo.ReturnType).ToArray();
        var builder = new System.Text.StringBuilder(types.Length);
        foreach (var type in types)
            builder.Append(typeChars.TryGetValue(type, out var c) ? c : 'i');
        return builder.ToString();
    }

    private static string? GetTypeSignature(Type type) => GetTypeSignature(type.GetMethod("Invoke"));

    private static string GetSymbolTypeSignature(string symbol, bool returnPointer)
    {
        try
        {
            var ret = GameData.GetMethodSymbol(symbol).TypeSignature;
            return returnPointer ? ret.Insert(1, "i") : ret;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static bool ValidateDelegate(Type delegateType, string symbol, bool returnPointer) => GetTypeSignature(delegateType) == GetSymbolTypeSignature(symbol, returnPointer);

    private static Delegate? GetMethodDelegate(IReflect ownerType, Type delegateType, object? o, string methodName)
    {
        var detourMethod = ownerType.GetMethod(methodName, Util.AllMembersBindingFlags);
        return CreateDelegate(delegateType, o, detourMethod);
    }

    private static Delegate[] GetMethodDelegates(IReflect ownerType, Type delegateType, object? o) => ownerType.GetAllMethods()
        .Select(methodInfo => CreateDelegate(delegateType, o, methodInfo)).Where(del => del != null).ToArray()!;

    private static Delegate? CreateDelegate(Type delegateType, object? o, MethodInfo? delegateMethod)
    {
        if (delegateMethod == null) return null;
        return delegateMethod.IsStatic
            ? Delegate.CreateDelegate(delegateType, delegateMethod, false)
            : Delegate.CreateDelegate(delegateType, o, delegateMethod, false);
    }

    private static void LogInjectError(MemberInfo memberInfo, string message, bool required)
    {
        message = $"Error injecting {memberInfo.ReflectedType?.FullName}.{memberInfo.Name}:\n{message}";
        if (required)
            throw new ApplicationException(message);
        Logging.Error(message);
    }

    public void Dispose()
    {
        scanner.Dispose();
        if (copyAddress != nint.Zero)
            Marshal.FreeHGlobal(copyAddress);
    }
}