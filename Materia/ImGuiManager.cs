using System.Reflection;
using ImGuiNET;
using ImGuiScene;
using PInvoke;
using Materia.Utilities;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using ImGuizmoNET;

namespace Materia;

internal sealed class ImGuiManager : IDisposable
{
    private RawDX11Scene? scene;

    public event Action? Draw;

    // TODO: This should be done in ImGuiScene, not here
    private readonly object drawLock = new();
    private DeviceContext deviceContext = null!;
    private RenderTargetView rtv = null!;
    private ImGui_Impl_DX11 imguiRenderer = null!;
    private ImGui_Input_Impl_Direct imguiInput = null!;
    private int targetWidth;
    private int targetHeight;

    public void Initialize(nint swapChain)
    {
        scene = new RawDX11Scene(swapChain)
        {
            UpdateCursor = false, // TODO: This doesn't work
            ImGuiIniPath = Path.Combine(Util.MateriaDirectory.FullName, "imgui.ini")
        };

        scene.OnBuildUI += OnBuildUi;
        scene.OnNewInputFrame += OnNewInputFrame;

        var sceneType = typeof(RawDX11Scene);
        deviceContext = (DeviceContext)sceneType.GetField(nameof(deviceContext), BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene)!;
        rtv = (RenderTargetView)sceneType.GetField(nameof(rtv), BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene)!;
        imguiRenderer = (ImGui_Impl_DX11)sceneType.GetField(nameof(imguiRenderer), BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene)!;
        imguiInput = (ImGui_Input_Impl_Direct)sceneType.GetField(nameof(imguiInput), BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene)!;

        var s = new SwapChain(swapChain);
        targetWidth = s.Description.ModeDescription.Width;
        targetHeight = s.Description.ModeDescription.Height;

        var io = ImGui.GetIO();
        io.SetPlatformImeDataFn = nint.Zero; // TODO: IME causes freezes
        io.ConfigFlags = ImGuiConfigFlags.None; // TODO: Viewports don't work
        io.FontGlobalScale = Materia.Config.UIScale;
    }

    public void Render(nint swapChain)
    {
        lock (drawLock)
        {
            if (scene == null)
                Initialize(swapChain);

            imguiRenderer.NewFrame();
            //OnNewRenderFrame?.Invoke();
            imguiInput.NewFrame(targetWidth, targetHeight);
            OnNewInputFrame();

            ImGui.NewFrame();
            ImGuizmo.BeginFrame();

            OnBuildUi();

            ImGui.Render();
        }
    }

    // TODO: Disable viewports if fullscreen or 1 monitor
    public void Present(nint swapChain, uint syncInterval, uint presentFlags)
    {
        lock (drawLock)
        {
            deviceContext.OutputMerger.SetRenderTargets(rtv);
            imguiRenderer.RenderDrawData(ImGui.GetDrawData());
            deviceContext.OutputMerger.SetRenderTargets((RenderTargetView?)null);
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
        }
    }

    public void PreResizeBuffers(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags)
    {
        lock (drawLock)
        {
            if (scene?.SwapChain.NativePointer != swapChain) return;
            scene.OnPreResize();
            rtv = null;
        }
    }

    public void PostResizeBuffers(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags)
    {
        lock (drawLock)
        {
            if (scene?.SwapChain.NativePointer != swapChain) return;
            targetWidth = (int)width;
            targetHeight = (int)height;
            scene.OnPostResize((int)width, (int)height);
            rtv = (RenderTargetView)typeof(RawDX11Scene).GetField(nameof(rtv), BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene)!;
        }
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