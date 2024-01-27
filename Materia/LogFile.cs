using System.Text;

namespace Materia;

public class LogFile
{
    private readonly SemaphoreSlim writeLock = new(1);

    public string FilePath { get; }
    public Action<string>? OnOutput;

    internal LogFile(string path) => FilePath = path;

    public async Task WriteAsync(string message, bool addTimestamp = true)
    {
        await Task.Yield();
        await writeLock.WaitAsync();

        try
        {
            OnOutput?.Invoke(message);
            await using var log = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            message = addTimestamp ? $"{DateTime.Now:HH:mm:ss}    {message}\n" : $"{message}\n";
            await log.WriteAsync(Encoding.UTF8.GetBytes(message));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error logging to file\n{e}");
        }
        finally
        {
            writeLock.Release();
        }
    }

    private void PrintAndLog(string prefix, string message, ConsoleColor color = ConsoleColor.Gray)
    {
        message = $"[{prefix}] {message}";
#if DEBUG
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
#endif
        _ = WriteAsync(message);
    }

    public void Verbose(string message) => PrintAndLog("VRB", message, ConsoleColor.DarkGray);
    public void Info(string message) => PrintAndLog("INF", message);
    public void Debug(string message) => PrintAndLog("DBG", message, ConsoleColor.DarkBlue);
    public void Warning(string message) => PrintAndLog("WRN", message, ConsoleColor.DarkYellow);
    public void Error(string message) => PrintAndLog("ERR", message, ConsoleColor.DarkRed);
}