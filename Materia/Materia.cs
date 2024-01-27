using System.Reflection;
using PInvoke;
using Materia.Game;
using Materia.Plugin;
using Materia.UI;
using Materia.Utilities;

namespace Materia;

internal static class Materia
{
    public static MateriaConfig Config { get; } = PluginConfiguration.Load<MateriaConfig>(Path.Combine(Util.MateriaDirectory.FullName, $"{nameof(Materia)}.json"));
    public static PluginManager PluginManager { get; } = new();
    public static RenderManager? RenderManager { get; private set; }
    public static InputManager? InputManager { get; private set; }
    public static ImGuiManager? ImGuiManager { get; private set; }

    private delegate void BugsnagStartDelegate(nint config, nint method);
    private static bool systemsInitialized = false;
    private static IMateriaHook<Action>? playerLoopHook;
    private static readonly HookManager hookManager = new();

    public static void Initialize()
    {
        if (!GameData.CheckVersion())
        {
            var result = User32.MessageBox(IntPtr.Zero, $"The installed {nameof(Materia)} Framework version is incompatible with the current game version, allowing the game to continue may be unsafe.\nContinue?",
                $"{nameof(Materia)} Framework", User32.MessageBoxOptions.MB_YESNO | User32.MessageBoxOptions.MB_ICONEXCLAMATION);
            if (result != User32.MessageBoxResult.IDYES)
                Environment.Exit(0);
            return;
        }

        try
        {
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
            Logging.Error(e);
#if !DEBUG
            throw;
#endif
        }
    }

    private static void InitializeSystems()
    {
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
        systemsInitialized = true;
    }

    public static void Update()
    {
        try
        {
            if (!systemsInitialized)
                InitializeSystems();
            PluginManager.Update();
            playerLoopHook!.Original();
        }
        catch (Exception e)
        {
            Logging.Error(e);
        }
    }

    public static void Draw()
    {
        PluginMenu.Draw(PluginManager);
        SettingsMenu.Draw();
        PluginManager.Draw();
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