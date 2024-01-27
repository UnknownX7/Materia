namespace Materia.Plugin;

public sealed class PluginServiceManager : IDisposable
{
    public PluginEventHandler EventHandler { get; } = new();
    public HookManager HookManager { get; } = new();
    public PluginLog Log { get; }
    internal PluginServiceManager(string name) => Log = new PluginLog(name);
    public void Dispose() => HookManager.Dispose();
}