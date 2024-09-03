using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Materia.Utilities;
using PInvoke;

namespace Materia;

internal class Updater
{
    private const string materiaGitHub = "https://github.com/UnknownX7/Materia";
    private const string latestReleasePath = $"{materiaGitHub}/releases/latest/download/";
    private const string updateZip = "update.zip";

    private const string materiaDepsGitHub = "https://github.com/UnknownX7/MateriaDeps";
    private const string latestLibPath = $"{materiaDepsGitHub}/raw/master/";
    private const string latestECGenPath = $"{latestLibPath}ECGen/";
    private const string libZip = "lib.zip";
    private const string genDll = "ECGen.Generated.dll";
    private const string symbolsFile = "symbols.bin";

    private static readonly DirectoryInfo updateDirectory = new(Path.Combine(Util.MateriaDirectory.FullName, ".update"));
    private static readonly DirectoryInfo updateLibDirectory = new(Path.Combine(updateDirectory.FullName, "lib"));
    private readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    // TODO: Check versions
    public void Update()
    {
        try
        {
#if RELEASE
            Util.AllocConsole();
#endif
            if (updateDirectory.Exists)
                updateDirectory.Delete(true);
            updateDirectory.Create();
            updateLibDirectory.Create();

            if (true)
                UpdateMateria();

            if (false)
            {
                UpdateLib();
                UpdateECGen();
            }
            else if (true)
            {
                UpdateECGen();
            }

            User32.MessageBox(IntPtr.Zero, "Update download has finished, the game will now relaunch.", $"{nameof(Materia)} Framework", User32.MessageBoxOptions.MB_ICONINFORMATION);
            FinishUpdating();
        }
        catch (Exception e)
        {
            User32.MessageBox(IntPtr.Zero, $"The following error occurred while updating:\n\n{e}", $"{nameof(Materia)} Framework", User32.MessageBoxOptions.MB_ICONERROR);
        }
    }

    private void UpdateMateria()
    {
        try
        {
            Logging.Info("Updating Materia");
            var updateZipPath = Path.Combine(Util.MateriaDirectory.FullName, updateZip);
            DownloadFile($"{latestReleasePath}{updateZip}", updateZipPath);
            ZipFile.ExtractToDirectory(updateZipPath, updateDirectory.FullName);
            File.Delete(updateZipPath);
        }
        catch (Exception e)
        {
            Logging.Error($"Error while updating Materia\n{e}");
            throw;
        }
    }

    private void UpdateLib()
    {
        try
        {
            Logging.Info("Updating lib");
            var libZipPath = Path.Combine(Util.MateriaDirectory.FullName, libZip);
            DownloadFile($"{latestLibPath}{libZip}", libZipPath);
            ZipFile.ExtractToDirectory(libZipPath, updateLibDirectory.FullName);
            File.Delete(libZipPath);
        }
        catch (Exception e)
        {
            Logging.Error($"Error while updating lib\n{e}");
            throw;
        }
    }

    private void UpdateECGen()
    {
        try
        {
            Logging.Info("Updating symbols");
            DownloadFile($"{latestECGenPath}{genDll}", Path.Combine(updateLibDirectory.FullName, genDll));
            DownloadFile($"{latestECGenPath}{symbolsFile}", Path.Combine(updateDirectory.FullName, symbolsFile));
        }
        catch (Exception e)
        {
            Logging.Error($"Error while updating\n{e}");
            throw;
        }
    }

    private void DownloadFile(string uri, string savePath)
    {
        Logging.Info($"Downloading \"{uri}\" to \"{savePath}\"");
        File.WriteAllBytes(savePath, DownloadBytes(uri));
    }

    private byte[] DownloadBytes(string uri)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = httpClient.Send(request).EnsureSuccessStatusCode();
        return response.Content.ReadAsByteArrayAsync().Result;
    }

    private static void FinishUpdating()
    {
        Process.Start(new ProcessStartInfo("cmd.exe")
        {
            Arguments = BuildCmdArg("timeout 2",
                $"move /Y \"{Path.Combine(updateDirectory.FullName, "*")}\" \"{Util.MateriaDirectory.FullName}\"", // TODO: Make recursive for subdirectories
                $"move /Y \"{Path.Combine(updateLibDirectory.FullName, "*")}\" \"{Path.Combine(Util.MateriaDirectory.FullName, "lib")}\"",
                $"rmdir /S /Q \"{updateDirectory.FullName}\"",
                $"\"{Path.Combine(Util.GameDirectory.FullName, "FF7EC.exe")}\""),
            UseShellExecute = true,
            WorkingDirectory = Util.MateriaDirectory.FullName
        });
    }

    private static string BuildCmdArg(params string[] args)
    {
        var arg = new StringBuilder("/C ");

        for (int i = 0; i < args.Length; i++)
        {
            if (i != 0)
                arg.Append(" & ");
            arg.Append('(');
            arg.Append(args[i]);
            arg.Append(')');
        }

        return arg.ToString();
    }
}