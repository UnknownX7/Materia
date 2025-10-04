using Materia.Utilities;

namespace Materia;

public static class Logging
{
    public enum LogType
    {
        Verbose,
        Info,
        Debug,
        Warning,
        Error
    }

    internal static LogType LoggingLevel
    {
        get => Materia.Config.LoggingLevel;
        set
        {
            Materia.Config.LoggingLevel = value;
            Materia.Config.Save();
        }
    }
    internal static string MainLogFilePath => mainLog.FilePath;
    internal static List<string> MainLogOutputLines { get; } = [];

    private static readonly Dictionary<string, LogFile> logFileCache = [];
    private static readonly LogFile mainLog;

    static Logging()
    {
        foreach (var log in Util.LogDirectory.GetFiles("*.log").OrderByDescending(f => f.CreationTime).Skip(10))
            log.Delete();

        mainLog = OpenLog();
        mainLog.OnOutput = s =>
        {
            lock (MainLogOutputLines)
                MainLogOutputLines.Add(s);
        };
    }

    public static LogFile OpenLog(string? prefix = null)
    {
        if (!string.IsNullOrEmpty(prefix))
            prefix += "_";

        var path = Path.Combine(Util.LogDirectory.FullName, $"{prefix}{DateTime.Now:yyyyMMdd}.log");
        lock (logFileCache)
        {
            if (!logFileCache.TryGetValue(path, out var logFile))
                logFileCache.Add(path, logFile = new(path));
            return logFile;
        }
    }

    private static void Log(LogType level, Action<string> logFunc, string message)
    {
        if (level < LoggingLevel) return;
        logFunc(message);
    }

    private static void Log(LogType level, Action<string> logFunc, Exception e)
    {
        if (level < LoggingLevel) return;
        logFunc(e.ToString());
    }

    private static void Log(LogType level, Action<string> logFunc, Func<string> getMessage)
    {
        if (level < LoggingLevel) return;
        logFunc(getMessage());
    }

    private static void Log(LogType level, Action<string> logFunc, string prefix, string message)
    {
        if (level < LoggingLevel) return;
        logFunc($"[{prefix}] {message}");
    }

    private static void Log(LogType level, Action<string> logFunc, string prefix, Exception e)
    {
        if (level < LoggingLevel) return;
        logFunc($"[{prefix}] {e}");
    }

    private static void Log(LogType level, Action<string> logFunc, string prefix, Func<string> getMessage)
    {
        if (level < LoggingLevel) return;
        logFunc($"[{prefix}] {getMessage()}");
    }

    internal static void Verbose(string message) => Log(LogType.Verbose, mainLog.Verbose, message);
    internal static void Verbose(Exception e) => Log(LogType.Verbose, mainLog.Verbose, e);
    internal static void Verbose(Func<string> getMessage) => Log(LogType.Verbose, mainLog.Verbose, getMessage);
    internal static void Verbose(string prefix, string message) => Log(LogType.Verbose, mainLog.Verbose, prefix, message);
    internal static void Verbose(string prefix, Exception e) => Log(LogType.Verbose, mainLog.Verbose, prefix, e);
    internal static void Verbose(string prefix, Func<string> getMessage) => Log(LogType.Verbose, mainLog.Verbose, prefix, getMessage);

    internal static void Info(string message) => Log(LogType.Info, mainLog.Info, message);
    internal static void Info(Exception e) => Log(LogType.Info, mainLog.Info, e);
    internal static void Info(Func<string> getMessage) => Log(LogType.Info, mainLog.Info, getMessage);
    internal static void Info(string prefix, string message) => Log(LogType.Info, mainLog.Info, prefix, message);
    internal static void Info(string prefix, Exception e) => Log(LogType.Info, mainLog.Info, prefix, e);
    internal static void Info(string prefix, Func<string> getMessage) => Log(LogType.Info, mainLog.Info, prefix, getMessage);

    internal static void Debug(string message) => Log(LogType.Debug, mainLog.Debug, message);
    internal static void Debug(Exception e) => Log(LogType.Debug, mainLog.Debug, e);
    internal static void Debug(Func<string> getMessage) => Log(LogType.Debug, mainLog.Debug, getMessage);
    internal static void Debug(string prefix, string message) => Log(LogType.Debug, mainLog.Debug, prefix, message);
    internal static void Debug(string prefix, Exception e) => Log(LogType.Debug, mainLog.Debug, prefix, e);
    internal static void Debug(string prefix, Func<string> getMessage) => Log(LogType.Debug, mainLog.Debug, prefix, getMessage);

    internal static void Warning(string message) => Log(LogType.Warning, mainLog.Warning, message);
    internal static void Warning(Exception e) => Log(LogType.Warning, mainLog.Warning, e);
    internal static void Warning(Func<string> getMessage) => Log(LogType.Warning, mainLog.Warning, getMessage);
    internal static void Warning(string prefix, string message) => Log(LogType.Warning, mainLog.Warning, prefix, message);
    internal static void Warning(string prefix, Exception e) => Log(LogType.Warning, mainLog.Warning, prefix, e);
    internal static void Warning(string prefix, Func<string> getMessage) => Log(LogType.Warning, mainLog.Warning, prefix, getMessage);

    internal static void Error(string message) => Log(LogType.Error, mainLog.Error, message);
    internal static void Error(Exception e) => Log(LogType.Error, mainLog.Error, e);
    internal static void Error(Func<string> getMessage) => Log(LogType.Error, mainLog.Error, getMessage);
    internal static void Error(string prefix, string message) => Log(LogType.Error, mainLog.Error, prefix, message);
    internal static void Error(string prefix, Exception e) => Log(LogType.Error, mainLog.Error, prefix, e);
    internal static void Error(string prefix, Func<string> getMessage) => Log(LogType.Error, mainLog.Error, prefix, getMessage);
}