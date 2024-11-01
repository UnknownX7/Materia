using ECGen.Generated;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Materia.Utilities;

public static partial class Util
{
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ReadProcessMemory(int hProcess, nint lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WriteProcessMemory(int hProcess, nint lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool AllocConsole();

    public static DirectoryInfo MateriaDirectory { get; } = new(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
    public static DirectoryInfo PluginDirectory { get; } = new(Path.Combine(MateriaDirectory.FullName, "plugins"));
    public static DirectoryInfo ConfigDirectory { get; } = new(Path.Combine(MateriaDirectory.FullName, "configs"));
    public static DirectoryInfo GameDirectory { get; } = new(Path.GetDirectoryName(Environment.ProcessPath)!);
    public static DirectoryInfo LogDirectory { get; } = new(Path.Combine(MateriaDirectory.FullName, "logs"));

    private static nint CurrentHandle => Process.GetCurrentProcess().Handle;

    public const BindingFlags AllMembersBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    static Util()
    {
        if (!PluginDirectory.Exists)
            PluginDirectory.Create();
        if (!ConfigDirectory.Exists)
            ConfigDirectory.Create();
        if (!LogDirectory.Exists)
            LogDirectory.Create();
    }

    public static void WaitFor(Func<bool> f, int sleepMs = 1000)
    {
        while (!f())
            Thread.Sleep(sleepMs);
    }

    public static bool IsFileInUse(string path)
    {
        try
        {
            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
            return false;
        }
        catch (IOException)
        {
            return true;
        }
    }

    public static void WaitForFile(string path, TimeSpan? maxWait = null)
    {
        if (!IsFileInUse(path)) return;
        var stopwatch = Stopwatch.StartNew();
        WaitFor(() => maxWait != null && stopwatch.Elapsed >= maxWait || !IsFileInUse(path), 100);
    }

    public static bool SaveJsonToFile(string path, object o)
    {
        try
        {
            if (path.EndsWith(".json"))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(o, Formatting.Indented));
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
                using var ms = new MemoryStream();
                using (var gs = new GZipStream(ms, CompressionMode.Compress))
                    gs.Write(bytes, 0, bytes.Length);
                File.WriteAllBytes(path, ms.ToArray());
            }
            return true;
        }
        catch (Exception e)
        {
            Logging.Error($"Failed to save {path}\n{e}");
            return false;
        }
    }

    public static T LoadJsonFromFile<T>(string path) where T : new()
    {
        try
        {
            string json;
            if (path.EndsWith(".json"))
            {
                json = File.ReadAllText(path);
            }
            else
            {
                using var ms = new MemoryStream(File.ReadAllBytes(path));
                using var gs = new GZipStream(ms, CompressionMode.Decompress);
                using var r = new StreamReader(gs);
                json = r.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(json)!;
        }
        catch (FileNotFoundException)
        {
            return new T();
        }
    }

    public static void MergeDirectory(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
    {
        if (!targetDirectory.Exists)
            targetDirectory.Create();
        foreach (var f in sourceDirectory.GetFiles())
            f.MoveTo(Path.Combine(targetDirectory.FullName, f.Name), true);
        foreach (var subDir in sourceDirectory.GetDirectories())
            MergeDirectory(subDir.FullName, Path.Combine(targetDirectory.FullName, subDir.Name));
        sourceDirectory.Delete();
    }

    public static void MergeDirectory(string source, string target) => MergeDirectory(new DirectoryInfo(source), new DirectoryInfo(target));

    private static Dictionary<string, ProcessModule>? moduleCache;
    public static ProcessModule? GetProcessModule(string name)
    {
        moduleCache ??= Process.GetCurrentProcess().Modules.Cast<ProcessModule>().DistinctBy(p => p.ModuleName).ToDictionary(p => p.ModuleName);
        if (moduleCache.TryGetValue(name, out var m)) return m;

        m = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault(p => p.ModuleName == name);
        moduleCache[name] = m!;
        return m;
    }

    public static unsafe T ReadMemory<T>(nint process, nint address) where T : unmanaged
    {
        var buffer = new byte[sizeof(T)];
        var _ = 0;
        if (!ReadProcessMemory((int)process, address, buffer, buffer.Length, ref _))
            throw new AccessViolationException($"Failed to read address {address:X} (Process: {process})");

        fixed (byte* b = buffer)
            return *(T*)b;
    }

    public static void WriteMemory(nint process, nint address, byte[] value)
    {
        var _ = 0;
        if (!WriteProcessMemory((int)process, address, value, value.Length, ref _))
            throw new AccessViolationException($"Failed to write to address {address:X} (Process: {process}) (Error Code: {Marshal.GetLastWin32Error()})");
    }

    public static void WriteMemory(nint address, byte[] value) => WriteMemory(CurrentHandle, address, value);

    public static unsafe void WriteMemory<T>(nint process, nint address, T value) where T : unmanaged
    {
        var buffer = new byte[sizeof(T)];
        fixed (byte* b = buffer)
            *(T*)b = value;
        WriteMemory(process, address, buffer);
    }

    public static void WriteMemory<T>(nint address, T value) where T : unmanaged => WriteMemory(CurrentHandle, address, value);

    public static unsafe string ReadCString(void* ptr) => Marshal.PtrToStringAnsi((nint)ptr)!;
    public static unsafe string ReadCString(void* ptr, int len) => Marshal.PtrToStringAnsi((nint)ptr, len);

    public static unsafe nint GetVirtualFunctionByName<T>(T* o, string name, bool isClassType = false) where T : unmanaged
    {
        try
        {
            object? vtbl;
            if (isClassType)
            {
                var vtblField = typeof(T).GetField("vtable");
                if (vtblField == null) return nint.Zero;

                vtbl = vtblField.GetValue(*o);
            }
            else
            {
                var classField = typeof(T).GetField("@class");
                if (classField == null) return nint.Zero;

                var @class = classField.GetValue(*o);
                if (@class == null) return nint.Zero;

                var classType = classField.FieldType.GetElementType()!;
                var vtblField = classType.GetField("vtable");
                if (vtblField == null) return nint.Zero;

                vtbl = vtblField.GetValue(Marshal.PtrToStructure((nint)Pointer.Unbox(@class), classType));
            }

            if (vtbl == null) return nint.Zero;

            foreach (var fieldInfo in vtbl.GetType().GetAllFields())
            {
                var fieldObj = fieldInfo.GetValue(vtbl);
                if (fieldObj == null) continue;

                var virtualInvokeData = (Il2CppVirtualInvokeData)fieldObj;
                var ptr = virtualInvokeData.method->name;
                if (ptr == null) continue;

                var vfName = ReadCString(ptr);
                if (vfName == name) return virtualInvokeData.methodPtr;
            }
        }
        catch { }

        return nint.Zero;
    }
}