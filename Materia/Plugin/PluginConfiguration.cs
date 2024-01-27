using Newtonsoft.Json;
using Materia.Utilities;

namespace Materia.Plugin;

public abstract class PluginConfiguration
{
    [JsonIgnore] public FileInfo ConfigFile { get; private set; } = null!;
    public Version? PluginVersion;

    public void Save() => Util.SaveJsonToFile(ConfigFile.FullName, this);

    public virtual void Initialize() { }

    public static T Load<T>(string path) where T : PluginConfiguration, new()
    {
        T config;

        try
        {
            config = Util.LoadJsonFromFile<T>(path);
        }
        catch (Exception e)
        {
            Logging.Error($"Error loading {path}! Renaming old file and resetting...\n{e}");
            config = ResetConfig<T>(path);
        }

        config.ConfigFile = new FileInfo(path);
        config.Initialize();
        config.UpdateVersion();

        return config;
    }

    public static T Load<T>() where T : PluginConfiguration, new() => Load<T>(Path.Combine(Util.ConfigDirectory.FullName, $"{typeof(T).Assembly.GetName().Name}.json"));

    private static T ResetConfig<T>(string path) where T : new()
    {
        var file = new FileInfo(path);
        file.MoveTo(path + ".CORRUPT", true);
        return new T();
    }

    private void UpdateVersion()
    {
        var pluginVersion = GetType().Assembly.GetName().Version;
        if (PluginVersion == pluginVersion) return;

        var prevVersion = PluginVersion;
        PluginVersion = pluginVersion;

        if (prevVersion < pluginVersion)
            OnUpdate(prevVersion);
    }

    protected virtual void OnUpdate(Version? previousVersion) { }
}