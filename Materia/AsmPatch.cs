using Materia.Utilities;

namespace Materia;

public sealed unsafe class AsmPatch : IDisposable
{
    public nint Address { get; }
    public byte[] NewBytes { get; }
    public byte[] OldBytes { get; }
    public bool IsEnabled { get; private set; }

    internal AsmPatch(nint address, IReadOnlyCollection<byte?> bytes)
    {
        var trimmedBytes = bytes.SkipWhile(b => !b.HasValue).ToArray();
        address += bytes.Count - trimmedBytes.Length;
        var ptr = (byte*)address;
        Address = address;
        OldBytes = Enumerable.Range(0, trimmedBytes.Length).Select(i => ptr[i]).ToArray();
        NewBytes = Enumerable.Range(0, trimmedBytes.Length).Select(i => trimmedBytes[i] ?? ptr[i]).ToArray();
    }

    public void Enable()
    {
        if (IsEnabled) return;
        Util.WriteMemory(Address, NewBytes);
        IsEnabled = true;
    }

    public void Disable()
    {
        if (!IsEnabled) return;
        Util.WriteMemory(Address, OldBytes);
        IsEnabled = false;
    }

    public void Toggle()
    {
        if (IsEnabled)
            Disable();
        else
            Enable();
    }

    public void Dispose() => Disable();
}