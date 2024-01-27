using System.Reflection;
using System.Runtime.Loader;

namespace Materia.Plugin;

internal sealed class LoadedPlugin : IDisposable
{
    public string PluginAssemblyName { get; }
    public string PluginName => plugin?.Name ?? PluginAssemblyName;
    public PluginServiceManager? PluginServiceManager { get; private set; }
    public Version? PluginVersion => assembly?.GetName().Version;

    private readonly string path;
    private AssemblyLoadContext? loadContext;
    private Assembly? assembly;
    private IMateriaPlugin? plugin;

    public LoadedPlugin(string p)
    {
        PluginAssemblyName = Path.GetFileNameWithoutExtension(p);
        path = p;
        Load();
    }

    public void Load()
    {
        loadContext = new AssemblyLoadContext(Path.GetFileNameWithoutExtension(path), true);
        using var fs = File.OpenRead(path);
        assembly = loadContext.LoadFromStream(fs);
        PluginServiceManager = new PluginServiceManager(PluginAssemblyName);
        SigScanner.InjectAttributes(assembly, PluginServiceManager.HookManager);
        var pluginType = assembly.GetTypes().First(t => !t.IsInterface && t.IsAssignableTo(typeof(IMateriaPlugin)));
        plugin = (IMateriaPlugin)Activator.CreateInstance(pluginType, PluginServiceManager)!;
    }

    public void Reload()
    {
        Dispose();
        plugin = null;
        loadContext = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Load();
    }

    public void Unload() => Dispose();

    public void Invoke(Action<LoadedPlugin> invoker, string eventName)
    {
        try
        {
            if (plugin != null)
                invoker(this);
        }
        catch (Exception e)
        {
            Logging.Error($"Error in {PluginAssemblyName}.{nameof(PluginEventHandler)}.{eventName}\n{e}");
        }
    }

    public void Dispose()
    {
        Invoke(p => p.PluginServiceManager?.EventHandler.InvokeDispose(), nameof(PluginEventHandler.Dispose));
        PluginServiceManager?.Dispose();
        loadContext?.Unload();
    }
}