namespace Materia.Plugin;

public class PluginLog
{
    private readonly string pluginName;
    internal PluginLog(string name) => pluginName = name;
    public void Verbose(string message) => Logging.Verbose(pluginName, message);
    public void Verbose(Exception e) => Logging.Verbose(pluginName, e);
    public void Verbose(Func<string> getMessage) => Logging.Verbose(pluginName, getMessage);
    public void Info(string message) => Logging.Info(pluginName, message);
    public void Info(Exception e) => Logging.Info(pluginName, e);
    public void Info(Func<string> getMessage) => Logging.Info(pluginName, getMessage);
    public void Debug(string message) => Logging.Debug(pluginName, message);
    public void Debug(Exception e) => Logging.Debug(pluginName, e);
    public void Debug(Func<string> getMessage) => Logging.Debug(pluginName, getMessage);
    public void Warning(string message) => Logging.Warning(pluginName, message);
    public void Warning(Exception e) => Logging.Warning(pluginName, e);
    public void Warning(Func<string> getMessage) => Logging.Warning(pluginName, getMessage);
    public void Error(string message) => Logging.Error(pluginName, message);
    public void Error(Exception e) => Logging.Error(pluginName, e);
    public void Error(Func<string> getMessage) => Logging.Error(pluginName, getMessage);
}