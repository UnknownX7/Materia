using Materia.Utilities;
using SharpDX.DXGI;

namespace Materia;

internal sealed class RenderManager : IDisposable
{
    private delegate nint PresentDelegate(nint swapChain, uint syncInterval, uint presentFlags);
    private readonly IMateriaHook<PresentDelegate>? presentHook;

    private delegate nint ResizeBuffersDelegate(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags);
    private readonly IMateriaHook<ResizeBuffersDelegate>? resizeBuffersHook;

    public delegate void PresentEventDelegate(nint swapChain, uint syncInterval, uint presentFlags);
    public event PresentEventDelegate? Present;

    public delegate void ResizeBuffersEventDelegate(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags);
    public event ResizeBuffersEventDelegate? PreResizeBuffers;
    public event ResizeBuffersEventDelegate? PostResizeBuffers;

    public SwapChain SwapChain { get; }

    public unsafe RenderManager()
    {
        // TODO: May have to change if ReShade ever takes off for this game
        // UnityPlayer.GetSwapChain
        var getSwapChain = (delegate* unmanaged<nint>)SigScanner.UnityPlayer.ScanText("E8 ?? ?? ?? ?? 4C 8B F0 48 85 C0 74 5C BA");
        var swapChain = getSwapChain();
        var vtbl = *(nint**)swapChain;

        var presentAddress = vtbl[8];
        presentHook = new MateriaHook<PresentDelegate>(presentAddress, PresentDetour);
        presentHook.Enable();

        var resizeBuffersAddress = vtbl[13];
        if (*(byte*)resizeBuffersAddress == 0xE9) // Steam fix
        {
            Util.WriteMemory<byte>(resizeBuffersAddress + 5, 0x90);
            Util.WriteMemory<byte>(resizeBuffersAddress + 6, 0x90);
        }
        resizeBuffersHook = new MateriaHook<ResizeBuffersDelegate>(resizeBuffersAddress, ResizeBuffersDetour);
        resizeBuffersHook.Enable();

        Logging.Info($"Swap chain: {swapChain:X}, Present: {presentAddress:X}, ResizeBuffers: {resizeBuffersAddress:X}");

        SwapChain = new SwapChain(swapChain);
    }

    private nint PresentDetour(nint swapChain, uint syncInterval, uint presentFlags)
    {
        Present?.Invoke(swapChain, syncInterval, presentFlags);
        return presentHook!.Original(swapChain, syncInterval, presentFlags);
    }

    private nint ResizeBuffersDetour(nint swapChain, uint bufferCount, uint width, uint height, uint newFormat, uint swapChainFlags)
    {
        PreResizeBuffers?.Invoke(swapChain, bufferCount, width, height, newFormat, swapChainFlags);
        var ret = resizeBuffersHook!.Original(swapChain, bufferCount, width, height, newFormat, swapChainFlags);
        PostResizeBuffers?.Invoke(swapChain, bufferCount, width, height, newFormat, swapChainFlags);
        return ret;
    }

    public void Dispose()
    {
        presentHook?.Dispose();
        resizeBuffersHook?.Dispose();
    }
}