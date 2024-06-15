namespace Materia.Updater;

internal class Program
{
    private const string materiaGitHub = "https://github.com/UnknownX7/Materia";
    private const string materiaDepsGitHub = "https://github.com/UnknownX7/MateriaDeps";
    private const string latestECGenPath = $"{materiaDepsGitHub}/raw/master/ECGen/";
    private const string genDll = "ECGen.Generated.dll";
    private const string symbolsFile = "symbols.bin";

    private static readonly DirectoryInfo materiaDirectory = new(Path.GetDirectoryName(Environment.ProcessPath)!);
    private static readonly DirectoryInfo libDirectory = new(Path.Combine(materiaDirectory.FullName, "lib"));
    private static readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    private static void Main(string[] args)
    {
        try
        {
            if (!materiaDirectory.Exists || !libDirectory.Exists)
                throw new ApplicationException($"Current executable directory is not a valid Materia directory! {materiaDirectory} {libDirectory}");

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
            UpdateLib();

        if (true)
            UpdateECGen();

        Console.WriteLine("Done updating!");
    }

    private static void UpdateLib()
    {
        try
        {
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