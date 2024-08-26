using System.IO.Compression;
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
            // TODO: Launch command to move directory and close game
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
}