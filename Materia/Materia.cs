using Materia.Game;
using Materia.Plugin;
using Materia.UI;
using Materia.Utilities;
using PInvoke;
using System.Diagnostics;
using System.Reflection;

namespace Materia;

internal static class Materia
{
    public static MateriaConfig Config { get; } = PluginConfiguration.Load<MateriaConfig>(Path.Combine(Util.MateriaDirectory.FullName, $"{nameof(Materia)}.json"));
    public static PluginManager PluginManager { get; } = new();
    public static RenderManager? RenderManager { get; private set; }
    public static InputManager? InputManager { get; private set; }
    public static ImGuiManager? ImGuiManager { get; private set; }
    internal static bool IsUpdateRunning { get; private set; }
    internal static int CurrentUpdateThreadId { get; private set; }

    private delegate void BugsnagStartDelegate(nint config, nint method);
    private static readonly HookManager hookManager = new();
    private static Task stopCrashHandlerTask = null!;
    private static IMateriaHook<Action>? playerLoopHook;
    private static bool initializing = false;
    private static bool systemsInitialized = false;

    public static void Initialize()
    {
        try
        {
            stopCrashHandlerTask = StopCrashHandlerAsync();

            if (!GameData.CheckVersion())
            {
                var result = User32.MessageBox(IntPtr.Zero, $"The installed {nameof(Materia)} Framework version is incompatible with the current game version, allowing the game to continue may be unsafe.\nContinue?",
                    $"{nameof(Materia)} Framework", User32.MessageBoxOptions.MB_YESNO | User32.MessageBoxOptions.MB_ICONEXCLAMATION);
                if (result != User32.MessageBoxResult.IDYES)
                    Environment.Exit(0);
                return;
            }

            DisableTelemetry();
            Logging.Info($"Starting {nameof(Materia)} {Assembly.GetExecutingAssembly().GetName().Version}");
            Logging.Info($"Game Version Timestamp: {DateTimeOffset.FromUnixTimeSeconds(GameData.Symbols.DllTimestamp):g}");
            var address = SigScanner.UnityPlayer.Scan("48 83 EC 78 80 3D");
            playerLoopHook = new MateriaHook<Action>(address, Update);
            playerLoopHook.Enable();
            Logging.Info($"PlayerLoop: {address:X}");
        }
        catch (Exception e)
        {
            FatalError(e);
        }
    }

    private static void InitializeSystems()
    {
        try
        {
            initializing = true;
            stopCrashHandlerTask.Wait();

            RenderManager = new RenderManager();
            InputManager = new InputManager(RenderManager.SwapChain);
            ImGuiManager = new ImGuiManager();
            RenderManager.Present += ImGuiManager.Present;
            RenderManager.PreResizeBuffers += ImGuiManager.PreResizeBuffers;
            RenderManager.PostResizeBuffers += ImGuiManager.PostResizeBuffers;
            InputManager.WndProcHandler = ImGuiManager.WndProcHandler;
            ImGuiManager.Draw += Draw;
            Logging.Info($"Finished initializing {nameof(Materia)}");

            SigScanner.InjectAttributes(Assembly.GetExecutingAssembly(), hookManager);
            PluginManager.Initialize();
            initializing = false;
            systemsInitialized = true;
        }
        catch (Exception e)
        {
            FatalError(e);
        }
    }

    public static void Update()
    {
        CurrentUpdateThreadId = Environment.CurrentManagedThreadId;
        IsUpdateRunning = true;

        try
        {

            if (!systemsInitialized)
                InitializeSystems();
            PluginManager.Update();
            GameInterop.Update();
            playerLoopHook!.Original();
        }
        catch (Exception e)
        {
            Logging.Error(e);
        }

        IsUpdateRunning = false;
    }

    public static void Draw()
    {
        PluginMenu.Draw(PluginManager);
        SettingsMenu.Draw();
        PluginManager.Draw();
    }

    private static async Task StopCrashHandlerAsync()
    {
        while (true)
        {
            try
            {
                const string crashHandlerName = "UnityCrashHandler64";
                var path = new FileInfo($"{crashHandlerName}.exe").FullName;
                var process = Process.GetProcessesByName(crashHandlerName).First(p => p.MainModule?.FileName == path);
                process.Kill(true);
                return;
            }
            catch (Exception e)
            {
                if (!initializing)
                {
                    await Task.Delay(200);
                    continue;
                }

                FatalError(e);
            }
        }
    }

    private static unsafe void DisableTelemetry()
    {
        new MateriaHook<BugsnagStartDelegate>(GameData.GetSymbolAddress("BugsnagUnity.Bugsnag$$Start"), (_, _) => { }).Enable();
        foreach (var s in GameData.Symbols.MethodSymbols.Where(s => s.Name.StartsWith("Code")))
        {
            if (s.Name.Contains("$$StartD"))
            {
                var address = SigScanner.GameAssembly.BaseAddress + s.Offset;
                if (*(byte*)address is 0xC2 or 0xC3 or 0xCC) continue;
                Util.WriteMemory<byte>(address, 0xC3);
            }
            else if (s.Name.Contains("$$StopD") && !s.Name.EndsWith("Internal"))
            {
                var address = SigScanner.GameAssembly.BaseAddress + s.Offset;
                if (!s.Name.Contains("In"))
                    ((delegate* unmanaged<nint, void>)address)(0);
                else if (*(byte*)address != 0x48)
                    throw new ApplicationException();
            }
        }
    }

    private static void FatalError(Exception e)
    {
        Logging.Error(e);
        User32.MessageBox(IntPtr.Zero, $"The following error occurred during initialization:\n\n{e}", $"{nameof(Materia)} Framework", User32.MessageBoxOptions.MB_ICONERROR);
        Environment.Exit(0);
    }

    public static void Dispose()
    {
        Logging.Info("Shutting down...");
        Config.Save();
        playerLoopHook?.Dispose();
        PluginManager.Dispose();
        ImGuiManager?.Dispose();
        InputManager?.Dispose();
        RenderManager?.Dispose();
        hookManager.Dispose();
        Logging.Info("Shutdown completed");
    }
}