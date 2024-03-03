using ImGuiNET;
using ImGuiScene;
using PInvoke;
using Materia.Utilities;

namespace Materia;

internal sealed class ImGuiManager : IDisposable
{
    private RawDX11Scene? scene;

    public event Action? Draw;

    public void Initialize(nint swapChain)
    {
        scene = new RawDX11Scene(swapChain)
        {
            UpdateCursor = false, // TODO: This doesn't work
            ImGuiIniPath = Path.Combine(Util.MateriaDirectory.FullName, "imgui.ini")
        };

        scene.OnBuildUI += OnBuildUi;
        scene.OnNewInputFrame += OnNewInputFrame;

        var io = ImGui.GetIO();
        io.SetPlatformImeDataFn = nint.Zero; // TODO: IME causes freezes
        io.ConfigFlags = ImGuiConfigFlags.None; // TODO: Viewports don't work
        io.FontGlobalScale = Materia.Config.UIScale;
    }

    // TODO: Disable viewports if fullscreen or 1 monitor
    public void Present(nint swapChain, uint syncInterval, uint presentFlags)
    {
        if (scene == null)
            Initialize(swapChain);
        scene!.Render();
    }

    public void PreResizeBuffers(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags)
    {
        if (scene?.SwapChain.NativePointer != swapChain) return;
        scene.OnPreResize();
    }

    public void PostResizeBuffers(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags)
    {
        if (scene?.SwapChain.NativePointer != swapChain) return;
        scene.OnPostResize((int)width, (int)height);
    }

    public unsafe nint? WndProcHandler(nint hWnd, uint msg, nint wParam, nint lParam) // TODO: Clicking on ImGui prevents dragging the game window
    {
        if (scene == null) return null;

        var newRet = scene.ProcessWndProcW(hWnd, (User32.WindowMessage)msg, (void*)wParam, (void*)lParam);

        switch ((User32.WindowMessage)msg)
        {
            // The game processes keyboard input / mouse scroll from here
            case User32.WindowMessage.WM_INPUT when new RawInput(lParam).Data is { header.device: not 0 } data:
                switch (data.header.type)
                {
                    case 0 when ImGui.GetIO().WantCaptureMouse:
                        return nint.Zero;
                    case 1 when ImGui.GetIO().WantCaptureKeyboard:
                        return nint.Zero;
                }
                break;
        }

        return newRet;
    }

    private void OnBuildUi()
    {
        try
        {
            Draw?.Invoke();
        }
        catch (Exception e)
        {
            Logging.Error(e);
        }
    }

    private void OnNewInputFrame()
    {
        // TODO: Gamepad?
    }

    public void Dispose()
    {
        scene?.Dispose();
        scene = null;
    }
}