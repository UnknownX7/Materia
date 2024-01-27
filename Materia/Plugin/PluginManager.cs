using System.Diagnostics;
using ImGuiNET;
using Materia.Utilities;

namespace Materia.Plugin;

internal sealed class PluginManager : IDisposable
{
    private static readonly FileSystemWatcher pluginDirWatcher = new(Util.PluginDirectory.FullName)
    {
        Filter = "*.dll",
        NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    };
    private readonly List<LoadedPlugin> plugins = new();
    private readonly Dictionary<string, WatcherChangeTypes> pendingFileChanges = new();
    private readonly Stopwatch fileChangedStopwatch = new();

    public void Initialize()
    {
        foreach (var file in Util.PluginDirectory.GetFiles("*.dll"))
            LoadPlugin(file.FullName);
        pluginDirWatcher.Created += OnFileChanged;
        pluginDirWatcher.Changed += OnFileChanged;
        pluginDirWatcher.Deleted += OnFileChanged;
    }

    public void LoadPlugin(string path)
    {
        var assemblyName = Path.GetFileNameWithoutExtension(path);
        var plugin = plugins.FirstOrDefault(p => p.PluginAssemblyName == assemblyName);

        try
        {
            if (plugin != null)
            {
                Logging.Info($"Reloading {assemblyName}");
                plugin.Reload();
            }
            else
            {
                Logging.Info($"Loading {assemblyName}");
                plugin = new(path);
                plugins.Add(plugin);
            }
            Logging.Info($"Finished loading {assemblyName} {plugin.PluginVersion}");
        }
        catch (Exception e)
        {
            Logging.Error($"Failed loading {assemblyName}\n{e}");
            UnloadPlugin(path);
        }
    }

    public void UnloadPlugin(string path)
    {
        var assemblyName = Path.GetFileNameWithoutExtension(path);
        var plugin = plugins.FirstOrDefault(p => p.PluginAssemblyName == assemblyName);
        if (plugin == null) return;

        try
        {
            Logging.Info($"Unloading {assemblyName}");
            plugin.Unload();
            plugins.Remove(plugin);
            Logging.Info($"Finished unloading {assemblyName}");
        }
        catch (Exception e)
        {
            Logging.Error($"Failed unloading {assemblyName}\n{e}");
        }
    }

    public void InvokeAll(Action<LoadedPlugin> f, string eventName)
    {
        foreach (var loadedPlugin in plugins)
            loadedPlugin.Invoke(f, eventName);
    }

    private void OnFileChanged(object sender, FileSystemEventArgs args)
    {
        fileChangedStopwatch.Restart();
        pendingFileChanges[args.FullPath] = args.ChangeType;
    }

    private void ProcessPendingFileChanges()
    {
        foreach (var kv in pendingFileChanges)
        {
            var needLoad = (kv.Value & (WatcherChangeTypes.Created | WatcherChangeTypes.Changed)) != 0;
            if (needLoad)
                LoadPlugin(kv.Key);
            else
                UnloadPlugin(kv.Key);
        }

        pendingFileChanges.Clear();
    }

    public void Update()
    {
        if (fileChangedStopwatch is { IsRunning: true, ElapsedMilliseconds: >= 500 })
        {
            fileChangedStopwatch.Stop();
            ProcessPendingFileChanges();
        }

        InvokeAll(p => p.PluginServiceManager?.EventHandler.InvokeUpdate(), nameof(PluginEventHandler.Update));
    }

    public void Draw()
    {
        InvokeAll(p =>
        {
            ImGui.PushID(p.PluginAssemblyName);
            p.PluginServiceManager?.EventHandler.InvokeDraw();
            ImGui.PopID();
        }, nameof(PluginEventHandler.Draw));
    }

    public void Dispose()
    {
        pluginDirWatcher.Dispose();
        foreach (var plugin in plugins)
            plugin.Dispose();
    }
}