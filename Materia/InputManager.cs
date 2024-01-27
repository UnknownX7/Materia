using System.Runtime.InteropServices;
using Materia.Game;
using SharpDX.DXGI;

namespace Materia;

internal sealed class InputManager : IDisposable
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint GetWindowLongPtrW(nint hWnd, int nIndex);

    private delegate bool ConsoleEventDelegate(int type);
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate handler, bool add);
    private static ConsoleEventDelegate? onClose;

    private delegate nint WndProcDelegate(nint hWnd, uint msg, nint wParam, nint lParam);
    private readonly IMateriaHook<WndProcDelegate>? wndProcHook;

    public delegate void WndProcEventDelegate(nint hWnd, uint msg, nint wParam, nint lParam);
    public event WndProcEventDelegate? WndProc;

    public delegate nint? WndProcHandlerDelegate(nint hWnd, uint msg, nint wParam, nint lParam);
    public WndProcHandlerDelegate? WndProcHandler;

    public unsafe InputManager(SwapChain swapChain)
    {
        var wndProcAddress = GetWindowLongPtrW(swapChain.Description.OutputHandle, -4);
        Logging.Info($"WndProc: {wndProcAddress:X}");
        wndProcHook = new MateriaHook<WndProcDelegate>(wndProcAddress, WndProcDetour);
        wndProcHook.Enable();

        var getSettings = (delegate* unmanaged<nint, nint>)GameData.GetSymbolAddress("UnityEngine.InputSystem.InputSystem$$get_settings");
        var setBackgroundBehavior = (delegate* unmanaged<nint, int, nint, void>)GameData.GetSymbolAddress("UnityEngine.InputSystem.InputSettings$$set_backgroundBehavior");
        setBackgroundBehavior(getSettings(0), 2, 0);
#if DEBUG
        onClose = _ => WndProcDetour(0, 16, 0, 0) != 0;
        SetConsoleCtrlHandler(onClose, true);
#endif
    }

    private nint WndProcDetour(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        if (msg == 16) // Close
        {
            try
            {
                Materia.Dispose();
            }
            catch (Exception e)
            {
                Logging.Error($"Error while disposing\n{e}");
            }
        }

        WndProc?.Invoke(hWnd, msg, wParam, lParam);
        return WndProcHandler?.Invoke(hWnd, msg, wParam, lParam) ?? wndProcHook!.Original(hWnd, msg, wParam, lParam);
    }

    public void Dispose() => wndProcHook?.Dispose();
}