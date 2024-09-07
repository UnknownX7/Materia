using System.Diagnostics;
using System.Runtime.InteropServices;
using Materia.Utilities;

namespace Materia.Game;

public class Symbols
{
    public record struct MethodSymbol(string Name, string Signature, string TypeSignature, int Offset);
    public record struct GlobalSymbol(string Name, string Signature, int Offset);
    public record struct GlobalMethodSymbol(string Name, int Offset, int MethodOffset);

    public MethodSymbol[] MethodSymbols { get; set; } = [];
    public GlobalSymbol[] GlobalSymbols { get; set; } = [];
    public GlobalMethodSymbol[] GlobalMethodSymbols { get; set; } = [];
    public int DllTimestamp { get; set; }
}

public static unsafe class GameData
{
    private static readonly string symbolsPath = Path.Combine(Util.MateriaDirectory.FullName, "symbols.bin");
    private static Symbols symbols = null!;
    private static Dictionary<string, int> symbolDictionary = null!;
    private static Dictionary<string, Symbols.MethodSymbol> methodSymbolDictionary = null!;
    private static Dictionary<string, Symbols.GlobalSymbol> globalSymbolDictionary = null!;
    private static Dictionary<string, Symbols.GlobalMethodSymbol> globalMethodSymbolDictionary = null!;

    public static Symbols Symbols
    {
        get => symbols;
        set
        {
            symbols = value;
            symbolDictionary = value.MethodSymbols.Select(s => (s.Name, s.Offset))
                .Concat(value.GlobalSymbols.Select(s => (s.Name, s.Offset)))
                .Concat(value.GlobalMethodSymbols.Select(s => (s.Name, s.Offset)))
                .ToDictionary(s => s.Name, s => s.Offset);
            methodSymbolDictionary = value.MethodSymbols.ToDictionary(s => s.Name);
            globalSymbolDictionary = value.GlobalSymbols.ToDictionary(s => s.Name);
            globalMethodSymbolDictionary = value.GlobalMethodSymbols.ToDictionary(s => s.Name);
        }
    }

    static GameData() => Symbols = Util.LoadJsonFromFile<Symbols>(symbolsPath);

    public static int GetSymbolOffset(string name) => symbolDictionary[name];
    public static bool TryGetSymbolOffset(string name, out int offset) => symbolDictionary.TryGetValue(name, out offset);
    public static nint GetSymbolAddress(string name) => SigScanner.GameAssembly.BaseAddress + symbolDictionary[name];

    public static bool TryGetSymbolAddress(string name, out nint address)
    {
        if (!symbolDictionary.TryGetValue(name, out var offset))
        {
            address = nint.Zero;
            return false;
        }

        address = SigScanner.GameAssembly.BaseAddress + offset;
        return true;
    }

    public static Symbols.MethodSymbol GetMethodSymbol(string name) => methodSymbolDictionary[name];
    public static Symbols.GlobalSymbol GetGlobalSymbol(string name) => globalSymbolDictionary[name];
    public static Symbols.GlobalMethodSymbol GetGlobalMethodSymbol(string name) => globalMethodSymbolDictionary[name];

    internal static bool CheckVersion() => symbols.DllTimestamp == GetGameAssemblyTimestamp();

    private static int GetGameAssemblyTimestamp()
    {
        var baseAddress = Util.GetProcessModule("GameAssembly.dll")!.BaseAddress;
        var ntHeader = baseAddress + *(int*)(baseAddress + 0x3C);
        var fileHeader = ntHeader + 0x4;
        var timestamp = *(int*)(fileHeader + 0x4);
        return timestamp;
    }

    [Conditional("DEBUG")]
    internal static void DumpGameAssembly()
    {
        var m = Util.GetProcessModule("GameAssembly.dll")!;
        var copy = new byte[m.ModuleMemorySize];
        Marshal.Copy(m.BaseAddress, copy, 0, copy.Length);
        fixed (byte* b = copy)
        {
            var ntHeader = b + *(int*)(b + 0x3C);
            var sectionCount = *(short*)(ntHeader + 0x6);
            var sectionHeader = (SectionHeader*)(ntHeader + 0x18 + 0xF0);
            for (int i = 0; i < sectionCount; i++)
            {
                sectionHeader->rawDataSize = sectionHeader->virtualSize;
                sectionHeader->rawDataOffset = sectionHeader->virtualOffset;
                sectionHeader++;
            }
        }

        var gameAssemblyPath = Path.Combine(Util.MateriaDirectory.FullName, "GameAssembly.dll");
        File.WriteAllBytes(gameAssemblyPath, copy);
    }
}