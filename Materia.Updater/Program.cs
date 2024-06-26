using System.IO.Compression;

namespace Materia.Updater;

internal class Program
{
    private const string updaterName = $"{nameof(Materia)}.{nameof(Updater)}";

    private const string materiaGitHub = "https://github.com/UnknownX7/Materia";
    private const string latestReleasePath = $"{materiaGitHub}/releases/latest/download/";
    private const string updateZip = "update.zip";

    private const string materiaDepsGitHub = "https://github.com/UnknownX7/MateriaDeps";
    private const string latestLibPath = $"{materiaDepsGitHub}/raw/master/";
    private const string latestECGenPath = $"{latestLibPath}ECGen/";
    private const string libZip = "lib.zip";
    private const string genDll = "ECGen.Generated.dll";
    private const string symbolsFile = "symbols.bin";

    private static readonly DirectoryInfo materiaDirectory = new(Path.GetDirectoryName(Environment.ProcessPath)!);
    private static readonly DirectoryInfo libDirectory = new(Path.Combine(materiaDirectory.FullName, "lib"));
    private static readonly DirectoryInfo tempDirectory = new(Path.Combine(materiaDirectory.FullName, "temp"));
    private static readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    private static void Main(string[] args)
    {
        try
        {
            Update();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    // TODO: Check versions
    private static void Update()
    {
        if (true)
            UpdateMateria();

        if (true)
            UpdateLib();

        if (true)
            UpdateECGen();

        Console.WriteLine("Done updating!");
    }

    private static void UpdateMateria()
    {
        try
        {
            if (tempDirectory.Exists)
                tempDirectory.Delete(true);

            var updateZipPath = Path.Combine(materiaDirectory.FullName, updateZip);
            DownloadFile($"{latestReleasePath}{updateZip}", updateZipPath);
            ZipFile.ExtractToDirectory(updateZipPath, tempDirectory.Name);
            File.Delete(updateZipPath);
            foreach (var file in tempDirectory.GetFiles())
                file.MoveTo(Path.Combine(materiaDirectory.FullName, file.Name.StartsWith(updaterName) ? $"{file.Name}.new" : file.Name), true);

            tempDirectory.Delete(true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while updating Materia\n{e}");
        }
    }

    private static void UpdateLib()
    {
        try
        {
            if (libDirectory.Exists) return; // TODO: Reenable when version checking is added

            var genDllFilePath = Path.Combine(libDirectory.FullName, genDll);
            var genDllFileTempPath = Path.Combine(materiaDirectory.FullName, genDll);
            var genDllFile = new FileInfo(genDllFilePath);

            if (libDirectory.Exists)
            {
                if (genDllFile.Exists)
                    genDllFile.MoveTo(genDllFileTempPath);
                libDirectory.Delete(true);
            }

            var libZipPath = Path.Combine(materiaDirectory.FullName, libZip);
            DownloadFile($"{latestLibPath}{libZip}", libZipPath);
            ZipFile.ExtractToDirectory(libZipPath, libDirectory.Name);
            File.Delete(libZipPath);

            if (genDllFile.Exists)
                genDllFile.MoveTo(genDllFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while updating lib\n{e}");
        }
    }

    private static void UpdateECGen()
    {
        try
        {
            if (!libDirectory.Exists)
                libDirectory.Create();

            DownloadFile($"{latestECGenPath}{symbolsFile}", Path.Combine(materiaDirectory.FullName, symbolsFile));
            DownloadFile($"{latestECGenPath}{genDll}", Path.Combine(libDirectory.FullName, genDll));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while updating\n{e}");
        }
    }

    private static void DownloadFile(string uri, string savePath)
    {
        Console.WriteLine($"Downloading \"{uri}\" to \"{savePath}\"");
        File.WriteAllBytes(savePath, DownloadBytes(uri));
    }

    private static byte[] DownloadBytes(string uri)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = httpClient.Send(request);
        return response.Content.ReadAsByteArrayAsync().Result;
    }
}